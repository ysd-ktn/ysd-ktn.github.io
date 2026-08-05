#!/usr/bin/env python3
"""
Notion の Markdown エクスポートを、 WRITING 用の記事 Markdown に変換する。

使い方:
    python3 script/notion-to-md.py <エクスポートしたzip> <slug> [--meta meta.json]

やること:
  1. zip を展開する (Notion の zip は UTF-8 フラグを立てずに UTF-8 名を入れて
     くるので、 標準の unzip では日本語ファイル名が化ける。 ここで手当てする)
  2. 先頭の H1 / プロパティ行 / ハッシュタグ行を frontmatter 側に移す
  3. 最初の画像をサムネイルとして本文から抜く
  4. 画像を WebP に変換して public/images/writing/<slug>/ へ置く
     + OGP 用に 1200×630 の JPG を書き出す
     (WebP は一部 SNS が OGP で読めないため、 共有カード用は JPG を別に持つ)
  5. Notion 特有の崩れを直す:
     - 複数行の引用で 2 行目以降に `>` が付かない → 付け直す
     - 引用の終端に残る空の `>` 行 → 削除
     - 見出し直後の `---` → 削除 (h2 の罫線は CSS が描くので二重になる)
     - `##見出し` のようにスペース無しの見出し → 太字に落とす
     - `[CLAUDE.md](http://claude.md/)` のような誤オートリンク → ただの文字に
  6. alt / キャプションを meta.json から流し込む
     (Notion のエクスポートは alt をファイル名にしてしまい、 キャプションは
      丸ごと失われるため、 元記事から拾ったものを外から与える)

meta.json の形:
    {
      "img1.png": {"alt": "...", "caption": "..."},
      ...
    }
"""

import argparse
import json
import pathlib
import re
import shutil
import sys
import tempfile
import urllib.parse
import zipfile

from PIL import Image

# 本文カラムは 680px。 2x ディスプレイでも足りるよう長辺 1600px で頭打ちにする
MAX_W = 1600
WEBP_Q = 82
# method は 0-6 で、 大きいほど圧縮率が上がる代わりに極端に遅くなる。
# 4 でも 6 との差は数 % なので、 記事を何本も回すことを優先して 4 にする。
WEBP_M = 4
OGP_SIZE = (1200, 630)   # X / Facebook / Slack 共通の推奨サイズ (1.91:1)
OGP_Q = 88


# ────────────────────────────────────────────────
# zip 展開
# ────────────────────────────────────────────────
def _extract(src: pathlib.Path, dest: pathlib.Path) -> None:
    with zipfile.ZipFile(src) as z:
        for info in z.infolist():
            name = info.filename
            if not (info.flag_bits & 0x800):        # UTF-8 フラグが無い
                name = name.encode("cp437").decode("utf-8")
            target = dest / name
            if info.is_dir():
                target.mkdir(parents=True, exist_ok=True)
                continue
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(z.read(info))


def unzip_utf8(src: pathlib.Path, dest: pathlib.Path) -> None:
    """Notion の zip を文字化けさせずに展開する (入れ子 zip も開く)

    Notion は "Part-1.zip" を内側にもう一枚挟むことがある。
    再帰で書くと「展開 → 中の zip を見つける → まだ消してないので同じものを
    また見つける」で無限ループになるため、 展開したそばから unlink して
    ループで回す。
    """
    dest.mkdir(parents=True, exist_ok=True)
    _extract(src, dest)

    for _ in range(8):                      # 自己参照 zip への保険
        nested = list(dest.rglob("*.zip"))
        if not nested:
            return
        for z_path in nested:
            _extract(z_path, dest)
            z_path.unlink()
    raise RuntimeError("入れ子 zip が深すぎるごわす")


# ────────────────────────────────────────────────
# 本文の整形
# ────────────────────────────────────────────────
def fix_blockquotes(lines: list[str]) -> list[str]:
    """複数行の引用で 2 行目以降に `>` が落ちるのを直す"""
    out, in_quote = [], False
    for line in lines:
        stripped = line.strip()
        if stripped.startswith(">"):
            # 終端に残る空の `>` は捨てて引用を閉じる
            if stripped in (">", "> "):
                in_quote = False
                continue
            in_quote = True
            out.append(line)
            continue
        if in_quote:
            if not stripped:                 # 空行では閉じない (次行も引用の続き)
                out.append(">")
                continue
            if stripped.startswith(("#", "!", "|")) or re.match(r"^\s{0,3}$", line):
                in_quote = False             # 明らかに引用の外の要素なら閉じる
            else:
                out.append("> " + line)
                continue
        out.append(line)

    # 末尾に付いた空の引用行を落とす
    while out and out[-1].strip() == ">":
        out.pop()
    return out


def demote_headings(md: str) -> str:
    """本文が H1 を章に使っている場合、 見出しを 1 段ずつ下げる。

    記事によって Notion 側の書き方が違い、 章を `#` で書くものと `##` で
    書くものが混在している。 サイト側はページタイトルが H1、 章が H2、
    小見出しが H3 という前提 (目次は H2 だけを拾う) なので、
    本文に H1 が残っていると目次が拾う階層がズレる。

    深い方から順に置換しないと `#` → `##` した行を次のルールが
    さらに `###` にしてしまうので、 h5 → h6, h4 → h5 ... の順で回す。
    """
    if not re.search(r"^# ", md, re.M):
        return md
    for level in range(5, 0, -1):
        md = re.sub(r"^#{%d} " % level, "#" * (level + 1) + " ", md, flags=re.M)
    return md


def unfence_lists(md: str) -> str:
    """日本語の引用がコードブロックとして書き出されたものを引用に戻す。

    Notion は引用や囲み枠の中身を、 なぜか ```jsx や ```html といった
    プログラミング言語タグ付きのコードブロックとして書き出すことがある。
    そのままだと日本語の文章が等幅フォントの黒い箱に入り、 構文強調まで
    かかってコードとして読まれてしまう。

    判定は「言語タグが付いている」かつ、 次のどちらかを満たすこと:
      - 中身が全部リスト項目 (`- ` や `1. ` で始まる)
      - 中身が日本語主体 (CJK が 30% 以上)
    サービス名の羅列のように日本語が薄いリストもあるので、 リスト判定は
    別に持たないと取りこぼす。

    ユーザーが意図して置いた本物のコードブロック (コマンド例、 ファイル一覧、
    フェーズ一覧など) は Notion では言語タグ無しで書き出されるため、
    この条件に引っかからずそのまま残る。
    """

    def conv(m: re.Match) -> str:
        lang, body = m.group(1), m.group(2)
        if not lang:
            return m.group(0)  # タグ無し = 本物のコードブロック

        lines = [l for l in body.split("\n") if l.strip()]
        if not lines:
            return m.group(0)

        all_list = all(re.match(r"\s*(-|\*|\d+\.)\s+\S", l) for l in lines)
        text = "".join(lines)
        cjk_ratio = len(re.findall(r"[ぁ-んァ-ヶ一-龠]", text)) / max(len(text), 1)

        if not (all_list or cjk_ratio >= 0.3):
            return m.group(0)

        return "\n".join("> " + l.strip() for l in lines)

    return re.sub(r"^```(\w*)\n(.*?)^```", conv, md, flags=re.M | re.S)


def fix_captions(md: str) -> str:
    """画像キャプションの重複と書式を整える。

    Notion は記事によって挙動が違い、 キャプションを画像の下に素のテキストで
    残す場合と、 丸ごと落とす場合がある。 落とす記事のために --meta で
    外から与えられるようにしてあるが、 残る記事では両方入って二重になる。

    1. `*X*` の直後に素の `X` が続いていたら、 素の方を捨てる (二重解消)
    2. 画像の直後の素のテキストが alt と同じなら、 それは Notion が
       残したキャプションなので斜体にする
    """
    # 1. 斜体キャプションと同じ内容の素の段落を削除
    md = re.sub(r"^(\*(.+?)\*)[ \t]*\n\n+\2[ \t]*$", r"\1", md, flags=re.M)

    # 2. 画像 → 空行 → alt と同じ素のテキスト、 を斜体に格上げ
    def italicize(m: re.Match) -> str:
        return f"{m.group(1)}\n\n*{m.group(2)}*"

    md = re.sub(
        r"^(!\[([^\]]+)\]\([^)]*\))[ \t]*\n\n+\2[ \t]*$",
        italicize,
        md,
        flags=re.M,
    )
    return md


def clean_body(md: str) -> str:
    lines = md.split("\n")
    lines = fix_blockquotes(lines)
    md = "\n".join(lines)

    md = demote_headings(md)

    # 見出し全体が太字で囲まれているものは ** を外す。
    # `## **💡 こんな人に読んでほしい**` のような書き方が Notion から出てくるが、
    # そのままだと <h2><strong> になり、 本文の strong に当てている accent 色が
    # 見出し全体に乗って派手になる。 意味も無いので剥がす。
    md = re.sub(r"^(#{1,6} )\*\*(.+?)\*\*\s*$", r"\1\2", md, flags=re.M)

    # 見出し直後の水平線を落とす (h2 の罫線は CSS が描く)。
    # 末尾を `\s*\n` にすると見出しと本文の間の空行まで食ってしまうので、
    # `---` の行末だけを消す `[ \t]*\n` にする。
    md = re.sub(r"(^#{2,4} .+\n)\n*---[ \t]*\n", r"\1", md, flags=re.M)

    # `##話し方` のようにスペース無しのものは見出しとして解釈されない → 太字に。
    # `(?![\s#])` が要点。 `(?=\S)` だと `## 見出し` に対して `#{1,6}` が
    # 1 個だけマッチし、 次の `#` を非空白と見なして正しい見出しまで壊す。
    md = re.sub(r"^(>?\s*)#{1,6}(?![\s#])(.+)$", r"\1**\2**", md, flags=re.M)

    # Notion の誤オートリンク対策。
    # `CLAUDE.md` のようなファイル名を勝手に `http://claude.md/` へ
    # リンクしてしまうので、 それだけを狙って素の文字に戻す。
    # 「表示文字とリンク先が一致」だけを条件にすると
    # `[ysd-ktn.github.io](https://ysd-ktn.github.io/)` のような
    # 本物のリンクまで消えるため、 拡張子で絞り込む。
    fileish = r"\.(md|json|js|ts|jsx|tsx|css|scss|html|astro|py|sh|yml|yaml|toml)$"

    def kill_autolink(m):
        text, href = m.group(1), m.group(2)
        bare = text.lower().strip("`*「」 ")
        host = re.sub(r"^https?://|/$", "", href).lower()
        if host == bare and re.search(fileish, bare):
            return text
        return m.group(0)

    md = re.sub(r"\[([^\]]+)\]\((https?://[^)]+)\)", kill_autolink, md)

    # 句読点だけを囲んだ強調を外す。
    # Notion の書き出しには `しました**。**` のような残骸が混ざり、
    # そのままだと句点だけが accent 色で光る。
    md = re.sub(r"\*\*([。、！？!?,.\s]+)\*\*", r"\1", md)

    md = unfence_lists(md)
    md = fix_captions(md)

    # 3 行以上の空行を 2 行に詰める
    md = re.sub(r"\n{3,}", "\n\n", md)
    return md.strip() + "\n"


# ────────────────────────────────────────────────
# 画像
# ────────────────────────────────────────────────
def to_webp(src: pathlib.Path, dst: pathlib.Path) -> tuple[int, int]:
    im = Image.open(src)
    if im.mode in ("RGBA", "LA", "P"):
        im = im.convert("RGB")
    if im.width > MAX_W:
        im = im.resize((MAX_W, round(im.height * MAX_W / im.width)), Image.LANCZOS)
    dst.parent.mkdir(parents=True, exist_ok=True)
    im.save(dst, "WEBP", quality=WEBP_Q, method=WEBP_M)
    return im.size


def to_ogp(src: pathlib.Path, dst: pathlib.Path) -> None:
    """1200×630 の JPG を書き出す。 比率が違う場合は中央を切り出す"""
    im = Image.open(src).convert("RGB")
    tw, th = OGP_SIZE
    scale = max(tw / im.width, th / im.height)
    im = im.resize((round(im.width * scale), round(im.height * scale)), Image.LANCZOS)
    left, top = (im.width - tw) // 2, (im.height - th) // 2
    im.crop((left, top, left + tw, top + th)).save(
        dst, "JPEG", quality=OGP_Q, optimize=True, progressive=True
    )


# ────────────────────────────────────────────────
def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("zip", type=pathlib.Path)
    ap.add_argument("slug")
    ap.add_argument("--meta", type=pathlib.Path, help="alt / caption を与える JSON")
    ap.add_argument("--root", type=pathlib.Path, default=pathlib.Path.cwd())
    args = ap.parse_args()

    meta = json.loads(args.meta.read_text("utf-8")) if args.meta else {}
    # 展開先はリポジトリの外に取る。 共有フォルダ上に作ると環境によっては
    # 後片付けの削除が権限で弾かれ、 ゴミが残り続けるため。
    work = pathlib.Path(tempfile.mkdtemp(prefix="notion-"))
    unzip_utf8(args.zip, work)

    md_files = list(work.rglob("*.md"))
    if len(md_files) != 1:
        print(f"!! .md が {len(md_files)} 件見つかった。 1 記事ずつ変換してほしいごわす")
        return 1
    src_md = md_files[0]
    raw = src_md.read_text("utf-8")

    # ── frontmatter に上げる情報を拾う ───────────────
    title = re.search(r"^# (.+)$", raw, re.M)
    title = title.group(1).strip() if title else args.slug
    note_url = re.search(r"^URL:\s*(\S+)", raw, re.M)
    note_url = note_url.group(1).split("?")[0] if note_url else ""
    note_tags = re.findall(r"#([^\s#]+)", re.search(r"^#[^\n]*$", raw, re.M).group(0)) \
        if re.search(r"^#[^\s]", raw, re.M) else []

    # ── 冒頭のメタ領域を落とす ──────────────────────
    body = re.sub(r"^# .+$", "", raw, count=1, flags=re.M)
    # Notion のプロパティ行。 データベースの列名がそのまま出てくるので、
    # 記事ごとに増える可能性がある (制作期間 / カテゴリ など)。
    body = re.sub(
        r"^(ステータス|URL|アカウント|タグ|作成日|公開日|制作期間|カテゴリ|"
        r"公開状態|Status|Date|Tags|Created|Published):.*$",
        "",
        body,
        flags=re.M,
    )
    body = re.sub(r"^#[^\s].*$", "", body, count=1, flags=re.M)   # ハッシュタグ行

    # ── 画像を差し替えつつ変換 ─────────────────────
    img_dir_url = f"/images/writing/{args.slug}"
    out_img = args.root / "public" / "images" / "writing" / args.slug
    out_img.mkdir(parents=True, exist_ok=True)

    order: list[str] = []

    def swap(m: re.Match) -> str:
        rel = urllib.parse.unquote(m.group(2))
        fname = rel.split("/")[-1]
        src = src_md.parent / rel
        if not src.exists():
            hits = list(work.rglob(fname))
            if not hits:
                print(f"!! 画像が見つからない: {fname}")
                return m.group(0)
            src = hits[0]

        order.append(fname)
        stem = "thumb" if len(order) == 1 else f"img{len(order) - 1}"
        w, h = to_webp(src, out_img / f"{stem}.webp")
        if len(order) == 1:
            to_ogp(src, out_img / "ogp.jpg")
            return "@@THUMB@@"            # 本文からは抜いてサムネに回す

        # alt の決め方:
        #   1. meta.json で指定があればそれ
        #   2. Notion の alt がファイル名でなければ流用する
        #      (記事によっては Notion がキャプションを alt に入れてくれている)
        #   3. どちらも無ければ空にする
        info = meta.get(fname, {})
        notion_alt = m.group(1).strip()
        if re.search(r"\.(png|jpe?g|gif|webp|svg)$", notion_alt, re.I):
            notion_alt = ""
        alt = info.get("alt") or notion_alt
        line = f"![{alt}]({img_dir_url}/{stem}.webp)"
        if info.get("caption"):
            line += f"\n\n*{info['caption']}*"
        print(f"   {fname:<12} → {stem}.webp  {w}×{h}")
        return line

    body = re.sub(r"!\[([^\]]*)\]\(([^)]+)\)", swap, body)
    body = body.replace("@@THUMB@@", "")
    body = clean_body(body)

    # ── 書き出し ────────────────────────────────
    # 最初の本文らしい段落を excerpt に使う (見出し / 画像 / 引用 / 罫線は飛ばす)
    excerpt = next(
        (l.strip() for l in body.split("\n")
         if l.strip()
         and not l.startswith(("#", "!", ">", "*", "-", "|", "`"))
         and l.strip() != "---"),
        "",
    )[:120].replace('"', "'")

    fm = "\n".join([
        "---",
        f'title: "{title}"',
        'date: "TODO"                # 例: 2026-06-21',
        'tags: []                    # 例: ["AI", "DESIGN"]',
        f'thumbnail: "{img_dir_url}/thumb.webp"',
        f'ogpImage: "{img_dir_url}/ogp.jpg"',
        f'noteUrl: "{note_url}"',
        f'excerpt: "{excerpt}"',
        "draft: false",
        f'# note のタグ: {" ".join(note_tags)}' if note_tags else "",
        "---",
        "",
    ])
    fm = "\n".join(l for l in fm.split("\n") if l != "")  + "\n\n"

    out_md = args.root / "src" / "content" / "writing" / f"{args.slug}.md"
    out_md.parent.mkdir(parents=True, exist_ok=True)
    out_md.write_text(fm + body, "utf-8")

    shutil.rmtree(work, ignore_errors=True)
    print(f"\n✓ {out_md.relative_to(args.root)}")
    print(f"✓ {out_img.relative_to(args.root)}/ に画像 {len(order)} 枚 + ogp.jpg")
    print("\n→ frontmatter の date / tags を埋めてほしいごわす")
    return 0


if __name__ == "__main__":
    sys.exit(main())
