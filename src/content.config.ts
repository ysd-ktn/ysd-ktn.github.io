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
import { file } from 'astro/loaders';

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
    /** STATUS が ● 強調になるか (true で先頭に ● が出る) */
    statusOpen: z.boolean(),
    /** STATUS の表示テキスト (例: "OPEN FOR PROJECTS · 2026.Q3") */
    statusLabel: z.string(),
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
    /** 表示順序。降順で並べる (大きいほど上=新しい) */
    order: z.number().int(),
  })
  .strict()
  .refine(
    (e) => Boolean(e.date) !== Boolean(e.from || e.to),
    { message: '単発エントリは date のみ、期間エントリは from + to のみを指定してください' }
  );

// ───────────────────────────────────────────────
// WRITING — note 記事リスト + メタ情報
// articles 内で featured: true がちょうど 3件 (3カラムグリッド固定のため)
// ───────────────────────────────────────────────
const writingArticleSchema = z
  .object({
    /** カード左上 [ idx ] 表示。省略時は配列順から自動生成 ("01", "02"...) */
    idx: z.string().optional(),
    /** 投稿日 (例: "2025.10") */
    date: z.string(),
    /** いいね数。省略すると ♡ N 表示自体を非表示 */
    likes: z.number().int().nonnegative().optional(),
    /** サムネ画像のパス (例: "/images/writing/01-xxx.webp")。
     *  public/ 配下の絶対パスを期待 (Astro は public/ をルートにマップ)。
     *  Phase 7 直前に dashed プレースホルダ → 実画像に切替済み (2026.05.05)。 */
    thumbnail: z.string(),
    /** 16:9 サムネ placeholder ラベル。
     *  かつての dashed 枠 + テキスト表示用。 thumbnail に切替後は実用上不要だが、
     *  alt テキストや将来 placeholder へ戻す際の予備として残す (省略可)。 */
    thumbLabel: z.string().optional(),
    /** 記事タイトル */
    title: z.string(),
    /** 概要 (6行程度) */
    excerpt: z.string(),
    /** 記事 URL。本物投入前は "#" でも通る (Phase 8 までに本物に差し替え) */
    url: z.string(),
    /** カードフッタ左の plat 表示。省略時 "NOTE" */
    platform: z.string().optional(),
    /** いいね数を非表示にしたい個別記事は false (likes 値があっても効く) */
    showLikes: z.boolean().optional(),
    /** トップに表示する3件を選ぶフラグ */
    featured: z.boolean(),
  })
  .strict();

const writingSchema = z
  .object({
    /** CTA "→ NOTE PROFILE" のリンク先 */
    noteProfileUrl: z.string().url(),
    /** note 上の全記事数 (CTA の "+N MORE" 表示に使用、 N = totalCount - 3) */
    totalCount: z.number().int().nonnegative(),
    /** 候補記事リスト (featured: true のうち上から3件をトップに表示) */
    articles: z.array(writingArticleSchema).min(3),
  })
  .strict()
  .refine(
    (w) => w.articles.filter((a) => a.featured).length === 3,
    {
      message:
        'articles のうち featured: true の記事はちょうど 3件 にしてください (3カラムグリッド固定のため)',
    }
  );

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
  loader: file('src/content/writing.json'),
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
