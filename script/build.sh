# このスクリプトの場所まで移動
cd `dirname $0`

# 変数定義
base=".."
build="${base}/build"

# 静的化
python "${base}/freeze.py"
mv $build/*.html $base
rm -rf $build