// 記事コレクションの共通ロジック。
// 一覧ページ / 記事ページ / トップの LATEST セクションが同じ並び順と
// 同じ「外部記事かどうか」の判定を使えるよう、ここに集約する。

import { getCollection, type CollectionEntry } from 'astro:content';

export type Article = CollectionEntry<'writing'>;

/** 一覧のフィルタに出すタグ。 content.config.ts の z.enum と揃えること */
export const TAGS = ['DESIGN', 'CAREER', 'AI'] as const;

/**
 * 公開記事を新しい順で返す。
 * draft は本番ビルドでのみ除外する (dev では下書きを確認したいため)。
 */
export async function getArticles(): Promise<Article[]> {
  const all = await getCollection('writing', ({ data }) =>
    import.meta.env.PROD ? !data.draft : true
  );
  return all.sort((a, b) => b.data.date.valueOf() - a.data.date.valueOf());
}

/** まだ note にしか無い記事か (= 個別ページを持たない) */
export const isExternal = (a: Article): boolean => Boolean(a.data.externalUrl);

/** カードのリンク先。 外部記事は note、 移行済みは自サイトの個別ページ */
export const hrefOf = (a: Article): string =>
  a.data.externalUrl ?? `/writing/${a.id}/`;

/** カードやメタ行の日付表記 (例: 2026.06) */
export const ym = (d: Date): string =>
  `${d.getFullYear()}.${String(d.getMonth() + 1).padStart(2, '0')}`;

/** 記事ヘッダの日付表記 (例: 2026.06.21) */
export const ymd = (d: Date): string =>
  `${ym(d)}.${String(d.getDate()).padStart(2, '0')}`;

/**
 * 読了時間の見積り。 日本語は 1 分 600 字が目安。
 * 画像・コードブロック・記号を除いた実文字数で数える。
 */
export function readingMinutes(body: string): number {
  const text = body
    .replace(/```[\s\S]*?```/g, '')
    .replace(/!\[[^\]]*\]\([^)]*\)/g, '')
    .replace(/[#*`>|\-\s]/g, '');
  return Math.max(1, Math.round(text.length / 600));
}

/**
 * 目次に載せる見出しを選ぶ。
 *
 * 引数は Astro の `render()` が返す headings をそのまま渡す。
 * 自前で Markdown を正規表現で舐めて slug を作ると、 本文側に実際に
 * 振られた id (github-slugger 生成) とズレてアンカーが死ぬ。
 * レンダラが返した slug をそのまま使うのが唯一確実。
 *
 * H2 のみに絞るのは、 目次がそのまま進捗レールを兼ねる設計のため。
 * H3 まで載せると章の粒度が細かくなり、 記事ごとに目次の密度が
 * バラついてレールの動きが安定しない。
 */
export interface Heading {
  depth: number;
  slug: string;
  text: string;
}

export const tocFrom = (headings: Heading[]): Heading[] =>
  headings.filter((h) => h.depth === 2);
