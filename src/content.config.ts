// src/content.config.ts
//
// Phase 5 で導入した Astro Content Collections + Zod スキーマ定義。
//
// 4つのコレクションを `file()` loader で JSON ファイルから読み込む:
//   - profile  (シングルトン: src/content/profile.json の "main" キー)
//   - writing  (シングルトン: src/content/writing.json の "main" キー)
//   - timeline (配列: src/content/timeline.json)
//   - contact  (配列: src/content/contact.json)
//
// Zod スキーマで JSON の「形」を保証する:
//   - 必須フィールドの欠落を build 時に検出
//   - 配列の長さや featured 件数を refine で制約
//   - typo (例: "focas") は schema にないキー扱いで素通りするので、
//     「無くてはいけないキー」だけ z.object に列挙し、不要キーは strict() で塞ぐ
//
// 使い方 (各コンポーネント側):
//   import { getEntry, getCollection } from 'astro:content';
//   const profile = (await getEntry('profile', 'main'))!.data;
//   const timeline = await getCollection('timeline');
//
// JSON ファイルの shape:
//   profile.json  — { "main": { ...ProfileSchema } }
//   writing.json  — { "main": { ...WritingSchema } }
//   timeline.json — [ { id: "...", ...TimelineEntrySchema }, ... ]
//   contact.json  — [ { id: "...", ...ContactEntrySchema }, ... ]

import { defineCollection, z } from 'astro:content';
import { file, glob } from 'astro/loaders';

// ───────────────────────────────────────────────
// PROFILE — Hero + About PROFILE シートで使う
// ───────────────────────────────────────────────
const profileSchema = z
  .object({
    // Hero 用
    /** 名前1行目 (Barlow 800 で巨大表示。例: "YOSHIDA") */
    nameFirst: z.string(),
    /** 名前2行目 (例: "KOTONE") */
    nameLast: z.string(),
    /** 日本語表記 (About PROFILE の NAME 行で併記。例: "吉田琴音") */
    nameJp: z.string(),
    /** Hero 下の役職・状況1行 (例: "UI DESIGNER · BASED IN OSAKA, JP · OPEN FOR PROJECTS — 2026 Q3") */
    roleHero: z.string(),
    /** 緯度 (Hero メタ行で表示) */
    lat: z.number(),
    /** 経度 (Hero メタ行で表示) */
    lng: z.number(),
    /** 最終更新 (例: "2026.04.26") */
    lastUpdate: z.string(),

    // About PROFILE 用
    /** LOCATION の日本語 (例: "大阪府") */
    locationJp: z.string(),
    /** LOCATION の英語 (例: "Osaka, Japan") */
    locationEn: z.string(),
    /** UTC オフセット (例: "+9") */
    utc: z.string(),
    /** FOCUS の項目 (例: ["Product UI", "Design Systems", ...]) */
    focus: z.array(z.string()).min(1),
    /** TOOLS の項目 (例: ["Figma", "After Effects", ...]) */
    tools: z.array(z.string()).min(1),
    /** WRITING 行の自己紹介文 */
    writingIntro: z.string(),
    /** note プロフィールの URL (一覧ページ下部の CTA で使う) */
    noteProfileUrl: z.string().url(),
    /** /writing/ の巨大タイトル下に「画面に出す」リード文。
     *  1要素 = 1行 (配列の区切りがそのまま改行になる)。
     *  すぐ下にタグのフィルタ行が来るので、 話題の羅列を書くと重複する。
     *  フィルタからは読み取れないこと (書き手の態度) だけを短く書く。 */
    writingLede: z.array(z.string()).min(1).max(3),
    /** /writing/ の meta description。 検索結果や SNS のカードに出る文章。
     *  画面のリード文とは読む相手も目的も違うので別で持つ。
     *  こちらは検索で拾われたい語を入れて 120 字前後にするのが目安。 */
    writingDescription: z.string().min(20).max(160),
  })
  .strict();

// ───────────────────────────────────────────────
// TIMELINE — 経歴 + 展覧会の混在配列
// 単発イベント (date + tag) と期間付き (from/to + role) の2形態を許容
// ───────────────────────────────────────────────
const timelineSchema = z
  .object({
    /** Astro file() loader が array 形式で要求する一意 ID。
     *  Astro はこの id を entry のキーに使うが、 schema 検証時にも data に残るので
     *  strict() スキーマでは明示的に許容する必要がある。 */
    id: z.string(),
    /** 単発イベント (展覧会等) の日付 (例: "2024.10")。期間エントリでは未指定 */
    date: z.string().optional(),
    /** 期間: 開始 (例: "2024.04")。単発エントリでは未指定 */
    from: z.string().optional(),
    /** 期間: 終了 (例: "PRESENT" or "2024.03")。単発エントリでは未指定 */
    to: z.string().optional(),
    /** バッジ表示 (例: "EXHIBITION")。あれば左に ● バッジ風に表示 */
    tag: z.string().optional(),
    /** タイトル (太字、例: "株式会社ACME" or "「TYPE WALK」展") */
    title: z.string(),
    /** 単発イベント用の補足 (例: "主催 — 大阪") */
    detail: z.string().optional(),
    /** 期間エントリ用の役職 (例: "UI Designer") */
    role: z.string().optional(),
    /** 外部リンク URL (例: 展覧会サイト)。省略可 */
    url: z.string().url().optional(),
    /** トグル詳細: 本文テキスト (複数段落は "\n\n" で区切る)。省略可 */
    description: z.string().optional(),
    /** トグル詳細: リンク一覧。省略可 */
    links: z
      .array(z.object({ label: z.string(), url: z.string().url() }).strict())
      .optional(),
    /** 表示順序。降順で並べる (大きいほど上=新しい) */
    order: z.number().int(),
  })
  .strict()
  .refine(
    (e) => Boolean(e.date) !== Boolean(e.from || e.to),
    { message: '単発エントリは date のみ、期間エントリは from + to のみを指定してください' }
  );

// ───────────────────────────────────────────────
// WRITING — 記事コレクション (src/content/writing/*.md)
//
// 自サイトに移行済みの記事と、まだ note にしか無い記事を「1つの
// コレクション」で扱う。 外部記事も本文なしの .md をスタブとして置き、
// externalUrl を持たせる。 こうすると一覧の並び替え・タグ絞り込み・
// 件数カウントを1箇所のロジックで書けて、移行が進むたびに
// externalUrl を消して本文を足すだけで内部記事に切り替わる。
//
// 並び順は date の降順。 トップの LATEST セクションはこの上位3件を
// 自動で拾うので、 featured フラグの手動管理は不要。
// ───────────────────────────────────────────────

/** 一覧のフィルタと連動するタグ。 z.enum なので typo はビルドで落ちる */
const TAGS = ['DESIGN', 'CAREER', 'AI'] as const;

const writingSchema = z
  .object({
    /** 記事タイトル */
    title: z.string(),
    /** 公開日。 "2026-06-21" のような文字列を Date に変換する */
    date: z.coerce.date(),
    /** タグ (1〜3個想定)。 一覧ページのフィルタになる */
    tags: z.array(z.enum(TAGS)).min(1).max(3),
    /** カード / 記事ヘッダのサムネ。
     *  通常は public/ 配下の絶対パス ("/images/writing/xxx/thumb.webp")。
     *  自サイトに移行しない外部記事に限り、 note 側の画像 URL を
     *  そのまま指してもよい (手元に画像が無いため)。 */
    thumbnail: z.string().regex(/^(\/|https:\/\/)/, {
      message: 'thumbnail は "/" 始まりのパスか https:// の URL にしてください',
    }),
    /** OGP 用画像。 WebP は一部 SNS が読めないので 1200×630 の JPG を指す。
     *  自サイトで公開する記事では必須 (下の refine で検査)。 */
    ogpImage: z.string().startsWith('/').optional(),
    /** note にも載せている場合の URL。 canonical は自サイト側に置き、
     *  記事ページから note へのリンクは張らない (導線は note → 自サイトの一方通行)。 */
    noteUrl: z.string().url().optional(),
    /** まだ自サイトに移行していない記事の外部リンク先。
     *  これがある = 一覧カードから直接 note へ飛ばす (個別ページを作らない)。 */
    externalUrl: z.string().url().optional(),
    /** カードに出す概要 (2行に切り詰めて表示) */
    excerpt: z.string().min(10),
    /** true の間はビルド対象から外す */
    draft: z.boolean().default(false),
  })
  .strict()
  .refine((a) => Boolean(a.externalUrl) || Boolean(a.ogpImage), {
    message:
      '自サイトで公開する記事 (externalUrl なし) には ogpImage が必要です。' +
      'script/notion-to-md.py が ogp.jpg を書き出すのでそのパスを指定してください',
  });

// ───────────────────────────────────────────────
// CONTACT — key/value 連絡先リスト
// action 省略時は href が "mailto:" で始まれば copy、それ以外は external
// ───────────────────────────────────────────────
const contactSchema = z
  .object({
    /** Astro file() loader が array 形式で要求する一意 ID (timeline と同じ事情) */
    id: z.string(),
    /** ラベル (例: "EMAIL", "X", "GITHUB") */
    label: z.string(),
    /** 表示文字列 (例: "hello@example.com", "@ysd_ktn") */
    value: z.string(),
    /** リンク先 (mailto: / https://) */
    href: z.string(),
    /** ext の挙動。省略時は href から自動判定 */
    action: z.enum(['copy', 'external']).optional(),
    /** 表示順 (昇順) */
    order: z.number().int(),
  })
  .strict();

// ───────────────────────────────────────────────
// Collections export
// ───────────────────────────────────────────────
const profile = defineCollection({
  loader: file('src/content/profile.json'),
  schema: profileSchema,
});

const writing = defineCollection({
  loader: glob({ pattern: '**/*.md', base: './src/content/writing' }),
  schema: writingSchema,
});

const timeline = defineCollection({
  loader: file('src/content/timeline.json'),
  schema: timelineSchema,
});

const contact = defineCollection({
  loader: file('src/content/contact.json'),
  schema: contactSchema,
});

export const collections = { profile, writing, timeline, contact };
