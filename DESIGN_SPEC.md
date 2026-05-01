# DESIGN_SPEC.md

ysd-ktn.github.io ポートフォリオサイト全面リニューアル — 設計仕様書

---

## 概要

- 既存の Flask + Frozen-Flask 製ポートフォリオを全面リニューアル
- 対象ユーザー: 採用担当 / クライアント / 同業デザイナー
- ゴール: 「サイト自体が作品」と言えるレベルの完成度を持つブルータリズム系ポートフォリオ
- スコープ: シングルページ (Hero / About / Works / Contact) + Works 詳細下層ページ

---

## マスタービジュアル仕様書

**`_mockups/option-1-cyber-v2-writing-grid.html`** が完成形のレイアウト・カラー・タイポ・モーション全部を含む。Astro 実装時はこのHTMLをコンポーネントに分解していく形で進める。

### 参考ファイル

| ファイル | 内容 |
|---|---|
| `_mockups/option-1-cyber-v2-writing-grid.html` | **採用案・最終版** |
| `_mockups/index.html` | モックアップへの入口ページ |
| `_mockups/_archive/option-1-cyber.html` | v1 BYLD寄せ案 (撤去) |
| `_mockups/_archive/option-2-editorial.html` | 不採用案 (オフホワイト・タイポ大胆) |
| `_mockups/_archive/option-3-hybrid.html` | 不採用案 (折衷・明るい技術ドキュメント) |
| `_mockups/_archive/option-1-cyber-v2-detail.html` | Works 詳細ページ (将来 WORKS 復活時のテンプレート) |

---

## 技術スタック

| 項目 | 採用 | 理由 |
|---|---|---|
| フレームワーク | **Astro** | 静的サイト生成、GitHub Pages 互換、画像最適化自動、コンポーネント分割しやすい、シングルページ用途に最適 |
| ホスティング | **GitHub Pages** (既存リポジトリ ysd-ktn.github.io) | 既存ドメインそのまま使える、無料、コミット即デプロイ |
| デプロイ | **GitHub Actions** | Astro ビルド → gh-pages ブランチに `dist/` 出力、または main の `docs/` に出す方式 |
| 言語 | TypeScript + Astro components (.astro) + 素のCSS | 必要十分、依存最小 |
| パッケージマネージャ | npm | デフォルトでOK |

不採用案: Next.js (オーバースペック), Webflow/Framer (カスタマイズ性), 素のHTML (メンテ性), Flask 継続 (Pythonサーバ不要なため運用負担減らしたい)

---

## デザイン原則 (ブルータリズム解釈)

### 採用した要素

- 等幅フォント (JetBrains Mono) と DIN系サンセリフ (Barlow) のミックス
- グリッドの可視化 (1px 罫線、破線で区切り)
- メタ情報の露出 (座標、UTC時刻、ビューポート、ビルド番号、index)
- 装飾の排除 (影・グラデ・アイコン装飾はゼロ)
- カチッとしたイージング (`cubic-bezier(0.7,0,0.3,1)` または `linear`、ふわっとさせない)
- 番号付きセクション (`00 INDEX`, `01 ABOUT`, ...)
- ブラケット記法 (`[ KEY ]` `[ 01 ]` 等で要素を「タグ付け」する)

### 意図的に避けた要素

- 過度なアニメーション (フェードイン・スクロールトリガー演出など)
- マーキー (検討したが「動く帯」が情報の流れを浅くするので削除済)
- 複数アクセントカラー (1色に絞ってブランド化)
- カラフルな装飾 (作品サムネと喧嘩する)
- 角丸 (例外: ヘッダーピルのみ意図的に丸く)
- レジストレーションマーク・寸法線などの重装飾 (一度試したが Hero がうるさくなったので撤去)

---

## カラー

```css
--bg: #0a0a0c        /* ほぼ黒の背景 */
--bg-alt: #14141a    /* セクション背景の差分 (ホバー時など) */
--fg: #e8e6e0        /* オフホワイトのテキスト */
--dim: #6b6b6b       /* メタ情報・補足色 */
--line: #2a2a2e      /* 罫線・破線 */
--accent: #00f0bd    /* ブライトな青緑 (ティール) */
--warn: #ff4747      /* 警告色 (現在未使用、予備) */
```

### アクセント色 #00f0bd の選定理由

- 当初 neon green `#00ff66` を検討 → 「2000年代テック」感が強く却下
- caution yellow `#fae04a` を試作 → 工事現場感が出すぎて却下
- petrol teal `#009cae` → 渋すぎて目立たず却下
- bright cyan `#00e5ff` → シアンに寄りすぎ
- **最終: `#00f0bd`** (HSL ~170°, 100%, 47%) — 青緑として明確に独自性があり、`#00ff66` と同等の明度を保ちつつ作品サムネを邪魔しない

### スワップ候補 (CSS 冒頭コメントに残してある)

```css
/* --accent: #00f0bd;  bright blue-green / teal (current) */
/* --accent: #00e5ff;  bright cyan */
/* --accent: #009cae;  muted petrol teal */
/* --accent: #fae04a;  caution yellow */
/* --accent: #ff5500;  industrial orange */
/* --accent: #ffffff;  strict monochrome (white) */
```

---

## タイポグラフィ

```css
--font-display: 'Barlow', sans-serif       /* 見出し・タイトル系 */
--font-mono: 'JetBrains Mono', monospace   /* メタ・データ・ラベル */
--font-jp: 'Noto Sans JP', sans-serif      /* 日本語 */
```

### フォント選定理由

- **Barlow**: ユーザー希望の DIN系サンセリフを無料で実現するため。本家 FF DIN / DIN Next は有料、Google Fonts で全weightが揃っており DIN特有の幾何学的・工業的な印象を再現できる。`Barlow Condensed` も検討したが幅が狭すぎたので Barlow Regular に決着
- **JetBrains Mono**: 等幅で技術ドキュメント感を出すため。判読性が高く、データ・座標・ラベル類との相性◎
- **Noto Sans JP**: 日本語サブセット用。Barlow とのウェイト感が揃う

### サイズ・運用

| 用途 | サイズ | フォント | weight |
|---|---|---|---|
| Hero h1 | clamp(56px, 10vw, 168px) | Barlow | 800 |
| Section title | (未使用 / mono ラベルで代替) | - | - |
| Body | 14-15px | Barlow | 400 |
| Meta / labels | 11px | JetBrains Mono | 400-500 |
| Letter-spacing (mono) | 0.08-0.12em | - | - |

---

## 情報設計 (IA)

```
/ (シングルページ)
  ├─ 00 / INDEX     Hero (名前 + 役職 + 座標)
  ├─ 01 / ABOUT     01.1 PROFILE + 01.2 TIMELINE (展覧会含む)
  ├─ 02 / WRITING   note記事の時系列リスト
  ├─ 03 / CONTACT   email + SNS
  └─ FOOTER         画面下固定ステータスバー
```

### WORKS について

当初 WORKS セクションを設計していたが、企業勤めデザイナーで業務作品の公開ができないため、現時点では撤去。
代わりに WRITING (note 記事) を中心に「考えていることのアウトプット」で見せる構成に。

**将来 WORKS を復活させる場合のテンプレート**は `_mockups/_archive/option-1-cyber-v2-detail.html` に保存済み:
- 個人開発プロジェクトが増えた時
- 公開可能な業務外プロジェクト (展覧会の詳細ページなど) を載せたくなった時
- WORKS グリッド (現行 WRITING グリッドの構造を流用可) と詳細ページのデザインがそのまま使える

### 展覧会プロジェクトの扱い

業務外で開催した展覧会は **TIMELINE 内に1行追加** して扱う。年月 + `EXHIBITION` タグ + タイトル + 場所、で記載。
専用セクションを作るほどの数ではないため、TIMELINE で「活動履歴」として一緒に並べる。

---

## セクション仕様

### Header (フローティングピル)

- 上中央に固定 (`top: 20px; left: 50%; transform: translateX(-50%)`)
- 通常時: 180px × 44px のピル形 (`border-radius: 9999px`)、ロゴ + `+` トグル表示
- ホバー時: 720px に展開、ナビ4項目 (`00 INDEX / 01 ABOUT / 02 WORKS / 03 CONTACT`) と言語切替が可視化、`+` が `×` 風に45°回転
- ボーダー: アクセント色 1px、内部仕切りは破線 (var(--line))
- イージング: `cubic-bezier(0.7,0,0.3,1)` 0.55秒

### Hero

- 1行目: `[ 00 / INDEX ] · 34.6937°N · 135.5023°E · LAST UPDATE 2026.04.26` (mono 11px)
- メイン: 名前 (YOSHIDA / KOTONE.) — Barlow 800、ピリオドはアクセント色
- 3行目: `> UI DESIGNER · BASED IN OSAKA, JP · OPEN FOR PROJECTS — 2026 Q3` (mono 13px)
- マーキーは削除済 (情報の流れが浅くなるため)

### About

```
[ 01 ] ABOUT — PROFILE + TIMELINE — v.2026
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

[ 01.1 / PROFILE ]
─────────────────
[NAME]      YOSHIDA KOTONE / 吉田琴音
[ROLE]      UI Designer, occasional illustrator
[LOCATION]  Osaka, Japan (UTC+9)
[FOCUS]     Product UI · Design Systems · Brand Identity · Editorial
[TOOLS]     Figma · After Effects · Cinema 4D · HTML/CSS · Git
[WRITING]   ...
[STATUS]    ● OPEN FOR PROJECTS · 2026.Q3

[ 01.2 / TIMELINE ]
─────────────────
[2024.04 — PRESENT]    STUDIO ACME — UI Designer
[2021.04 — 2024.03]    BYLD INC. — Junior Designer
[2017.04 — 2021.03]    KYOTO UNIV. OF ART — BFA, Design
[2014.04 — 2017.03]    OSAKA HIGHSCHOOL
```

- PROFILE: key/value のスペックシート (現状 dl + dt/dd で実装、Astro では JSON から生成)
- TIMELINE: 学歴・職歴を一緒に時系列で表示。`PRESENT` はアクセント色で強調
- 区切りは破線 (var(--line))

### Writing (現行構成)

**レイアウト**: 3カラムカードグリッド (旧 WORKS グリッドのCSS言語を踏襲)
- グリッド枠は1px罫線、カード間も1pxセパレータ
- 各カード = メタ行 + 16:9サムネ + タイトル + 概要 + 外部リンクフッタ

**表示件数**: 最大3件（手動セレクトで選んだ "FEATURED" 記事）
- セクション見出しに `FEATURED · 3 OF N` 形式で全体数を提示
- 自動的に最新3件にする運用も可能だが、編集眼を出すため手動セレクトを採用
- データ側は `featured: true` フラグまたは `priority` フィールドで管理

**MORE リンク**: グリッド下に `VIEW ALL ARTICLES · +N MORE → NOTE PROFILE ↗` のCTAボックス
- ホバーでアクセント色に
- 外部リンクで note プロフィールへ
- N の値は手動更新 (年に2-3回程度の更新運用想定)

**いいね数表示**: 各カードのメタ行右に `♡ N` の実数で表示、アクセント色強調
- メンテ負担: note の API は非公開、手動更新運用（リリース時 + 半年に1回など）
- データ側に `showLikes: false` フラグ持たせて、低い数値の記事は個別に非表示にできる構造を残す
- 表示する数字は丸めない（"100+" 等の桁表示はしない）

**カード構造**
```
[ 01 ]  2025.10                    ♡ 312
[ ⊕ THUMB · 16:9 (filter: grayscale 0.85) ]
タイトル (Barlow 17px, weight 700)
概要 (12px, dim, 6行程度)
─────────────────────
NOTE                  → READ ↗
```

**ホバー**
- カード背景が `--bg-alt` にうっすら
- タイトル・サムネ枠線がアクセント色に

### Works (現状なし、将来追加候補)

- 撤去理由: 業務作品が NDA で公開不可、学生作品のみだとガラガラ感が出る
- 復活タイミング: 個人開発プロジェクトが3件以上溜まったタイミング、または展覧会等の公開可能な大型プロジェクトが2件以上できた時
- 復活時のテンプレート: `_mockups/_archive/option-1-cyber-v2-detail.html` (詳細ページ) と現行 `option-1-cyber-v2-writing-grid.html` (WRITING グリッド構造を流用) を参照
- セクション番号: 復活時は WRITING との順序を `02 WORKS / 03 WRITING / 04 CONTACT` のように調整

### Contact

- key/value のリスト形式
- `[ EMAIL ]` `hello@example.com` → COPY
- `[ X ]` `@ysd_ktn` → EXTERNAL
- `[ INSTAGRAM ]` `@ysd.ktn` → EXTERNAL
- `[ GITHUB ]` `github.com/ysd-ktn` → EXTERNAL
- 値は Barlow 22px

### Footer (ステータスバー)

- 画面下に常時固定 (position: fixed)
- 表示: `● ONLINE · BUILD 0426.2026` / `CURSOR · X,Y` / `UTC HH:MM:SS` / `VIEWPORT · W×H`
- `●` は1.4秒ごとに点滅 (アクセント色)

---

## カーソル (Custom)

- マウス追従の SVG カーソル (Figma 風の選択矢印 ▲、塗りはアクセント色 + bg outline)
- 追従するアノテーションチップ + 座標表示
- ホバー対象に応じてチップ文言が変化:
  - リンク → `OPEN LINK`
  - ボタン → `FILTER`
  - .work カード → `OPEN PROJECT`
- 意図: UIデザイナーの作業ツール (Figma / Sketch 等) のメタファー。「自分=デザインツールを使う人」を視覚的に提示

実装上の注意: タッチデバイスでは hover が効かないので、`@media (hover: hover)` で出し分け推奨。

---

## コンテンツモデル (Astro 実装時の指針)

### 記事 (Writing)

`src/content/writing.json` で配列管理 (note記事へのリンク集):

```json
{
  "noteProfileUrl": "https://note.com/ysd_ktn",
  "totalCount": 15,
  "articles": [
    {
      "date": "2025.10",
      "title": "デザインシステムを5年運用して学んだこと",
      "excerpt": "運用フェーズで見えた、ガバナンスと拡張性のバランス...",
      "url": "https://note.com/ysd_ktn/n/xxxxxxx",
      "platform": "NOTE",
      "thumbnail": "../../assets/writing/2025-10-design-system.png",
      "likes": 312,
      "featured": true,
      "showLikes": true
    }
  ]
}
```

- `featured: true` の記事だけがトップに表示される（最大3件）
- `featured: true` が4件以上ある場合は配列順で上から3件を採用、または `priority` フィールドで明示
- `showLikes: false` にすると個別カードのいいね数を非表示にできる
- `totalCount` は MORE リンクの `+N MORE` 表示に使用、手動更新

### 経歴 (Timeline)

`src/content/timeline.json` で配列管理。雇用と単発イベント (展覧会等) を混在可能:

```json
[
  { "date": "2024.10", "tag": "EXHIBITION", "title": "「TYPE WALK」展", "detail": "主催 — 大阪" },
  { "from": "2024.04", "to": "PRESENT", "title": "株式会社ACME", "role": "UI Designer" },
  { "from": "2021.04", "to": "2024.03", "title": "BYLD Inc.", "role": "Junior Designer" }
]
```

`tag` フィールドがあれば EXHIBITION バッジが表示され、`from`/`to` があれば期間付きの雇用エントリとして表示。

### 作品 (Works) — 将来用

WORKS が復活したら `src/content/works/*.md` で 1作品1ファイル管理:

```yaml
---
title: "Banking App Redesign"
slug: "banking-app-2026"
category: "ui-product"  # ui-product | branding | graphic | experiment
year: 2026
client: "STARTUP A"
role: "Lead UI Designer"
tools: ["Figma", "After Effects"]
thumbnail: "../../assets/works/banking/thumb.png"
heroImage: "../../assets/works/banking/hero.png"
description: "短い概要 (2-3行)"
order: 1
draft: false
---

# 詳細マークダウン本文 (Problem / Process / Solution / Outcome)
```

### プロフィール (Profile)

`src/content/profile.json`:

```json
{
  "name": "Yoshida Kotone",
  "nameJp": "吉田琴音",
  "role": "UI Designer",
  "location": "Osaka, Japan",
  "lat": 34.6937,
  "lng": 135.5023,
  "focus": ["Product UI", "Design Systems", "Brand Identity", "Editorial"],
  "tools": ["Figma", "After Effects", "Cinema 4D", "HTML/CSS", "Git"],
  "status": { "open": true, "quarter": "2026.Q3" }
}
```

---

## 残タスク (新セッションで対応)

### 実装フェーズ

1. Astroプロジェクト初期化 (`npm create astro@latest`)
2. **既存リポジトリの整理 (重要)**: Flask 関連 (`run.py`, `freeze.py`, `templates/`, `static/`, `__pycache__/`, 古い HTML ファイル) をどうするか相談
   - 選択肢: ① 削除 ② `_old/` フォルダに退避 ③ `legacy` ブランチに移動して main から削除
3. v2 HTML を Astro components に分解
   - `Header.astro`, `Hero.astro`, `About.astro`, `Works.astro`, `Contact.astro`, `Footer.astro`, `Cursor.astro`
4. グローバル CSS 移植 (CSS変数・フォント import)
5. Content Collections セットアップ (works / timeline / profile)
6. 作品一覧ページのフィルタリング JS 実装
7. 作品詳細ページ `[slug].astro` の作成 (テンプレ未デザインなので別途仕様詰める)
8. GitHub Actions ワークフロー (`.github/workflows/deploy.yml`)
9. レスポンシブ対応 (現状 desktop 想定、モバイルレイアウトは要設計)
10. SEO (meta tags, OG image, sitemap.xml, robots.txt)
11. Lighthouse / アクセシビリティチェック

### コンテンツ

- 実際の作品の棚卸し (写真・概要・カテゴリ・年代を整理)
- 経歴の差し替え (現在ダミー)
- プロフィール文言の確定
- メールアドレス・SNSアカウントの確定
- ファビコン・OG画像の作成

### 未決事項

- **言語切替 (EN/JP)** を実装するかどうか — 現状ヘッダーに `EN / JP` リンクがあるが機能未実装
- **作品詳細ページのテンプレート** — 現状未デザイン、最低限のフォーマット (Problem/Process/Final/Outcome) を別途デザイン
- **モバイル対応** — Hero フォントサイズ・ヘッダーピルの挙動・カーソル無効化など
- **ダークモード以外への対応** — 現状ダーク固定。ライトモード追加するか
- **お問い合わせフォーム vs mailto** — 現状 mailto。フォーム化するなら別途バックエンド要 (Formspree 等)

---

## 新セッションへのバトン

> **進捗管理は `PROGRESS.md` に分離した (2026.05.01〜)**
> このファイル (DESIGN_SPEC.md) は静的な設計仕様、PROGRESS.md は動的な進捗管理。
> 新セッションでは `PROGRESS.md` → `DESIGN_SPEC.md` → モックHTML の順で参照する。

新しいセッションを開始するときは、以下を最初に伝える:

> ポートフォリオサイト ysd-ktn.github.io を Astro で実装中。`PROGRESS.md` で現在のフェーズを確認し、設計仕様は `DESIGN_SPEC.md` と `_mockups/option-1-cyber-v2-writing-grid.html` 参照。

その後の進め方の推奨順:

1. SPEC.md を読む
2. `_mockups/option-1-cyber-v2-writing-grid.html` をブラウザで開いて視覚確認
3. 既存リポジトリの Flask 関連ファイルをどうするか相談
4. Astro プロジェクト初期化
5. v2 のスタイルを Astro グローバル CSS に移植
6. 各セクションを component に分解しながら実装
7. ローカルで `npm run dev` 動かして確認
8. ダミーコンテンツのまま GitHub Pages にデプロイして公開確認
9. 実コンテンツ差し替え

---

最終更新: 2026.04.26
