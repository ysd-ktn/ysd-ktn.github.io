# ysd-ktn.github.io
このリポジトリはysd-ktn.github.ioのリポジトリです。

## 使用技術
|技術|名前|
|:--|:--|
| Webフレームワーク| flask|
| ホスティング| Github Pages|

## 構成
- static
  - css: CSSのフォルダ。実際にはSASSで書く予定。
- templates: flaskにおけるテンプレートフォルダ。ここにレンダリング前のHTMLファイルが入っている。
- .gitignore: gitに追跡させないもの。
- build.sh: 静的サイト化する時のビルドスクリプト。
- freeze.py: 静的サイト化するpythonファイルで、build.shから呼び出されるので、自ら実行することはない。
- index.html: レンダリングされた静的ファイル。
- run.py: flaskサーバーを立ち上げるためのファイル。手元で確認する際に使用するのみで、実際のホスティングには使用しない。

## フロー
1. flaskのサーバーを立ち上げ、コーディング。
2. ある程度できたら`sh build.sh`で静的サイト化。
3. ステージングするなら、`git add <file name>`
4. ログに書き込むなら、`git commit -m "<message>"`
5. デプロイするなら、`git push`
