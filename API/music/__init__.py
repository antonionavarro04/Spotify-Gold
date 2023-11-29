from flask import Blueprint, jsonify, request, send_file
import pytube as pt
import os
import multiprocessing as mp
import moviepy.editor as mop
import secrets as sc
import string
import time

def deleteFile(file_name: str):
    # sleep for 10 seconds to allow the file to be downloaded
    abs_file_path = os.path.abspath(file_name)
    os.remove(abs_file_path)


def download(vid: pt.Stream, file_name: str):
    vid.download(filename=file_name)

musicBP = Blueprint('music', __name__)

@musicBP.get('/<string:code>')
def get(code: str):
    url = "https://www.youtube.com/watch?v=" + code
    yt = pt.YouTube(url)
    vid = yt.streams.filter(only_audio=True, abr="320kbps").first()

    file_name = ''.join(sc.choice(string.ascii_letters) for i in range(10)) + ".mp4"
    abs_file_path = os.path.abspath(file_name); print(abs_file_path)

    download_async = mp.Process(target=download, args=(vid, abs_file_path))
    download_async.start()
    download_async.join()
    
    response = send_file(abs_file_path, as_attachment=True)

    mp.Process(target=deleteFile, args=(abs_file_path,)).start()

    return response, 200
