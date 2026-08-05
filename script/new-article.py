#!/usr/bin/env python3
"""
新しい記事の雛形を作る / 画像を最適化する。

Notion からの移行ではなく、 最初から Markdown で書く場合に使う。

── 使い方 ────────────────────────────────────────────
1) 雛形を作る
       python3 script/new-article.py my-slug

   できるもの:
       src/content/writing/my-slug.md          記事の雛形
       public/images/writing/my-slug/_raw/     画像の置き場 (作業用)

2) 記事を書く。 画像は _raw/ に放り込む。
   サムネイルにしたい 1 枚は `thumb` で始まる名前にする
   (thumb.png / thumb-01.jpg など)。

3) 画像を最適化する
       python3 script/new-article.py my-slug --images

   やること:
       _raw/*.png|jpg|jpeg|webp → 長辺 1600px の WebP に変換
       thumb* → thumb.webp + ogp.jpg (1200×630) を書き出す
       _raw/ はそのまま残す (元画像を捨てない)

   本文からは ![説明](/images/writing/my-slug/xxx.webp) で参照する。
   変換後にファイル名の一覧を表示するのでコピペすればよい。

── なぜ ogp.jpg が別に要るのか ─────────────────────
   表示用は WebP で軽くしたいが、 OGP (SNS の共有カード) 用の画像は
   WebP を読めないクローラがまだいる。 カードが画像なしで出てしまうので、
   共有用だけ JPG を別に持つ。 サイズ 1200×630 は X / Facebook / Slack
   共通の推奨値 (1.91:1)。
"""

import argparse
import pathlib
import re
import sys

from PIL import Image

MAX_W = 1600
WEBP_Q = 82
WEBP_M = 4
OGP_SIZE = (1200, 630)
OGP_Q = 88
EXTS = {'.png', '.jpg', '.jpeg', '.webp'}

TEMPLATE = '''---
title: "{title}"
date: {today}
tags: ["DESIGN"]
thumbnail: "/images/writing/{slug}/thumb.webp"
ogpImage: "/images/writing/{slug}/ogp.jpg"
excerpt: "一覧のカードに出る紹介文。2行に切り詰めて表示されるので、100字前後が目安。"
draft: true
---

## はじめに

ここから本文。

見出しは H2 が章の区切りで、そのまま目次と進捗レールになる。
H3 は目次には載らない小見出し。絵文字を入れても URL は綺麗に保たれる。

![画像の説明を必ず書く](/images/writing/{slug}/img1.webp)

*画像の下に斜体で1行書くとキャプションになる*

## まとめ

<!--
公開するときは frontmatter の draft を false にする。
draft: true の間は本番ビルドに含まれない (dev では見える)。
-->
'''


def optimize(slug: str, root: pathlib.Path) -> int:
    out = root / 'public' / 'images' / 'writing' / slug
    raw = out / '_raw'
    if not raw.is_dir():
        print(f'!! {raw.relative_to(root)} が無いごわす。 先に雛形を作ってほしいごわす')
        return 1

    files = sorted(p for p in raw.iterdir() if p.suffix.lower() in EXTS)
    if not files:
        print(f'!! {raw.relative_to(root)} に画像が無いごわす')
        return 1

    thumbs = [p for p in files if p.stem.lower().startswith('thumb')]
    if not thumbs:
        print('!! サムネイルが見つからないごわす。')
        print('   1枚を thumb.png のように thumb で始まる名前にしてほしいごわす')
        return 1
    thumb = thumbs[0]
    others = [p for p in files if p != thumb]

    print(f'サムネイル: {thumb.name}')
    w, h = to_webp(thumb, out / 'thumb.webp')
    print(f'   → thumb.webp  {w}×{h}')
    to_ogp(thumb, out / 'ogp.jpg')
    print(f'   → ogp.jpg     {OGP_SIZE[0]}×{OGP_SIZE[1]} (SNS 共有カード用)')

    print('\n本文用:')
    refs = []
    for i, p in enumerate(others, 1):
        name = f'img{i}.webp'
        w, h = to_webp(p, out / name)
        print(f'   {p.name:<24} → {name}  {w}×{h}')
        refs.append(name)

    if refs:
        print('\n本文にはこう書くごわす:')
        for name in refs:
            print(f'   ![説明を書く](/images/writing/{slug}/{name})')
    return 0


def to_webp(src: pathlib.Path, dst: pathlib.Path) -> tuple[int, int]:
    im = Image.open(src)
    if im.mode in ('RGBA', 'LA', 'P'):
        im = im.convert('RGB')
    if im.width > MAX_W:
        im = im.resize((MAX_W, round(im.height * MAX_W / im.width)), Image.LANCZOS)
    dst.parent.mkdir(parents=True, exist_ok=True)
    im.save(dst, 'WEBP', quality=WEBP_Q, method=WEBP_M)
    return im.size


def to_ogp(src: pathlib.Path, dst: pathlib.Path) -> None:
    """1200×630 の JPG。 比率が違う場合は中央を切り出す"""
    im = Image.open(src).convert('RGB')
    tw, th = OGP_SIZE
    scale = max(tw / im.width, th / im.height)
    im = im.resize((round(im.width * scale), round(im.height * scale)), Image.LANCZOS)
    left, top = (im.width - tw) // 2, (im.height - th) // 2
    im.crop((left, top, left + tw, top + th)).save(
        dst, 'JPEG', quality=OGP_Q, optimize=True, progressive=True
    )


def scaffold(slug: str, root: pathlib.Path) -> int:
    from datetime import date

    md = root / 'src' / 'content' / 'writing' / f'{slug}.md'
    if md.exists():
        print(f'!! {md.relative_to(root)} は既にあるごわす')
        return 1

    md.parent.mkdir(parents=True, exist_ok=True)
    md.write_text(
        TEMPLATE.format(slug=slug, title=slug.replace('-', ' '), today=date.today().isoformat()),
        'utf-8',
    )
    raw = root / 'public' / 'images' / 'writing' / slug / '_raw'
    raw.mkdir(parents=True, exist_ok=True)
    (raw / '.gitkeep').touch()

    print(f'✓ {md.relative_to(root)}')
    print(f'✓ {raw.relative_to(root)}/')
    print('\n次にやること:')
    print(f'  1. {md.relative_to(root)} を書く')
    print(f'  2. 画像を {raw.relative_to(root)}/ に置く (サムネは thumb.png など)')
    print(f'  3. python3 script/new-article.py {slug} --images')
    print('  4. npm run dev で確認 → draft: false にして push')
    return 0


def main() -> int:
    ap = argparse.ArgumentParser(description='記事の雛形作成 / 画像最適化')
    ap.add_argument('slug', help='URL になる文字列 (英小文字とハイフン)')
    ap.add_argument('--images', action='store_true', help='_raw/ の画像を最適化する')
    ap.add_argument('--root', type=pathlib.Path, default=pathlib.Path.cwd())
    args = ap.parse_args()

    if not re.fullmatch(r'[a-z0-9][a-z0-9-]*', args.slug):
        print('!! slug は英小文字・数字・ハイフンだけにしてほしいごわす (例: design-system)')
        return 1

    return optimize(args.slug, args.root) if args.images else scaffold(args.slug, args.root)


if __name__ == '__main__':
    sys.exit(main())
