python freeze.py
find ./build/* -not -name static -exec mv {} . \;
rm -rf build
