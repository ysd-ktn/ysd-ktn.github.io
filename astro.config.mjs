// @ts-check
import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';
import rehypeCleanHeadingIds from './src/lib/rehype-clean-heading-ids.ts';
import rehypeLinkCards from './src/lib/rehype-link-cards.ts';
import remarkLineBreaks from './src/lib/remark-line-breaks.ts';

// ysd-ktn.github.io はユーザーページ (root deploy) なので base は "/"
// プロジェクトページに変更する場合は base: "/repo-name/" に変更
export default defineConfig({
  site: 'https://ysd-ktn.github.io',
  integrations: [sitemap()],
  markdown: {
    // 記事は Notion / note で書いているので、 Enter がそのまま改行になる
    // 感覚に合わせる (標準の Markdown では単独の改行は空白扱いで無視される)。
    remarkPlugins: [remarkLineBreaks],

    // rehypeCleanHeadingIds:
    //   見出しには絵文字を使う (`## 🔧 前提` など)。 標準の id 生成だと
    //   絵文字が落ちた跡にハイフンが残って `#-前提` になるので先回りする。
    // rehypeLinkCards:
    //   単独で置かれたリンクを枠付きのカードにする。
    // 詳細は各プラグイン側のコメント参照。
    rehypePlugins: [rehypeCleanHeadingIds, rehypeLinkCards],
  },
});
