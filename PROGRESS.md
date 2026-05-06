# PROGRESS.md

ysd-ktn.github.io ポートフォリオサイト リニューアル — 進捗管理

このファイルは「いま何ができてて、次に何をやるか」を Phase 単位で管理する。
詳細な仕様は `DESIGN_SPEC.md`、最終モックは `_mockups/option-1-cyber-v2-writing-grid.html` を参照。

最終更新: 2026.05.06 (Phase 7-1 (Header 以外のレスポンシブ対応) 完了 ✅ — 全コミット作成済、 まだ push してない。 残りは **Phase 7-2 (Header モバイル挙動の 3 案モック → 実装)** で、 これは新セッションで対応予定)

---

## ステータス凡例

- ✅ 完了
- ⏳ 着手中
- ⏸ 保留 (条件待ち)
- ⬜ 未着手

---

## Phase 1 — 設計確定 ✅

- 採用案 `_mockups/option-1-cyber-v2-writing-grid.html` を確定
- DESIGN_SPEC.md に仕様 (色・タイポ・IA・コンテンツモデル) を記述

## Phase 2 — Astro 雛形 + リポジトリ整理 ✅

- Astro プロジェクト初期化 (`package.json`, `astro.config.mjs`, `tsconfig.json`, `src/`)
- Flask 関連ファイル (`run.py`, `freeze.py`, `templates/`, 旧 `static/` 等) を `legacy` ブランチに退避
- master ブランチを Astro ベースに整理して GitHub に push

## Phase 3 — グローバル CSS + Header / Hero 分解 ✅

- `src/styles/global.css` に CSS変数・reset・scanline・vignette・main / section 共通枠を移植
- `src/layouts/Layout.astro` に Google Fonts (Barlow / JetBrains Mono / Noto Sans JP) 統合 + global.css import
- `src/components/Header.astro` 新設 (フローティングピル、hover で 720px 展開)
- `src/components/Hero.astro` 新設 (props で profile 情報を受ける設計)
- `src/pages/index.astro` を Header + Hero 構成に
- `npm run build` で動作確認 (静的生成OK)
- ヘッダーピル幅をモック値 180px → 200px に修正 (トグル + が切れる問題のフィックス)
- ヘッダーの `+` トグルから `margin-left` の制御を完全廃止。`margin: 0 → auto` は CSS transition が補間できずカクついていたため、nav と lang の max-width 展開によって toggle が自然に右へ押し出される flex 駆動の構造に変更
- ホバー時のピル幅を 720px → 650px に縮小 (実コンテンツ幅にフィット、toggle が右端ぴったりに収まるよう)
- 言語切替削除 (Phase 6 直後) に伴いホバー時ピル幅を 650px → 580px に再調整。lang 領域 132px を削った後、CONTACT が overflow:hidden で toggle に食われないよう ~20px の安全マージン込み
- ヘッダー toggle に `transform-origin: 11px center` を指定して回転中心を + 文字の中心にロック (padding 非対称により要素中心と文字中心がズレ、rotate 中に + が斜めに飛んで見える問題を修正)
- nav 最後の項目 (03 CONTACT) の padding-right を 14px → 22px に拡大 (隣の lang との dashed 区切り線が CONTACT 文字に近接していた問題を修正)

### Phase 3 の積み残し
- ✅ カスタムカーソル (`cursor: none` + `Cursor.astro`) → Phase 4-5 で実装済み
- ✅ Header の言語切替 (EN / JP) 表示削除 → 実コード上の削除は Phase 6 完了直後 (2026.05.05) に実施。それまでは本 PROGRESS.md だけ「完了」表記が先行し、実装は Phase 3 から残置されていた点に注意

---

## Phase 4 — 残りセクション分解 (UI 完成) ✅

**DoD**: モックアップ `_mockups/option-1-cyber-v2-writing-grid.html` と見た目同等になること。

実装するコンポーネント:

- ✅ **Phase 4-1** `src/components/About.astro` — PROFILE の dl/dt/dd spec シート (NAME / ROLE / LOCATION / FOCUS / TOOLS / WRITING / STATUS) + TIMELINE テーブル (EXHIBITION タグ + PRESENT のアクセント色)
  - props 駆動: `SpecEntry[]` と `TimelineEntry[]` を受ける設計
  - `TimelineEntry` は単発イベント (date + tag) と期間付き (from/to + role) の両方に対応
  - `to === 'PRESENT'` でアクセント色化、`accent: true` の SpecEntry は値の頭に ● を付与
  - サブ見出し (`[ 01.1 / PROFILE ]` 等) のスタイルは About 内に閉じる
  - index.astro に組み込み済み、ビルド検証済み
- ✅ **Phase 4-2** `src/components/Writing.astro` (3カードグリッド + MORE リンク)
  - meta 行 (`[ idx ]` / 日付 / ♡ likes) + 16:9 サムネ + タイトル + 概要 + NOTE → READ
  - 下部に `VIEW ALL ARTICLES · +N MORE → NOTE PROFILE` の CTA
  - props 駆動: `Article[]` + `totalCount` + `noteProfileUrl` を受ける設計
  - `idx` / `thumbLabel` 省略時は配列順から自動生成 (`01`, `02`, ... / `⊕ THUMB_NN.PNG · 16:9`)
  - `likes` が未指定の記事は ♡ 表示自体を非表示にできる
  - `+N MORE` の N は `totalCount - articles.length` から計算 (負値は 0 にクランプ)
  - hover 時に thumb の border / article-title / 背景色が accent に切り替わる挙動を移植
  - index.astro に組み込み済み、ビルド検証済み
- ✅ **Phase 4-3** `src/components/Contact.astro` (key/value リスト)
  - email (mailto + COPY) / X / Instagram / GitHub
  - props 駆動: `ContactEntry[]` を受ける設計 (label / value / href / action)
  - `action` 省略時は href が `mailto:` で始まれば 'copy'、それ以外は 'external' と自動判定
  - 'copy' は `<button class="ext copy-btn" data-cursor-chip="COPY">→ COPY</button>` で出力、 click で `navigator.clipboard.writeText(value)` → 1.5秒だけラベルを `→ COPIED ✓` + アクセント色に切替
  - 'external' は装飾のみの span、 リンク自体に `target="_blank" rel="noopener"`
  - button のデフォルト見た目は appearance/background/border/padding を 0 にして span.ext と完全一致させる
  - `data-cursor-chip="COPY"` で Cursor.astro 側に「ホバー時に COPY と出して欲しい」と markup レベルで宣言 (Phase 4-5 と整合)
  - index.astro に組み込み済み、ビルド検証済み (script タグも `<script type="module">` でバンドルされる)
- ✅ **Phase 4-4** `src/components/Footer.astro` (画面下固定ステータスバー)
  - ● ONLINE · BUILD / CURSOR · X,Y / UTC HH:MM:SS / VIEWPORT · W×H
  - ● は 1.4秒間隔で点滅 (アクセント色) — `@keyframes blink` で `steps(2)` infinite
  - BUILD 日付はビルド時の `new Date()` から MMDD.YYYY 形式で自動生成、 props で上書き可
  - CURSOR は document の mousemove で 4桁ゼロ埋め座標に更新 (passive listener)
  - UTC + viewport は 1秒 tick で更新、 viewport は resize でも即時反映
  - 各 id は `status-` prefix を付けて Phase 4-5 (Cursor.astro) との衝突を回避
  - 装飾的な HUD なので `aria-hidden="true"` でスクリーンリーダから除外
  - index.astro では `<main>` の外、 Layout 直下に配置 (position: fixed なので構造上はどこでも可だが意味的に外側)
  - ビルド検証済み
- ✅ **Phase 4-5** `src/components/Cursor.astro` (Figma 風 SVG カーソル + チップ + 座標)
  - global.css に `@media (hover: hover) { html, body { cursor: none } }` を追加。 タッチデバイスでは OS カーソルをそのまま使う
  - SVG 矢印 (▲ Figma 選択ツール風) + チップ + 座標 を `position: fixed; z-index: 200` で配置 (scanline / status-bar より前)
  - mousemove (`passive: true`) で `transform: translate()` 更新、 transition 0.05s linear で軽く追従感を出す
  - チップは初期 `opacity: 0`、 hover 中の対象を検出したら `.show` クラスで `opacity: 1`
  - hover 判定は document-level の event delegation (`mouseover` / `mouseout` + `closest('a, button, .work')`) に変更 — モックの per-element listener と違って、 子要素経由の mouseout を `relatedTarget.closest()` で吸収できるので、 リンク内の子要素 (.thumb 等) を経由してもチップがチラつかない
  - チップ文言の解決: **`data-cursor-chip` 属性 (markup 側の自己宣言) > `.work` → OPEN PROJECT > `<a>` → OPEN LINK > `<button>` (chip 未宣言) → 非表示** の優先順位
    - `<button>` の一律 FILTER デフォルトは廃止 (Contact の COPY ボタンに FILTER と出てしまう問題があったため)。 各ボタンが `data-cursor-chip="..."` で自己宣言する設計に変更
    - Contact.astro の COPY ボタンには `data-cursor-chip="COPY"` を付与済み
    - 将来 WORKS が復活してフィルタボタンを置く時は `data-cursor-chip="FILTER"` で対応
  - 座標は `XXXX, YYYY` (4桁ゼロ埋め)、 アクセント色 mono 9px。 Footer の `CURSOR · XXXX,XXXX` とは別レイヤで、 マウス追従するのはこちら、 画面下固定なのは Footer
  - id は `cursor` / `cursor-meta` / `cursor-chip` / `cursor-coord` (Footer は `status-` prefix なので衝突なし)
  - `@media (hover: none)` で `.cursor` / `.cur-meta` を `display: none` にしてタッチデバイスでは描画ごとスキップ
  - `aria-hidden="true"` でスクリーンリーダから除外
  - index.astro に `<Cursor />` を `<Layout>` 直下 (Header の前) で組み込み済
  - サンドボックス側でビルド検証済 (Cowork のマウントが unlink を許さないため `astro.config.sandbox.mjs` という残骸ファイルが project root に残っている — 手元で `rm` して可)

各サブフェーズ完了ごとに dev で確認 → コミット → push を推奨。

---

## Phase 5 — Content Collections でデータ外出し + 実テキスト投入 ✅

**DoD**:
1. コンポーネント側のハードコード値を JSON に分離し、型安全に読み込めること。
2. JSON に入る **テキスト・数字・リンク類は本物のデータ**になっていること (placeholder の二度手間を避ける)。
3. 画像アセット (記事サムネ等) は Phase 8 まで placeholder のままで OK。 JSON にはパスだけ仮置き。

### Phase 5-1 構造化 ✅ (2026.05.02 完了)

- ✅ `src/content.config.ts` で Astro Content Collections + Zod スキーマ定義 (`profile` / `writing` シングルトン、 `timeline` / `contact` 配列)
  - `profile` / `writing` は `{ "main": {...} }` 形式の JSON、 コンポーネント側は `getEntry('profile', 'main')` で取得
  - `timeline` / `contact` は配列 JSON、 各要素に `id` 必須 (Astro file() loader の規約)、 `getCollection()` で取得後 `order` でソート
  - `writing` は `articles[].featured: true` がちょうど 3件 になるように `.refine()` で制約 (3カラムグリッド固定のため)
  - `timeline` は単発エントリ (`date`) と期間エントリ (`from + to`) のどちらか一方を許容する `.refine()` 制約付き
  - 全 schema を `.strict()` で塞いで typo を build エラー化
- ✅ `src/content/profile.json` `timeline.json` `writing.json` `contact.json` を新規作成 (中身は **現コンポーネント内 dummy 値の転記** — 本物データ投入は Phase 5-2 で)
  - Instagram は最初から含めない方針 (要件として恒久的に削除)
  - X ハンドル `@ysd_ktn` は実在
- ✅ `Hero.astro` / `About.astro` / `Writing.astro` / `Contact.astro` を JSON 駆動に切替
  - frontmatter で `getEntry` / `getCollection` で読込、 props default に流す
  - props インターフェース自体は override 用に残してある (将来プレビューページ用に温存)
  - About は `profile` から spec シート 7行を組み立てるロジックをコンポーネント側に持たせた (NAME / ROLE / ... / STATUS の並びは固定)
  - Writing は `articles.filter(featured).slice(0, 3)` でトップ表示3件を選別
- ✅ ビルド検証
  - サンドボックスは mount が unlink を許さないので `astro.config.sandbox.mjs` で cacheDir / outDir / vite.cacheDir を `/tmp` に逃がし、 さらに project ごと `/tmp` に tar 展開 + `npm install` してビルド
  - 生成 HTML が Phase 4 終了時と完全一致することを確認 (Hero / About PROFILE 7行 + TIMELINE 5行 / Writing 3カード `+12 MORE` / Contact 3行 — Instagram なし / Footer ステータスバー)
  - Zod 制約が効くことも確認 (timeline の `id` を欠落させたら build エラー)

### Phase 5-2 本物データ投入 ✅ (2026.05.02 完了)

- ✅ `src/content/profile.json` の本物データ化
  - 拠点を神奈川 (川崎駅 35.5308°N · 139.6968°E) に
  - 役職を `UI/UX Designer` に統一
  - `roleHero` を `UI/UX DESIGNER · BASED IN KANAGAWA, JP · CLOSED FOR NEW PROJECTS` に (Hero 用1行)
  - `focus` (UI/UX設計 · デザインシステム構築 · ビジュアル/インフォグラフィックデザイン) と `tools` (Figma · HTML/CSS · Git · Adobe Illustrator · Adobe Photoshop · Unity) を実領域・実ツールに
  - `writingIntro` を本物の自己紹介文に
  - `statusOpen: false` + `statusLabel: "CLOSED FOR NEW PROJECTS · INQUIRIES OPEN"` (現在新規案件停止中、 連絡窓口は開放)
  - `lastUpdate: "2026.05.06"`
- ✅ `src/content/timeline.json` の本物データ化
  - 5行に整理: 株式会社セガ (現職) → 株式会社マネーフォワード → ClusterGAMEJAM 2022 (EVENT) → 京都市芸バーチャル展 (EXHIBITION) → 京都市立芸術大学
  - `order` 降順で並びを管理 (50, 40, 30, 20, 10)
- ✅ `src/content/writing.json` の本物データ化
  - 実 note 3記事に差し替え (`featured: true`)、 `totalCount: 4` (うち1件は非掲載)
  - excerpt は note 本文冒頭をそのまま流し込み (CSS line-clamp で2行 + … に自動切り詰め)
  - `noteProfileUrl: https://note.com/ysd_ktn`
- ✅ `src/content/contact.json` の本物データ化
  - email を `yosida.kotone@gmail.com` (mailto: 付き)
  - X は `@ysd_ktn` (元から本物)

### Phase 5 関連の追加改善 (構造化中・データ投入中に発生したもの)

- ✅ STATUS dot を `● open / ○ closed` の二状態に拡張
  - SpecEntry の `accent: boolean` を `dot: 'open' | 'closed' | undefined` に置換
  - OPEN は ● in accent (#00f0bd)、 CLOSED は ○ in dim (#6b6b6b)
  - 「募集停止だが連絡は受け付ける」状態を視覚的に表現できる構造に
- ✅ Writing excerpt の自動切り詰め (CSS line-clamp + 自動 …)
  - JSON には note 本文冒頭をそのままコピペで OK、 文字数気にしなくて良い
  - 当初 `flex: 1` と `-webkit-line-clamp` の競合で短いタイトルのカードのみ3行表示されるバグあり → ラッパー要素で責任分離して修正
- ✅ PROFILE の ROLE 行を削除 (情報整理)
  - `roleShort` を schema / JSON / About.astro spec 構築から完全削除
  - PROFILE は NAME / LOCATION / FOCUS / TOOLS / WRITING / STATUS の 6行構成に
- ✅ Writing の likes 表示を削除
  - note は公式 API がなく自動取得が困難、 手動更新だと整合性崩れるため
  - schema の `likes` フィールド自体は optional のまま温存 (将来復活したくなれば JSON に書き戻すだけ)
  - 各カードの meta-row は `[ idx ] 日付` のみのシンプル構成に

### サンドボックスビルド用の残骸

- `astro.config.sandbox.mjs` がプロジェクト root に残っている。ローカル開発では不要なファイル (Cowork サンドボックスでビルド検証するためだけのもの)。手元で `rm astro.config.sandbox.mjs` してよい。push する前に削除推奨。

---

## Phase 6 — GitHub Actions デプロイ ✅ (2026.05.05 完了)

**DoD**: master push で自動的にデプロイされ、`https://ysd-ktn.github.io` で公開されること。

### 最終構成

- ✅ `.github/workflows/deploy.yml`
  - Build ジョブ: `actions/checkout@v5` → `actions/setup-node@v5` (Node 22) → `npm install` → `astro build` → `actions/upload-pages-artifact@v3`
  - Deploy ジョブ: `actions/deploy-pages@v4` (公式 OIDC アクション)
  - `environment: { name: github-pages, url: ... }` 設定 (deploy-pages@v4 必須)
  - `permissions: { contents: read, pages: write, id-token: write }`
  - `concurrency: { group: pages, cancel-in-progress: false }`

- ✅ GitHub Pages Settings → **Source: GitHub Actions**
  - 旧 `gh-pages branch` 設定では deploy-pages@v4 が機能しないため切替必須
  - gh-pages ブランチ自体は不要 (公式アクションは Actions artifact から直接デプロイ)

### Phase 6 中に踏んだ罠 (将来の参考)

- 一時的に「environment 保護ルールが邪魔」と判断して environment ブロックごと削除したが、deploy-pages@v4 は environment 必須で `HttpError: Missing environment` で deploy が 400 失敗する。**environment は消してはいけない**。保護ルールが邪魔なら GitHub Settings → Environments → github-pages の Deployment branches を「All branches」に緩めるのが正解
- peaceiris/actions-gh-pages は新しい OIDC ベースのデプロイ非対応。`actions/deploy-pages@v4` に置き換え済
- `actions/upload-pages-artifact@v3` と `actions/deploy-pages@v4` はバージョン整合性が重要 (セットで使う)

---

## Phase 6.5 — Writing 記事サムネ実画像差し替え ✅ (2026.05.05 完了)

**経緯**: Phase 7 (レスポンシブ) に入る前にサムネのサイズ感を確定させとくと、レスポンシブ確認時に「placeholder 用に組んだ枠が実画像で破綻」する手戻りを避けられる。Phase 8 の画像差し替えタスクのうち、 Writing 分のみを前倒しで処理。

### 実施内容

- ✅ `public/images/writing/` 新設、 3枚の note OGP 画像を WebP で配置
  - `01-game-industry.webp` (2025.04 ゲーム業界に転職しました, 100KB)
  - `02-sakuji.webp` (2025.01 1日1作字, 116KB)
  - `03-mf-shinsotsu.webp` (2024.06 MF 24新卒研修, 68KB)
  - 解像度: 2560×1340 (note OGP 標準 1280×670 の 2x、 Retina 対応)
  - 全 200KB 以下で Lighthouse Performance に優しい
- ✅ `content.config.ts`: `writingArticleSchema` に `thumbnail: z.string()` 必須フィールド追加。 漏れたら build エラー化することを実機検証済み (`articles.0.thumbnail: Required` で停止)
- ✅ `writing.json`: 3記事に `thumbnail` パス書き込み
- ✅ `Writing.astro` 改修
  - dashed プレースホルダ枠 + `⊕ THUMB_NN.PNG · 16:9` テキスト → `<img>` に置換
  - 16:9 枠は維持 (`aspect-ratio: 16/9` + `overflow: hidden`)、 画像は `object-fit: cover` で詰める
  - 画像ソースが 2560×1340 (≈1.91:1) で 16:9 (1.78:1) より少し横長 → 左右が極わずかにトリミングされるが note OGP は中央配置が多いので実害なし
  - Border は dashed → solid に (実画像が入るので破線枠は不適切)
  - `loading="lazy"` + `decoding="async"` で performance 最適化
  - `alt=""` で screen reader からは装飾扱い (記事タイトルが直下にあるため重複読み上げを防ぐ)
  - hover 演出: 枠を accent 色 (#00f0bd) + 画像の `grayscale(0.85)` を解除して「光が点く」演出 (transition 0.2s)
  - `Article` interface (props 型) にも `thumbnail: string` 必須を追加
- ✅ サンドボックスで `astro build` 検証済 — 3枚の `<img>` が dist/index.html に正しく組み込まれ、 dist/images/writing/ にも asset がコピーされていることを確認
  - サンドボックス手順: `/tmp/ysd-build/` に rsync → node_modules を cp → `./node_modules/.bin/astro build`
  - `astro.config.sandbox.mjs` は不要だった (build cache の unlink 問題は今回出ず)
- ✅ コミット & push 済 → GitHub Actions による自動デプロイ完了、 本番 (`https://ysd-ktn.github.io`) に反映済

### 残った画像系タスク (Phase 8 で対応)

- favicon (`favicon.ico` / `apple-touch-icon.png`) — 素材未制作
- OG image (1200×630) — 素材未制作
- `Layout.astro` の meta タグ整備 (title / description / og:* / twitter:*) — 上記 2 と同時にやる

---

## Phase 7 — レスポンシブ対応 ⏳ (Header モバイル挙動のみ残)

**DoD**: モバイル (~390px) でレイアウト破綻しないこと。

ブレイクポイントは 720px 一本で統一 (mobile-first ではなく desktop ベースに `@media (max-width: 720px)` で書く方針)。

### Phase 7-1 Header 以外のレスポンシブ対応 ✅ (2026.05.06 完了)

- ✅ **Hero フォントサイズ + role 行**
  - h1 を `clamp(48px, 14vw, 88px)` に詰めて 390px でも YOSHIDA / KOTONE が収まるように
  - `.role` (`> UI/UX DESIGNER · BASED IN KANAGAWA, JP · CLOSED FOR NEW PROJECTS`) は mobile で `display: block` に切替。 元の flex だと `>` ::before とテキスト span が wrap で別行に分離してたが、 block にすることで inline 文字列として 1 つの塊で流れて、 末尾で word-wrap による自然な折り返しに。 `>` の右に `margin-right: 8px` で本文との間隔
- ✅ **Writing 3カラムグリッド → 1カラム**
  - `@media (max-width: 720px) { .writing-grid { grid-template-columns: 1fr } }`
  - `gap: 1px` の grid 区切り線はそのまま動く (1fr でも水平区切りになる)
- ✅ **Writing サムネの grayscale をタッチデバイスで解除**
  - `@media (hover: none) { .writing-card .thumb img { filter: none } }`
  - hover でカラーに点灯する演出は touch では発動しないので、 最初からフルカラーで note OGP 画像が見える状態に (Phase 6.5 の宿題)
- ✅ **Cursor デスクトップ限定**
  - 既存の `@media (hover: none) { .cursor, .cur-meta { display: none } }` で対応済 (Phase 4-5 から)、 Phase 7 では確認のみ
- ✅ **Footer ステータスバー情報量削減**
  - `CURSOR · X,Y` (touch にマウスなし) と `VIEWPORT · W×H` を 720px 以下で `display: none`、 padding を `8px 16px` に詰める。 残るは `● ONLINE · BUILD MMDD.YYYY` と `UTC HH:MM:SS` のみ
  - 各 span にクラス (`status-cursor` / `status-viewport`) を追加して mobile media query から指定

#### Phase 7-1 で発生した追加対応 (PROGRESS のリスト外だが DoD 達成に必須だった)

- ✅ **About / Contact のモバイル縦積み**
  - `.spec` (`grid-template-columns: 200px 1fr`) や `.timeline .date` (`width: 240px` 固定)、 `.contact-row` (`90px 1fr 1fr`) は 390px 画面だと長文 token (例: メールアドレス) が grid item の `min-width: auto` 仕様で min-content として親 grid を突き破って、 横スクロール余白を生んでた
  - `@media (max-width: 720px)` で `.spec` を 1 カラム縦積み (dt 上 / dd 下)、 timeline は table を捨てて `display: block` で各 td を縦積み、 Contact は 1 カラムに + `→ COPY` / `→ EXTERNAL` の `.ext` を `display: none` に
  - `overflow-wrap: anywhere` を spec dd / contact a / timeline title に追加して、 長いトークンも安全に折り返すように
- ✅ **水平スクロールロック (保険)**
  - `html, body { overflow-x: hidden }` を global.css に追加。 上の縦積み対応で根本原因は潰したが、 将来また grid item の min-content overflow が起きても画面右の余白が生まれない保険
- ✅ **sec-head の余白調整 (デフォルト + mobile)**
  - デフォルト `grid-template-columns: 80px 1fr` を `auto 1fr` に変更 — `[ 0N ]` の実幅 (≈ 36px) ぴったりに num 列が詰まり、 50px の無駄余白が消えた
  - デフォルト 親 grid `gap: 32px → 16px`、 meta `gap: 24px → 16px` に詰めて、 中間 viewport (720-900px 程度) で SELECTED が右端ベタ寄りにならないように
  - mobile では更に gap 10/6、 font-size 11→9px、 letter-spacing 0.1→0.04em で物理幅縮小
  - mobile では `.sec-head .meta span` に `white-space: nowrap` を当てて `FEATURED · 3 OF 4` の中央スペースで折れる問題を解消
  - `.sec-head .num` に `white-space: nowrap` を追加 — 1fr 列の meta が長い (Writing) と auto 列が文字単位の min-content (1 文字幅) まで圧迫されて `[ 02 ]` が `[ 02` / `]` に分離する現象を解消
- ✅ **Writing CTA 矢印の維持 (mobile)**
  - `.writing-more` の `→ NOTE PROFILE ↗` 部分について、 当初は `.cta` 全体を mobile で `display: none` にしたが、 アクセント色の `↗` 矢印は残したいとの要望で構造変更
  - `.cta` を `<span class="cta-text">→ NOTE PROFILE </span><span class="arrow">↗</span>` の 2 段構造に分けて、 mobile では `.cta-text` だけ非表示、 `.arrow` は右端に残す形に
- ✅ **main / section の mobile padding 詰め**
  - `padding: 80px 32px 80px` → `padding: 64px 16px 80px` に (左右 32 → 16)
  - section は `padding: 56px 0` → `40px 0` に

#### Phase 7-1 のコミット履歴 (15 コミット、 まだ origin/master に push してない)

```
Add mobile padding and lock horizontal overflow
Make Hero responsive on narrow viewports
Stack Writing grid to single column on mobile
Trim Footer status bar on mobile
Stack About profile and timeline on mobile
Stack Contact rows on mobile
Tighten sec-head spacing on mobile
Hide writing-more CTA on mobile
Force sec-head nowrap on mobile
Restore writing-more arrow on mobile
Cut main padding and shrink sec-head further on mobile
Tighten sec-head num column to content width
Prevent sec-head num from wrapping when meta is long
Tighten sec-head default gaps
Inline Hero `>` prefix with role text on mobile
```

#### Phase 7-1 で踏んだ罠 (将来の参考)

- **Cowork サンドボックスの mount は unlink を許さない** — `rm` / `git commit` (内部で index.lock / HEAD.lock を作って消す) が permission denied で失敗。 そのため commit 作業はユーザー手元で実行、 マロは `/tmp` 側で commit chain を作って `git format-patch` で patch 化 → mount 経由でユーザー手元で `git am` が現実的なフロー (Phase 6.5 では発生しなかった、 連続 commit すると詰まる)
- **CSS Grid auto 列の落とし穴** — `grid-template-columns: auto 1fr` の auto 列は通常は内容の min-content で確保されるが、 1fr 列の中身が `white-space: nowrap` だと auto 列が文字単位 min-content (1 文字幅) まで圧迫される。 auto 列側にも `white-space: nowrap` を当てて min-content を全文字幅で固定する必要あり

### Phase 7-2 Header モバイル挙動 ⬜ (新セッションで対応予定)

**DoD**: タッチデバイスで Header のナビが操作できる状態。

- 現状: `Header.astro` は 200px 固定の floating pill、 hover で 580px 展開 (cubic-bezier transition)。 hover が無いタッチデバイスではナビが展開できないため未操作
- 検討する 3 案 (新セッションで `_mockups/` に比較モックを作る):
  - **案 A** ハンバーガー: トグル `+` ボタン (既存) を tap でドロップダウン or フルスクリーンメニューを開閉
  - **案 B** ボトムドロワー: 画面下から ナビパネルを slide up
  - **案 C** 縦並び: ピル展開を諦めて、 mobile では Header 全体を縦並びの別レイアウトに
- 新セッションでの作業フロー:
  1. `_mockups/` に 3 つのモック HTML を新規作成 (`option-mobile-header-A-hamburger.html` 等)
  2. ユーザーに見せて 1 案決定
  3. `Header.astro` に実装、 既存の hover pill 挙動はデスクトップ (`@media (hover: hover) and (min-width: 721px)` 等) に閉じる
  4. PROGRESS.md 更新 → コミット → push (Phase 7-1 のコミット 15 個 + Phase 7-2 のコミットをまとめて push)

---

## Phase 8 — SEO・最適化・画像アセット・本番化 ⬜

**DoD**: Lighthouse 95+、検索エンジンに正しくインデックスされ、画像アセットが本物に差し替わってる状態。

- meta tags 整備 (title / description / og:* / twitter:*)
- OG image (1200×630)
- sitemap.xml + robots.txt
- favicon (アイコンセット)
- Lighthouse スコア確認 (Performance / Accessibility / Best Practices / SEO)
- アクセシビリティ確認 (キーボード操作、スクリーンリーダ、コントラスト)
- 画像アセットの本物差し替え
  - Writing 記事サムネは Phase 6.5 で前倒し済 ✅
  - 残: favicon / apple-touch-icon / OG image (1200×630)
  - テキスト・リンク・数字は Phase 5 で既に実データ化済み

---

## 保留中の判断 (Pause Items)

- **WORKS セクション復活** ⏸
  - 復活条件: 個人開発が3件以上 or 公開可能な大型プロジェクト (展覧会等) が2件以上溜まったタイミング
  - 詳細ページのテンプレート: `_mockups/_archive/option-1-cyber-v2-detail.html`
  - グリッドのテンプレート: 現行 WRITING グリッド構造を流用
  - 復活時のセクション番号: `02 WORKS / 03 WRITING / 04 CONTACT` に組み替え
- **ライトモード** ⏸ — 現状ダーク固定
- **問い合わせフォーム vs mailto** ⏸ — 現状 mailto。フォーム化するなら Formspree 等別途検討

---

## 新セッションへの引き継ぎ手順

新しいセッションで作業を再開する時は以下の順で:

1. このファイル `PROGRESS.md` を最初に読む
2. 現在のフェーズ (✅ / ⏳ / ⬜) と直前の作業内容を把握
3. 詳細な仕様判断は `DESIGN_SPEC.md` を参照
4. レイアウトの最終形は `_mockups/option-1-cyber-v2-writing-grid.html` を参照
5. 進めるフェーズの DoD に向けて作業
6. 完了したらこのファイルを更新 (該当 Phase のステータス、最終更新日)
