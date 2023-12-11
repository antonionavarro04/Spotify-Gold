from flask import Blueprint, jsonify, request, send_file
import pytube as pt
import os
import multiprocessing as mp
import time
import file_manager as fm

musicBP = Blueprint('music', __name__)

def deleteFile(audio: str) -> None:
    """
    Deletes the audio file

    Parameters
    ----------
    audio : str
        The audio file to delete

    Returns
    -------
    None
    """

    time.sleep(60)
    os.remove(audio)

def reduceKBPS(current_kbps: int) -> int:
    """
    Reduces the KBPS by 1 level\n
    320 -> 128\n
    128 -> 64\n
    64 -> 32

    Parameters
    ----------
    current_kbps : int
        The current KBPS of the video

    Returns
    -------
    int
        The reduced KBPS
    """

    new_kbps = 64

    if (current_kbps == 320):
        new_kbps = 128
    
    return new_kbps

@musicBP.get('/<string:code>')
def get(code: str):
    # Load the video
    url = "https://www.youtube.com/watch?v=" + code
    yt = pt.YouTube(url)

    vid = None
    kbps = 320

    while (vid == None):
        vid = yt.streams.filter(only_audio=True, abr=f"{kbps}kbps").first()
        kbps = reduceKBPS(kbps)

    # Generate a random file name and get the absolute path
    fileName = fm.randomName(extension="mp3")
    absoluteAudioPath = os.path.abspath(fileName)

    # Download the video
    vid.download(filename=fileName)

    # Send the file    
    response = send_file(absoluteAudioPath, as_attachment=True)

    # Delete the files
    mp.Process(target=deleteFile, args=(absoluteAudioPath,)).start()

    # Return the response
    return response, 200