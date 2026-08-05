// 単独で置かれたリンクをカード表示に変える rehype プラグイン。
//
// 何をするか:
//   段落の中身がリンク 1 個だけのとき、 それを枠付きのカードにする。
//   本文の途中に出てくるリンクは対象外なので、 文章は普通のまま。
//
//     [1日1作字｜…](/writing/sakuji/)          → 内部記事のカード
//     [@ktn__kk のイラスト](https://instagram…) → 外部リンクのカード
//     https://x.com/ktnkk                       → URL だけのカード
//
// なぜ埋め込みではなくカードなのか:
//   Instagram や X の公式埋め込みは、 相手のスクリプトを読み込む必要があり、
//   静的サイトの表示速度と読者のプライバシーを両方削る。 見た目もこのサイトの
//   トーンから浮く。 リンク先の名前と行き先だけを自前の枠で出す方が、
//   軽くて世界観も保てる。
//
// 表示内容:
//   上段 = リンクテキスト (URL そのままの場合はホスト名に置き換える)
//   下段 = ホスト名 (内部リンクは "THIS SITE")
//   矢印 = 外部は ↗ / 内部は →

import type { Root, Element, ElementContent } from 'hast';

const isBlank = (n: ElementContent): boolean =>
  n.type === 'text' && n.value.trim() === '';

/** リンクの中の文字列を取り出す */
function textOf(node: ElementContent): string {
  if (node.type === 'text') return node.value;
  if (node.type === 'element') return (node.children ?? []).map(textOf).join('');
  return '';
}

export default function rehypeLinkCards() {
  return (tree: Root) => {
    const walk = (node: Root | Element): void => {
      const children = node.children ?? [];

      for (let i = 0; i < children.length; i++) {
        const p = children[i];
        if (p.type !== 'element') continue;

        if (p.tagName === 'p') {
          const meaningful = (p.children ?? []).filter((c) => !isBlank(c));
          const only = meaningful.length === 1 ? meaningful[0] : null;

          if (only && only.type === 'element' && only.tagName === 'a') {
            const href = String(only.properties?.href ?? '');
            if (!href) continue;

            const external = /^https?:\/\//.test(href);
            let host = 'THIS SITE';
            if (external) {
              try {
                host = new URL(href).hostname.replace(/^www\./, '');
              } catch {
                host = href;
              }
            }

            // リンクテキストが URL そのままなら見出しとして使えないので、
            // ホスト名を見出しに回す
            let label = textOf(only).trim();
            if (!label || /^https?:\/\//.test(label)) label = host;

            children[i] = {
              type: 'element',
              tagName: 'a',
              properties: {
                href,
                className: ['link-card'],
                ...(external ? { target: '_blank', rel: ['noopener'] } : {}),
              },
              children: [
                {
                  type: 'element',
                  tagName: 'span',
                  properties: { className: ['lc-label'] },
                  children: [{ type: 'text', value: label }],
                },
                {
                  type: 'element',
                  tagName: 'span',
                  properties: { className: ['lc-meta'] },
                  children: [
                    { type: 'text', value: host },
                    {
                      type: 'element',
                      tagName: 'span',
                      properties: { className: ['lc-arrow'] },
                      children: [{ type: 'text', value: external ? '↗' : '→' }],
                    },
                  ],
                },
              ],
            } as Element;
            continue;
          }
        }

        walk(p);
      }
    };

    walk(tree);
  };
}
