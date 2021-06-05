# このスクリプトの場所まで移動
cd `dirname $0`

# 変数定義
base=".."
build="${base}/build"

# 静的化
python "${base}/freeze.py"

if [ ! -e $base/internship ];then
  mkdir $base/internship
fi
if [ ! -e $base/uichallenge ];then
  mkdir $base/uichallenge
fi

mv $build/*.html $base
mv $build/internship/*.html $base/internship
mv $build/uichallenge/*.html $base/uichallenge 
mv $build/developpages/*.html $base/developpages
rm -rf $build