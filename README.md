# ysd-ktn.github.io

Yoshida Kotone のポートフォリオサイト。[https://ysd-ktn.github.io](https://ysd-ktn.github.io)

## 使用技術

| 技術 | 名前 |
|:--|:--|
| フレームワーク | [Astro](https://astro.build/) v6 |
| 言語 | TypeScript |
| スタイリング | CSS (CSS Variables) |
| コンテンツ管理 | Astro Content Collections (JSON) |
| ホスティング | GitHub Pages |
| デプロイ | GitHub Actions |

## 構成

```
src/
  components/    # 各セクションのコンポーネント
    Header.astro
    Hero.astro
    About.astro
    Writing.astro
    Contact.astro
    Footer.astro
    Cursor.astro
  content/       # サイトコンテンツ (JSON)
    profile.json   # プロフィール情報
    timeline.json  # 経歴・イベント
    writing.json   # note 記事
    contact.json   # 連絡先
  content.config.ts  # Content Collections スキーマ定義
  layouts/
    Layout.astro   # HTML 共通レイアウト (meta / fonts)
  pages/
    index.astro    # トップページ (唯一のページ)
  styles/
    global.css     # グローバルスタイル・CSS Variables
public/
  images/writing/  # 記事サムネイル (WebP)
  favicon.*        # favicon 一式
  og-image.png     # OGP 画像
  robots.txt
.github/workflows/
  deploy.yml       # GitHub Actions デプロイ設定
```

## 開発

```sh
npm install
npm run dev      # 開発サーバー起動 (localhost:4321)
npm run build    # 静的ファイル生成 (dist/)
npm run preview  # ビルド結果のプレビュー
```

## コンテンツの更新

サイトの内容は `src/content/` 以下の JSON ファイルを編集する。スキーマは `src/content.config.ts` で定義されており、型に違反した場合はビルドエラーになる。

- **プロフィール**: `src/content/profile.json`
- **経歴・イベント**: `src/content/timeline.json`
- **note 記事**: `src/content/writing.json` (featured: true の記事が必ず 3件必要)
- **連絡先**: `src/content/contact.json`

## デプロイ

`master` ブランチへの push で GitHub Actions が自動的にビルド・デプロイする。手動操作は不要。
