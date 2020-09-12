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
  - `sh script/build.sh`
- git
  - ステージングするなら、`git add <file name>`
  - ログに書き込むなら、`git commit -m "<message>"`
  - デプロイするなら、`git push`

## 処理の流れ
### 動的サイト
- クライアントがリクエストを投げる(urlを叩く)
- pythonサーバー(run.py)がリクエストを受け取る
- 対応する関数を実行する(例えば、`/art.html`にアクセスされたとすると, `@app.route('/art.html')`の下の関数が実行される. この例では`art`関数. )
- 大体において, 関数内でflaskの`render_template`関数を実行する. 
  - `render_template`は第一引数としてファイルパス(templatesのhtml)を受け取り, それをレンダリングしたhtmlファイルを返す. (レンダリングとは, いい感じに画像を埋め込んだりすること.)
  - また, `render_template`は第二引数以降に任意の変数を受け取ることができ, その変数をパス先のファイル(templatesのhtml)内で使用することができる.

`tempates`内のhtmlファイルにおいて変数を使用する方法は`{{変数名}}`など, 記法が存在し, ifやforなども使用できる. それらに関してはjinja2と検索するとわかるよ. (flaskはjinja2というテンプレートエンジンを使用している.)

### 静的サイト
- 全てのリクエストに対して予めhtmlファイルをレンダリングしておく. (`sh script/build.sh`がそれに当たる.)
- リクエストが来たら, そのhtmlファイルを返す. (以上!)