# PROGRESS.md

ysd-ktn.github.io ポートフォリオサイト リニューアル — 進捗管理

このファイルは「いま何ができてて、次に何をやるか」を Phase 単位で管理する。
詳細な仕様は `DESIGN_SPEC.md`、最終モックは `_mockups/option-1-cyber-v2-writing-grid.html` を参照。

最終更新: 2026.05.05 (Phase 5 完了 ✅ — 構造化 + 本物データ投入の両方が完了。Phase 6 着手中 ⏳ — ワークフロー YAML 作成完了。GitHub リポジトリ Pages 設定待ち)

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
- ヘッダー toggle に `transform-origin: 11px center` を指定して回転中心を + 文字の中心にロック (padding 非対称により要素中心と文字中心がズレ、rotate 中に + が斜めに飛んで見える問題を修正)
- nav 最後の項目 (03 CONTACT) の padding-right を 14px → 22px に拡大 (隣の lang との dashed 区切り線が CONTACT 文字に近接していた問題を修正)

### Phase 3 の積み残し
- ✅ カスタムカーソル (`cursor: none` + `Cursor.astro`) → Phase 4-5 で実装済み
- ✅ Header の言語切替 (EN / JP) 表示削除 → Phase 6 準備中で削除完了

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

## Phase 6 — GitHub Actions デプロイ ⏳

**DoD**: master push で自動的に gh-pages にデプロイされ、`https://ysd-ktn.github.io` で公開されること。

- ✅ `.github/workflows/deploy.yml` を作成 (Node LTS 20, peaceiris/actions-gh-pages v3)
  - Build ジョブ: チェックアウト → Node 20 セットアップ → `npm install` → `astro build` → dist アップロード
  - Deploy ジョブ: dist ダウンロード → peaceiris で gh-pages ブランチに自動 push
  - CNAME レコード設定（ysd-ktn.github.io）
- ⏳ GitHub リポジトリ設定 (手動。ユー作業)
  - Settings → Pages → Source を `gh-pages branch` に変更
  - (Enforce HTTPS はデフォルト有効にしておく)
- ⬜ 初回デプロイで実機確認
  - master へ git push → GitHub Actions 自動実行 → 3-5分待機 → `https://ysd-ktn.github.io` でビルド結果確認

---

## Phase 7 — レスポンシブ対応 ⬜

**DoD**: モバイル (~390px) でレイアウト破綻しないこと。

- Hero フォントサイズ (clamp で吸収済み、要確認)
- Header ピルのモバイル挙動 — 展開 720px は不可なので、ハンバーガー or ボトムドロワー or 縦並びに切替
- Writing 3カラムグリッド → 1カラム
- Cursor は `@media (hover: hover)` でデスクトップ限定に
- Footer ステータスバーの情報量を削る (CURSOR 表示は不要 etc)

---

## Phase 8 — SEO・最適化・画像アセット・本番化 ⬜

**DoD**: Lighthouse 95+、検索エンジンに正しくインデックスされ、画像アセットが本物に差し替わってる状態。

- meta tags 整備 (title / description / og:* / twitter:*)
- OG image (1200×630)
- sitemap.xml + robots.txt
- favicon (アイコンセット)
- Lighthouse スコア確認 (Performance / Accessibility / Best Practices / SEO)
- アクセシビリティ確認 (キーボード操作、スクリーンリーダ、コントラスト)
- 画像アセットの本物差し替え (Phase 5 で placeholder のままにしておいた記事サムネ等)
  - テキスト・リンク・数字は Phase 5 で既に実データ化済みなので、 ここでは画像のみが対象

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
