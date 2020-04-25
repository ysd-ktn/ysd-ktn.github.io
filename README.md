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
- 準備
  - flaskのサーバーを立ち上げる。 `python run.py`
  - ターミナルの別タブを開く. `command + t`
  - SCSSファイルの監視。 `sass --watch static/scss/style.scss:static/css/style.css`
- コーティング
  - HTMLを編集するなら、`templates`の中のファイル。
  - SCSSを編集するなら、`static`の中のファイル。
  - (注意)トップにあるHTMLファイルは静的化されたものなので編集しない。
- 静的化
  - `sh build.sh`
- git
  - ステージングするなら、`git add <file name>`
  - ログに書き込むなら、`git commit -m "<message>"`
  - デプロイするなら、`git push`
