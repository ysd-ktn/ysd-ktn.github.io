# PROGRESS.md

ysd-ktn.github.io ポートフォリオサイト リニューアル — 進捗管理

このファイルは「いま何ができてて、次に何をやるか」を Phase 単位で管理する。
詳細な仕様は `DESIGN_SPEC.md`、最終モックは `_mockups/option-1-cyber-v2-writing-grid.html` を参照。

最終更新: 2026.05.01 (Phase 4 進行中、About.astro 完了)

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
- カスタムカーソル (`cursor: none` + `Cursor.astro`) は Phase 4 へ持ち越し
- Header の言語切替 (EN / JP) は表示のみ、機能未実装

---

## Phase 4 — 残りセクション分解 (UI 完成) ⏳

**DoD**: モックアップ `_mockups/option-1-cyber-v2-writing-grid.html` と見た目同等になること。

実装するコンポーネント:

- ✅ **Phase 4-1** `src/components/About.astro` — PROFILE の dl/dt/dd spec シート (NAME / ROLE / LOCATION / FOCUS / TOOLS / WRITING / STATUS) + TIMELINE テーブル (EXHIBITION タグ + PRESENT のアクセント色)
  - props 駆動: `SpecEntry[]` と `TimelineEntry[]` を受ける設計
  - `TimelineEntry` は単発イベント (date + tag) と期間付き (from/to + role) の両方に対応
  - `to === 'PRESENT'` でアクセント色化、`accent: true` の SpecEntry は値の頭に ● を付与
  - サブ見出し (`[ 01.1 / PROFILE ]` 等) のスタイルは About 内に閉じる
  - index.astro に組み込み済み、ビルド検証済み
- ⬜ **Phase 4-2** `src/components/Writing.astro` (3カードグリッド + MORE リンク)
  - meta 行 (`[ idx ]` / 日付 / ♡ likes) + 16:9 サムネ + タイトル + 概要 + NOTE → READ
  - 下部に `VIEW ALL ARTICLES · +N MORE → NOTE PROFILE` の CTA
- ⬜ **Phase 4-3** `src/components/Contact.astro` (key/value リスト)
  - email (mailto + COPY) / X / Instagram / GitHub
- ⬜ **Phase 4-4** `src/components/Footer.astro` (画面下固定ステータスバー)
  - ● ONLINE · BUILD / CURSOR · X,Y / UTC HH:MM:SS / VIEWPORT · W×H
  - ● は 1.4秒間隔で点滅 (アクセント色)
  - UTC 時刻と viewport は client-side JS で更新
- ⬜ **Phase 4-5** `src/components/Cursor.astro` (Figma 風 SVG カーソル + チップ + 座標)
  - body に `cursor: none` を追加 (global.css の方を更新)
  - mousemove で SVG 矢印 + アノテーションチップ + 座標を追従
  - hover 対象に応じてチップ文言を変える (`OPEN LINK` / `FILTER` / `OPEN PROJECT`)
  - `@media (hover: hover)` でタッチデバイス無効化

各サブフェーズ完了ごとに dev で確認 → コミット → push を推奨。

---

## Phase 5 — Content Collections でデータ外出し ⬜

**DoD**: コンポーネント側のハードコード値を JSON に分離し、型安全に読み込めること。

- `src/content/profile.json` (Hero / About PROFILE 用)
- `src/content/timeline.json` (About TIMELINE 用、雇用 + 展覧会)
- `src/content/writing.json` (note 記事リスト + totalCount)
- `src/content.config.ts` で Content Collections + Zod スキーマ定義
- 各コンポーネントを props 駆動 → JSON 駆動に切り替え

JSON のスキーマは DESIGN_SPEC.md の「コンテンツモデル」セクションを参照。

---

## Phase 6 — GitHub Actions デプロイ ⬜

**DoD**: master push で自動的に gh-pages にデプロイされ、`https://ysd-ktn.github.io` で公開されること。

- `.github/workflows/deploy.yml` を作成
- Astro build → dist → gh-pages ブランチに出力 (peaceiris/actions-gh-pages 等)
- GitHub リポジトリ設定で Pages の Source を gh-pages branch に変更
- 初回デプロイで実機確認

---

## Phase 7 — レスポンシブ対応 ⬜

**DoD**: モバイル (~390px) でレイアウト破綻しないこと。

- Hero フォントサイズ (clamp で吸収済み、要確認)
- Header ピルのモバイル挙動 — 展開 720px は不可なので、ハンバーガー or ボトムドロワー or 縦並びに切替
- Writing 3カラムグリッド → 1カラム
- Cursor は `@media (hover: hover)` でデスクトップ限定に
- Footer ステータスバーの情報量を削る (CURSOR 表示は不要 etc)

---

## Phase 8 — SEO・最適化・本番化 ⬜

**DoD**: Lighthouse 95+、検索エンジンに正しくインデックスされる状態。

- meta tags 整備 (title / description / og:* / twitter:*)
- OG image (1200×630)
- sitemap.xml + robots.txt
- favicon (アイコンセット)
- Lighthouse スコア確認 (Performance / Accessibility / Best Practices / SEO)
- アクセシビリティ確認 (キーボード操作、スクリーンリーダ、コントラスト)
- 実コンテンツ差し替え (本物の経歴・記事・メアド・SNS アカウント)

---

## 保留中の判断 (Pause Items)

- **WORKS セクション復活** ⏸
  - 復活条件: 個人開発が3件以上 or 公開可能な大型プロジェクト (展覧会等) が2件以上溜まったタイミング
  - 詳細ページのテンプレート: `_mockups/_archive/option-1-cyber-v2-detail.html`
  - グリッドのテンプレート: 現行 WRITING グリッド構造を流用
  - 復活時のセクション番号: `02 WORKS / 03 WRITING / 04 CONTACT` に組み替え
- **言語切替 (EN / JP)** ⏸ — 現状 Header に表示のみ。実装するか未定
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
