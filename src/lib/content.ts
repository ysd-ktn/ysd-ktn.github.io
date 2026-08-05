// コンテンツ取得のヘルパー。
//
// なぜ必要か:
//   `(await getEntry('profile', 'main'))!.data` と書くと、 エントリが
//   取れなかったときに TypeScript の `!` は何もしてくれないので、
//   実行時に「Cannot read properties of undefined (reading 'data')」
//   という、どのファイルの話なのか一切分からないエラーだけが出る。
//
//   エントリが取れなくなる原因は主に3つ:
//     - JSON / Markdown がスキーマに違反している (Zod で弾かれる)
//     - ファイルを消した / リネームした
//     - content.config.ts を変更したのに .astro のキャッシュが古い
//
//   どれもコンテンツを触っていれば普通に起きることなので、
//   「何が無いのか」を名指しするエラーにしておく。

import { getEntry, type CollectionKey, type CollectionEntry } from 'astro:content';

/** エントリの data を返す。 無ければ「何が無いのか」を書いたエラーで落とす */
export async function requireEntry<C extends CollectionKey>(
  collection: C,
  id: string
): Promise<CollectionEntry<C>['data']> {
  const entry = await getEntry(collection as any, id as any);

  if (!entry) {
    throw new Error(
      [
        `コンテンツが見つかりません: collection="${collection}" / id="${id}"`,
        '',
        '考えられる原因:',
        `  1. src/content/${collection}.json (または ${collection}/) がスキーマに違反している`,
        '     → ビルドログの上の方に Zod のエラーが出ていないか確認',
        '  2. ファイルを消した / リネームした',
        '  3. content.config.ts を変えたのにキャッシュが古い',
        '     → rm -rf .astro して再ビルド',
      ].join('\n')
    );
  }

  return (entry as CollectionEntry<C>).data;
}
