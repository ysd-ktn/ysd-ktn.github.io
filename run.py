import os
import json

from flask import Flask
from flask import render_template

app = Flask(__name__, static_url_path='/static')

@app.route('/')
def index():
    skills_json_path = os.path.join(app.static_folder, 'json/skills.json')
    with open(skills_json_path) as skills_json_file:
        skills_list = json.load(skills_json_file)
    return render_template('index.html', skills_list=skills_list)

@app.route('/art.html')
def art():
    articles_json_path = os.path.join(app.static_folder, 'json/articles.json')
    with open(articles_json_path) as articles_json_file:
        articles_list = json.load(articles_json_file)

    return render_template('art.html', articles_list=articles_list)

@app.route('/design.html')
def design():
    contents_json_path = os.path.join(app.static_folder, 'json/contents.json')
    with open(contents_json_path) as contents_json_file:
        contents_list = json.load(contents_json_file)
    return render_template('design.html', contents_list=contents_list)

@app.route('/designpages/<contents_id>.html')
def contents_30days(contents_id):
    contents_json_path = os.path.join(app.static_folder, 'json/contents.json')
    with open(contents_json_path) as contents_json_file:
        contents_list = json.load(contents_json_file)
    contents = contents_list["30days_UI_Challenge"][int(contents_id)]
    return render_template('/designpages/contents.html', contents=contents)

@app.route('/develop.html')
def develop():
    project_json_path = os.path.join(app.static_folder, 'json/project.json')
    with open(project_json_path) as project_json_file:
        project_list = json.load(project_json_file)
    return render_template('develop.html', project_list=project_list)

@app.route('/developpages/<project_id>.html')
def project_web(project_id):
    project_json_path = os.path.join(app.static_folder, 'json/project.json')
    with open(project_json_path) as project_json_file:
        project_list = json.load(project_json_file)
    project = project_list["WEBサイト"][int(project_id)]
    return render_template('/developpages/project.html', project=project)

if __name__ == '__main__':
    app.run(debug=True)
