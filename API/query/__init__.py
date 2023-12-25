from flask import Blueprint, jsonify, request, send_file, Response
import pytube as pt
import os
import multiprocessing as mp
import time
import datetime as dt
import file_manager as fm
from secret_variables import youtube_data_api_key as api_key
import googleapiclient.discovery as YTAPIdiscovery
from ..music import deleteFile, reduceKBPS
import file_manager as fm
import pytube.exceptions as pt_ex

queryBP = Blueprint('query', __name__)
# Create the service object
ytapi = YTAPIdiscovery.build("youtube", "v3", developerKey=api_key)

@queryBP.get('/list/<string:query>')
def getList(query: str) -> Response:
    # Strip the query
    query = query.strip()
    print(f"Query is: {query} with a length of {len(query)}")

    # ! Make the request
    request = ytapi.search().list(
        part="snippet",
        q=query,
        maxResults=50
    )
    response = request.execute()

    # ! Extract the information
    videos = []
    for item in response["items"]:
        try:
            videos.append({
                "title": item["snippet"]["title"],
                "videoId": item["id"]["videoId"],
                "url": f"https://www.youtube.com/watch?v={item['id']['videoId']}",
                "thumbnail": item["snippet"]["thumbnails"]["high"]["url"],
                "channel": item["snippet"]["channelTitle"]
            })
        except KeyError: # Catch this error, not sure what happens sometimes
            print(f"KeyError: Skipping item {item['snippet']['title']}")

    print(f"Found {len(videos)} videos")

    # Return the response
    return videos, 200

@queryBP.get('/<string:query>')
def get(query: str) -> Response:
    # Strip the query
    query = query.strip()
    print(f"Query is: {query} with a length of {len(query)}")

    # ! Make the request
    request = ytapi.search().list(
        part="snippet",
        q=query,
        maxResults=50
    )
    response = request.execute()

    json = {}
    for item in response["items"]:
        try:
            json = {
                "title": item["snippet"]["title"],
                "videoId": item["id"]["videoId"],
                "url": f"https://www.youtube.com/watch?v={item['id']['videoId']}",
                "thumbnail": item["snippet"]["thumbnails"]["high"]["url"],
                "channel": item["snippet"]["channelTitle"]
            }
            break
        except KeyError: # Catch this error, not sure what happens sometimes
            print(f"KeyError: Skipping item {item['snippet']['title']}")

    print(f"Found video: {json['title']} with a videoId of {json['videoId']}")

    # Load the video
    yt = pt.YouTube(json['url'])

    # Get the audio file
    kbps = 320

    # Get the stream
    stream = None

    # Reduce the KBPS
    while (stream == None):
        kbps = reduceKBPS(kbps)
        try:
            stream = yt.streams.filter(only_audio=True, abr=f"{kbps}kbps").first()
        except pt_ex.AgeRestrictedError as e:
            print("Age restricted video, skipping")
            return fm.read("API/forbidden.html").replace("$$TYPE$$", "Age Resticted"), 403
        except Exception as e:
            print(f"Error: {e}")
            return None, 500

    #filename = f"{json['title']}[{fm.randomName(5)}].mp3" Problems with windows limitations to file names
    filename = f"SPG-{fm.randomName(10)}.mp3"

    stream.download(filename=filename)
    
    # Get the absolute path
    absolutePath = os.path.abspath(filename)

    # Start the process to delete the file
    mp.Process(target=deleteFile, args=(absolutePath,)).start()

    # Add the KBPS to the response
    json["kbps"] = kbps
    
    # Make a response sending the file and the json
    response = send_file(absolutePath, as_attachment=True)

    # Remove or replace unsupported characters
    json["title"] = json["title"].encode('latin-1', 'ignore').decode('latin-1')

    # Add the "json" header to the response
    response.headers["json"] = json

    # Return the response
    return response, 200
