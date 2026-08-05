# サイト更新ガイド

このサイトを自分で更新するための手引き。コードの知識は要らない範囲でまとめてある。

迷ったら **「本文は Markdown、それ以外の文言は JSON」** と覚えておけばだいたい合っている。

---

## 目次

- [0. 最初に覚えること](#0-最初に覚えること)
- [1. 記事を新しく書く](#1-記事を新しく書く)
- [2. Notion で書いた記事を持ってくる](#2-notion-で書いた記事を持ってくる)
- [3. 公開済みの記事を直す](#3-公開済みの記事を直す)
- [4. note の記事を一覧に並べる（移行はしない）](#4-note-の記事を一覧に並べる移行はしない)
- [5. サイトの文言を変える](#5-サイトの文言を変える)
- [6. 記事の書き方（記法メモ）](#6-記事の書き方記法メモ)
- [7. 公開する](#7-公開する)
- [8. 困ったとき](#8-困ったとき)

---

## 0. 最初に覚えること

### どこに何があるか

```
src/content/            ← ここを触れば内容が変わる
  writing/*.md            記事の本文
  profile.json            名前・拠点・ツール・リード文など
  timeline.json           経歴・展示歴
  contact.json            連絡先

public/images/writing/  ← 記事の画像

script/                 ← 作業を楽にする道具
  new-article.py          新しい記事の雛形を作る／画像を最適化する
  notion-to-md.py         Notion の書き出しを記事に変換する

src/components/         ← 見た目。ここは基本触らない
src/pages/              ← ページの骨組み。ここも基本触らない
```

**`src/content/` と `public/images/` の外は触らなくていい。**
「ここの文言を変えたいのに `src/content/` に無い」という場合は、そもそも
コンテンツとして外に出せていないということなので、直してもらうこと。

### 作業を始める前

ターミナルでこのフォルダを開いて、以下を実行する。

```sh
npm run dev
```

ブラウザで `http://localhost:4321` を開くと、編集した内容がその場で反映される。
作業中はこれを開きっぱなしにしておく。止めるときは `Ctrl + C`。

> **注意**
> `src/content.config.ts` を変更したときや、Markdown の変換設定を変えたときは、
> 開発サーバーを再起動しないと反映されない。おかしいと思ったら一度止めて開き直す。

---

## 1. 記事を新しく書く

### ステップ1 — 雛形を作る

`design-system` の部分が URL になる。英小文字・数字・ハイフンだけを使う。

```sh
python3 script/new-article.py design-system
```

これで2つできる。

| できるもの | 何に使うか |
|:--|:--|
| `src/content/writing/design-system.md` | 記事の本文を書くファイル |
| `public/images/writing/design-system/_raw/` | 画像を放り込む場所 |

### ステップ2 — 本文を書く

`src/content/writing/design-system.md` を開くと、上に囲みがある。ここが記事の設定。

```yaml
---
title: "記事のタイトル"
date: 2026-08-05
tags: ["DESIGN"]
thumbnail: "/images/writing/design-system/thumb.webp"
ogpImage: "/images/writing/design-system/ogp.jpg"
excerpt: "一覧のカードに出る紹介文。100字前後が目安。"
draft: true
---
```

| 項目 | 説明 |
|:--|:--|
| `title` | 記事タイトル |
| `date` | 公開日。`2026-08-05` の形式 |
| `tags` | `DESIGN` `CAREER` `AI` から1〜3個。**これ以外を書くとエラーになる** |
| `thumbnail` | サムネイル画像。ステップ3で自動生成される |
| `ogpImage` | SNS で共有されたときのカード画像。これも自動生成 |
| `excerpt` | 一覧カードの紹介文 |
| `draft` | `true` の間は公開されない。書き終わったら `false` にする |

囲みから下が本文。書き方は [6. 記事の書き方](#6-記事の書き方記法メモ) を参照。

### ステップ3 — 画像を入れる

`public/images/writing/design-system/_raw/` に画像を放り込む。

- **サムネイルにする1枚は `thumb` で始まる名前にする**（`thumb.png` など）
- 他の画像の名前は何でもいい

全部置いたら、次を実行する。

```sh
python3 script/new-article.py design-system --images
```

やってくれること。

- 画像を軽い形式（WebP）に変換する。だいたい 1/10 くらいになる
- SNS 共有用の画像（`ogp.jpg`）を作る
- 本文に貼り付ける行を一覧で出してくれるので、コピペする

元の画像は `_raw/` にそのまま残るので、あとから作り直せる。

### ステップ4 — 公開する

`npm run dev` で確認して問題なければ、`draft: true` を `draft: false` に変える。
あとは [7. 公開する](#7-公開する) へ。

---

## 2. Notion で書いた記事を持ってくる

Notion で書いたものを記事にする場合はこちら。

### ステップ1 — Notion から書き出す

記事ページの右上「**…**」→「**エクスポート**」→ 形式は「**Markdown & CSV**」。
zip ファイルがダウンロードされる。

### ステップ2 — 変換する

```sh
python3 script/notion-to-md.py ~/Downloads/Export-xxxx.zip design-system
```

Notion の書き出しは崩れている箇所が多いので、スクリプト側で直している。

- 章が `#` で書かれていたら見出しを1段下げる
- 引用がコードブロックとして書き出されるのを引用に戻す
- 見出し直後の余計な区切り線を消す
- 見出し全体を囲んでいる太字を外す
- ファイル名がリンクになってしまったものを普通の文字に戻す
- 画像を WebP に変換し、SNS 共有用の JPG も作る

### ステップ3 — 手で埋める

変換が終わったら `date` と `tags` が空なので埋める。

**Notion の書き出しは画像の説明文（alt）とキャプションが消えることがある。**
気になる場合は `--meta` で外から渡せる。

```json
// meta.json
{
  "img1.png": { "alt": "画像の説明", "caption": "画像の下に出る文" }
}
```

```sh
python3 script/notion-to-md.py ~/Downloads/Export-xxxx.zip design-system --meta meta.json
```

---

## 3. 公開済みの記事を直す

`src/content/writing/` の該当ファイルを開いて直すだけ。

**画像を差し替えるとき** は、`_raw/` に新しい画像を置いて
`python3 script/new-article.py <slug> --images` をもう一度実行する。

**記事を非公開に戻すとき** は `draft: true` にする。

**記事を消すとき** はファイルと `public/images/writing/<slug>/` を削除する。
ただし削除すると URL が消えるので、共有されていた場合はリンク切れになる。

---

## 4. note の記事を一覧に並べる（移行はしない）

note にあるままで、一覧にだけ載せたい場合。
`src/content/writing/` に本文なしのファイルを作る。

```yaml
---
title: "記事のタイトル"
date: 2024-06-28
tags: ["DESIGN"]
thumbnail: "https://assets.st-note.com/.../rectangle_large_type_2_xxxx.png"
externalUrl: "https://note.com/ysd_ktn/n/nxxxxxxxx"
excerpt: "一覧のカードに出る紹介文。"
draft: false
---
```

ポイントは **`externalUrl` を書くこと**。これがあると個別ページは作られず、
一覧のカードから直接 note に飛ぶ。カードには `NOTE ↗` と出る。

`thumbnail` は note の画像 URL をそのまま指してよい。
note の記事ページで画像を右クリック →「画像アドレスをコピー」で取れる。

> あとから自サイトに移行したくなったら、`externalUrl` を消して `noteUrl` に
> 書き換え、本文と `ogpImage` を足せばそのまま内部記事になる。

---

## 5. サイトの文言を変える

記事以外の文言はすべて `src/content/` の JSON にある。

| 変えたいもの | ファイル | 項目 |
|:--|:--|:--|
| 名前・肩書き・拠点 | `profile.json` | `nameFirst` `roleHero` `locationJp` など |
| 得意分野・使用ツール | `profile.json` | `focus` `tools` |
| About の WRITING の文章 | `profile.json` | `writingIntro` |
| 記事一覧ページのリード文 | `profile.json` | `writingLede`（1要素＝1行。画面に出る文） |
| 記事一覧ページの検索用説明文 | `profile.json` | `writingDescription`（検索結果や SNS カードに出る文） |
| note プロフィールへのリンク | `profile.json` | `noteProfileUrl` |
| 最終更新日 | `profile.json` | `lastUpdate` |
| 経歴・展示歴 | `timeline.json` | 配列に足す。`order` が大きいほど上 |
| メール・SNS | `contact.json` | 配列に足す。`order` が小さいほど上 |

JSON を編集するときの注意。

- 文字は必ず `"ダブルクォート"` で囲む
- 項目の区切りのカンマ `,` を忘れない。**最後の項目にはカンマを付けない**
- 決められた形から外れるとビルドが止まる。これは「間違いに気づける仕組み」なので、
  エラーが出たら落ち着いてメッセージを読む

### 画面に出る文と、検索結果に出る文は別

`writingLede` は**画面に出る文**、`writingDescription` は**検索結果や SNS のカードに出る文**。
読む相手も目的も違うので分けてある。

- 画面のリード文 → すぐ下にタグのフィルタ行があるので、話題を並べると重複する。
  フィルタからは読み取れないこと（書き手の態度）だけを短く書く
- 検索用の説明文 → 検索で拾われたい語を入れて 120 字前後

### OGP 画像（SNS で共有されたときの画像）を差し替えるとき

| 対象 | ファイル |
|:--|:--|
| サイト全体 | `public/og-image.png` |
| 記事ごと | `public/images/writing/<slug>/ogp.jpg`（スクリプトが自動生成） |

**1200×630 で作ること。** `Layout.astro` がこのサイズを申告しているので、違うサイズだと
SNS 側の表示がずれる。透過（背景が透明）は使わない。SNS 側で黒や白に潰れる。

差し替えてもファイル名が同じ場合、SNS 側が古い画像をキャッシュしていることがある
（X は最大7日ほど）。数日待っても直らないときは `og-image.png?v=2` のように
末尾を変えると別の画像として取り直してくれる。

---

## 6. 記事の書き方（記法メモ）

本文は Markdown で書く。よく使うものだけ。

```markdown
## 章の見出し（目次に載る。絵文字を付けてよい）

### 小見出し（目次には載らない）

普通の文章。**太字にするとアクセントカラーになる。**

改行はそのまま改行になる。
（Notion や note と同じ感覚で書ける）

- 箇条書き
- ふたつめ

> 引用。左にアクセントカラーの線が付く。
> 複数行はすべての行に > を付ける。

![画像の説明を必ず書く](/images/writing/スラッグ/img1.webp)

*画像の下に斜体で1行書くとキャプションになる*

[文中のリンク](https://example.com)は普通のリンクになる。

[単独で置いたリンク](https://example.com)

`コード` や、

```
コードブロック
```
```

### 覚えておくと便利なこと

**見出しは `##` が章、`###` が小見出し。**
`##` だけが目次に載り、そのまま読了バーになる。`#` は使わない（記事タイトルが `#` なので）。

**リンクを段落に1つだけ置くとカードになる。**
文章の途中に混ぜたリンクは普通のリンクのまま。使い分けはこれだけ。

**見出しに絵文字を入れても URL は綺麗に保たれる。**
`## 🔧 前提` と書いても、リンクは `#前提` になる。

**画像の説明文は必ず書く。**
目が見えない人の読み上げに使われるほか、画像が読み込めなかったときにも表示される。

---

## 7. 公開する

`master` ブランチに push すると、GitHub Actions が自動でビルドして公開する。
手作業でのアップロードは要らない。

```sh
git add .
git commit -m "Add design system article"
git push
```

コミットメッセージは英語で、1行で簡潔に。`Add 〜` `Fix 〜` `Update 〜` の形。

**push したら必ずこの2つを確認する。**

```sh
git status          # "up to date with 'origin/master'" になっていればOK
git ls-remote origin master   # 手元の最新コミットと同じ番号が出ればOK
```

`ahead` と出ていたらまだ届いていない。ターミナルを遡ってエラーを探すこと
（成功したように見えて途中で切れていることがある）。

反映されるまで 1〜2分かかる。GitHub のリポジトリの「Actions」タブで進行状況が見られる。
緑のチェックが付けば完了。赤い × が出たら [8. 困ったとき](#8-困ったとき) へ。

公開前に本番と同じものを確認したいときは、

```sh
npm run build
npm run preview
```

---

## 8. 困ったとき

### 「Cannot read properties of undefined」と出る

コンテンツのファイルが読めていない。だいたい次のどれか。

1. JSON か Markdown の書き方が決められた形から外れている
   → ターミナルの上の方にもっと詳しいエラーが出ているので読む
2. ファイルを消した／名前を変えた
3. 設定を変えたのに開発サーバーが古いまま
   → `Ctrl + C` で止めて `npm run dev` で開き直す

### 直したのに画面が変わらない

Astro は変換済みの記事や読み込んだ設定を保存して使い回す。
記事の文章を直しただけなら普通に反映されるが、**変換の仕組み側**
（`src/lib/` のプラグインや `astro.config.mjs`）を触ったときは
古いものが残り続けることがある。開発サーバーを止めて、

```sh
rm -rf .astro node_modules/.vite
npm run dev
```

**`node_modules/.vite` も消すのが大事。** `.astro` だけ消しても
プラグインの修正が効かず、「直したのに変わらない」で延々ハマる。

### push が「RPC failed; HTTP 400」で止まる

```
error: RPC failed; HTTP 400 curl 22 The requested URL returned error: 400
fatal: the remote end hung up unexpectedly
```

一度に送るデータが Git の送信バッファ（初期値 1MB）を超えると出る。
画像をたくさん追加した回に起きやすい。一度だけ設定すれば以後は出ない。

```sh
git config --global http.postBuffer 524288000
git push origin master
```

それでも駄目なら通信方式を固定する。

```sh
git config --global http.version HTTP/1.1
```

### タグでエラーが出る

`tags` に使えるのは `DESIGN` `CAREER` `AI` の3つだけ。
増やしたい場合は仕組み側の変更が必要なので相談すること。

### 画像が重い／表示が遅い

`_raw/` の中の元画像を直接本文から参照していないか確認する。
本文が指すべきは変換後の `img1.webp` などで、`_raw/` の中身ではない。

### 記事の URL を変えたい

ファイル名がそのまま URL になる。`design-system.md` なら `/writing/design-system/`。
ただし **公開後に変えると前の URL がリンク切れになる**ので、共有済みなら避ける。

---

## 補足：この構成にした理由

- **本文を Markdown にした** — 見た目と中身を分けておくと、デザインを変えても記事は
  そのまま使えるし、記事を足してもデザインを触らずに済む
- **タグを3つに固定した** — 増やせるようにすると分類がぶれて一覧のフィルタが機能しなくなる
- **画像を自動変換にした** — 手作業だと必ず重い画像が混ざり、表示が遅くなる
- **`draft` を付けた** — 書きかけを公開せずに本番と同じ見た目で確認するため

設計の判断の経緯は `DESIGN_SPEC.md` と `PROGRESS.md` に残してある。
