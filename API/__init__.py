import flask as fk
import datetime as dt
import pytube as pt

import file_manager as fm

from .music import musicBP

server = fk.Flask(__name__)

# Register the Blueprints
server.register_blueprint(musicBP, url_prefix='/music')

@server.route('/')
def info():
    return fm.read("API/index.html"), 200

