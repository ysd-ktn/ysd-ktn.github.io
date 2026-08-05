# ysd-ktn.github.io

Yoshida Kotone のポートフォリオサイト。[https://ysd-ktn.github.io](https://ysd-ktn.github.io)

> **記事を書く・サイトの文言を変える人は [UPDATING.md](./UPDATING.md) を読むこと。**
> このファイルは技術構成のメモ。日々の更新手順はあちらにまとめてある。

## 使用技術

| 技術 | 名前 |
|:--|:--|
| フレームワーク | [Astro](https://astro.build/) v6 |
| 言語 | TypeScript |
| スタイリング | CSS (CSS Variables) |
| コンテンツ管理 | Astro Content Collections (Markdown + JSON) |
| ホスティング | GitHub Pages |
| デプロイ | GitHub Actions |

## 構成

```
src/
  components/          # 各セクションのコンポーネント
    Header.astro         # フローティングピル / モバイルナビ
    Hero.astro
    About.astro
    Writing.astro        # トップの LATEST セクション (最新3件)
    ArticleCard.astro    # 記事カード (一覧と LATEST で共用)
    Contact.astro
    Footer.astro         # 画面下固定ステータスバー
    Cursor.astro         # カスタムカーソル
  content/             # サイトコンテンツ
    writing/*.md         # 記事本文 (外部記事は本文なし + externalUrl)
    profile.json         # プロフィール情報
    timeline.json        # 経歴・イベント
    contact.json         # 連絡先
  content.config.ts    # Content Collections スキーマ定義 (Zod)
  layouts/
    Layout.astro         # HTML 共通レイアウト (meta / OGP / fonts)
  lib/
    writing.ts             # 記事の並び替え・目次抽出などの共通処理
    content.ts             # コンテンツ取得ヘルパー (欠落時に原因を出す)
    remark-line-breaks.ts  # 単独の改行をそのまま改行にする
    rehype-clean-heading-ids.ts  # 絵文字入り見出しの id を整える
    rehype-link-cards.ts   # 単独リンクをカード表示にする
  pages/
    index.astro          # トップページ
    writing/
      index.astro        # 記事一覧
      [slug].astro       # 記事個別ページ
  styles/
    global.css           # グローバルスタイル・CSS Variables
public/
  images/writing/<slug>/  # 記事画像 (thumb.webp / img*.webp / ogp.jpg / _raw)
  favicon.*
  og-image.png            # サイト共通の OGP 画像
  robots.txt
script/
  new-article.py       # 記事の雛形作成 / 画像最適化
  notion-to-md.py      # Notion エクスポート → 記事 Markdown
.github/workflows/
  deploy.yml           # GitHub Actions デプロイ設定
_mockups/              # デザイン検討用の静的モック (本番には含まれない)
```

## 開発

```sh
npm install
npm run dev      # 開発サーバー起動 (localhost:4321)
npm run build    # 静的ファイル生成 (dist/)
npm run preview  # ビルド結果のプレビュー
```

## コンテンツの更新

手順は **[UPDATING.md](./UPDATING.md)** を参照。以下は概要のみ。

サイトの内容は `src/content/` 以下を編集する。スキーマは `src/content.config.ts` に
Zod で定義してあり、型に違反した場合はビルドエラーになる。

- **記事**: `src/content/writing/*.md` (frontmatter + 本文)
- **プロフィール / 各所の文言**: `src/content/profile.json`
- **経歴・イベント**: `src/content/timeline.json`
- **連絡先**: `src/content/contact.json`

記事コレクションは移行済み記事と note のみの記事を1つで扱う。
`externalUrl` を持つエントリは個別ページを生成せず、一覧のカードから直接 note へ飛ぶ。

### 補助スクリプト

```sh
python3 script/new-article.py <slug>            # 記事の雛形を作る
python3 script/new-article.py <slug> --images   # _raw/ の画像を WebP + OGP JPG に変換
python3 script/notion-to-md.py <zip> <slug>     # Notion エクスポートを記事に変換
```

## デプロイ

`master` ブランチへの push で GitHub Actions が自動的にビルド・デプロイする。手動操作は不要。

## 旧サイト (Flask 版)

リニューアル前のサイトは `legacy` ブランチに丸ごと残してある (GitHub にも push 済み)。
master 側からは削除済み。見たくなったら以下のいずれかで。

```sh
# 別フォルダに取り出して見る (今のフォルダは影響を受けない)
git worktree add ../old-site legacy
open ../old-site/index.html          # 静的出力なのでブラウザで直接開ける
git worktree remove ../old-site      # 見終わったら削除

# 1ファイルだけ中身を見る
git show legacy:index.html
```

ブラウザで見るだけなら GitHub 上の
[legacy ブランチ](https://github.com/ysd-ktn/ysd-ktn.github.io/tree/legacy) を開くのが早い。
