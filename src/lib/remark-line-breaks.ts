// 本文中の単なる改行を、そのまま改行として出す remark プラグイン。
//
// なぜ必要か:
//   Markdown の標準では、段落の中の 1 個の改行は「空白」として扱われ、
//   前後の行が繋がって 1 行に流し込まれる。 改行したい場合は行末に
//   半角スペース 2 個を置くか `<br>` を書く、というのが本来の作法。
//
//   ただ記事は Notion や note で書いていて、 あちらは Enter を押した
//   ところがそのまま改行になる。 書いた本人の意図は「ここで改行」なので、
//   そのまま出す方が元記事の見た目に一致する。
//
//   例 (引用の中):
//     > **Chat**:
//     > ブラウザやモバイルから使える通常のチャット。
//   これを標準の挙動に任せると「**Chat**: ブラウザやモバイルから…」と
//   1 行に繋がってしまう。
//
// 影響範囲:
//   コードブロックは text ノードではないので触らない。
//   段落を読みやすさのために折り返して書くと余計な改行が入るが、
//   この記事群は 1 段落 1 行で書かれているため問題にならない。

import type { Root, Parent, RootContent } from 'mdast';

export default function remarkLineBreaks() {
  return (tree: Root) => {
    const walk = (node: Parent): void => {
      if (!Array.isArray(node.children)) return;

      const next: RootContent[] = [];
      for (const child of node.children) {
        if (child.type === 'text' && child.value.includes('\n')) {
          // "a\nb" → [text(a), break, text(b)]
          child.value.split('\n').forEach((part, i) => {
            if (i > 0) next.push({ type: 'break' } as RootContent);
            if (part) next.push({ type: 'text', value: part } as RootContent);
          });
          continue;
        }
        next.push(child);
        if ('children' in child) walk(child as Parent);
      }
      node.children = next;
    };

    walk(tree as unknown as Parent);
  };
}
