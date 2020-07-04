import os
import json

from flask import Flask
from flask import render_template

app = Flask(__name__, static_url_path='/static')

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/art.html')
def art():
    articles_json_path = os.path.join(app.static_folder, 'json/articles.json')
    with open(articles_json_path) as articles_json_file:
        articles_list = json.load(articles_json_file)

    return render_template('art.html', articles_list=articles_list)

if __name__ == '__main__':
    app.run(debug=True)