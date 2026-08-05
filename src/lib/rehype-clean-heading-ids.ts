// 見出しの id を、絵文字を含んでいてもきれいな形で振る rehype プラグイン。
//
// 何が問題だったか:
//   記事の見出しには絵文字を使う (`## 🔧 前提` など)。
//   Astro 標準の id 生成 (github-slugger) にそのまま通すと、絵文字が
//   落ちた跡に区切りのハイフンだけが残って `#-前提` になる。
//   `## 😵 詰まったこと・気づいたこと` は `#-詰まったこと気づいたこと`。
//   動作はするが、共有した URL に意味のないハイフンが出て不格好。
//
// なぜ「後から直す」ではなく「先に振る」のか:
//   Astro のプラグイン適用順は  ユーザーの rehypePlugins → rehypeHeadingIds。
//   つまりユーザー側は必ず先に走るので、後から id を書き換えることはできない。
//   一方 rehypeHeadingIds は `typeof node.properties.id !== 'string'` の
//   ときだけ id を振る = 既に id があれば尊重する。
//   なのでこちらで先に正しい id を置いておけば、Astro はそれをそのまま使い、
//   render() が返す headings 配列にも同じ slug が載る。
//   本文のアンカーと目次のリンクが自動的に一致する。

import Slugger from 'github-slugger';
import type { Root, Element, ElementContent } from 'hast';

const HEADINGS = new Set(['h1', 'h2', 'h3', 'h4', 'h5', 'h6']);

/** 絵文字が除去された跡のハイフンを掃除する */
export function cleanId(raw: string): string {
  return raw
    .replace(/-{2,}/g, '-') // 連続ハイフンを 1 本に
    .replace(/^-+|-+$/g, ''); // 前後のハイフンを削除
}

/** 見出しの表示テキストを取り出す (インラインコードや強調の中身も拾う) */
function textOf(node: Element | ElementContent): string {
  if (node.type === 'text') return node.value;
  if (node.type === 'element') return (node.children ?? []).map(textOf).join('');
  return '';
}

export default function rehypeCleanHeadingIds() {
  return (tree: Root) => {
    const slugger = new Slugger();
    const used = new Set<string>();
    let fallback = 0;

    const walk = (node: Root | Element): void => {
      for (const child of node.children ?? []) {
        if (child.type !== 'element') continue;
        const el = child as Element;

        if (HEADINGS.has(el.tagName)) {
          el.properties ??= {};
          // 手書きで id が指定されている場合は触らない
          if (typeof el.properties.id !== 'string') {
            let id = cleanId(slugger.slug(textOf(el)));
            // 絵文字だけの見出しなどで空になったら連番に落とす
            if (!id) id = `section-${++fallback}`;
            // 掃除の結果ぶつかったら採番する
            if (used.has(id)) {
              let i = 2;
              while (used.has(`${id}-${i}`)) i++;
              id = `${id}-${i}`;
            }
            used.add(id);
            el.properties.id = id;
          }
        }
        walk(el);
      }
    };

    walk(tree);
  };
}
