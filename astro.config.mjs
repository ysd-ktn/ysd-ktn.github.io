// @ts-check
import { defineConfig } from 'astro/config';

// ysd-ktn.github.io はユーザーページ (root deploy) なので base は "/"
// プロジェクトページに変更する場合は base: "/repo-name/" に変更
export default defineConfig({
  site: 'https://ysd-ktn.github.io',
});
