# Standard
import os
import multiprocessing as mp
import json
import html

# Third-party
from flask import Blueprint, jsonify, request, send_file, Response
import pytube as pt
import googleapiclient.discovery as YTAPIdiscovery
import pytube.exceptions as pt_ex

# Local
from secret_variables import youtube_data_api_key as api_key
from ..music import deleteFile, reduceKBPS
import file_manager as fm
import API.functions as fx

# Blueprint
queryBP = Blueprint('query', __name__)

# Create the service object
ytapi = YTAPIdiscovery.build("youtube", "v3", developerKey=api_key)

@queryBP.get('/list/<string:query>')
def getList(query: str) -> Response:
    # ! Make the request
    request = ytapi.search().list(
        part="snippet",
        q=query.strip(),
        maxResults=50
    ); response = request.execute()

    videos = [] # ! Extract the information from the response
    for item in response["items"]:
        try:
            videos.append({
                "title": item["snippet"]["title"],
                "videoId": item["id"]["videoId"],
                "url": f"https://www.youtube.com/watch?v={item['id']['videoId']}",
                "thumbnail": item["snippet"]["thumbnails"]["high"]["url"],
                "channel": item["snippet"]["channelTitle"]
            })
        except KeyError: # Catch this error, it happens when item is not a video
            print(f"KeyError: Skipping item {item['snippet']['title']}")

    # Return the response
    return videos, 200

# ? Test route
""" @queryBP.get('/json')
def getJson() -> Response:
    json_response = {
        "title": "空腹 まふまふ",
        "videoId": "2bYg3y7B2ZI",
        "url": "https://www.youtube.com/watch?v=2bYg3y7B2ZI",
        "thumbnail": "https://i.ytimg.com/vi/2bYg3y7B2ZI/hqdefault.jpg",
        "channel": "まふまふちゃんねる"
    }

    # Encode the json to send it in the header
    json_encoded = json.dumps(json_response, ensure_ascii=True).encode("utf-8")
    
    # Make a response sending the file and the json
    response = send_file("SPG-buzocyglno.mp3", as_attachment=True)

    # Add the "json" header to the response
    response.headers["json"] = json_encoded

    # Return the response
    return response, 200 """

@queryBP.get('/<string:query>')
def get(query: str) -> Response:
    # Strip the query and unescape it
    query = query.strip()

    try: # FIXED: Issue #17 -> https://github.com/antonionavarro04/Spotify-Gold-API/issues/17
        print(f"Query is: {query} with a length of {len(query)}")
    except UnicodeEncodeError:
        query = fx.decode_unicode(query)
        print(f"Query is: {query} with a length of {len(query)}")
    except Exception as e:
        print(f"Unknown Error: {e}")

    # ! Make the request
    request = ytapi.search().list(
        part="snippet",
        q=query,
        maxResults=50
    )
    response = request.execute()

    json_response = {}
    for item in response["items"]:
        try:
            json_response = {
                "title": item["snippet"]["title"],
                "videoId": item["id"]["videoId"],
                "url": f"https://www.youtube.com/watch?v={item['id']['videoId']}",
                "thumbnail": item["snippet"]["thumbnails"]["high"]["url"],
                "channel": item["snippet"]["channelTitle"]
            }
            break
        except KeyError: # Catch this error, not sure what happens sometimes
            print(f"KeyError: Skipping item {item['snippet']['title']}")

    try: # FIXED: Issue #17 -> https://github.com/antonionavarro04/Spotify-Gold-API/issues/17
        print(f"Found video: {json_response['title']} with a videoId of {json_response['videoId']}")
    except UnicodeEncodeError:
        json_response['title'] = fx.decode_unicode(json_response['title'])
        print(f"Found video: {json_response['title']} with a videoId of {json_response['videoId']}")

    # Load the video
    yt = pt.YouTube(json_response['url'])

    # Get the audio file
    kbps = 321

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
    json_response["kbps"] = kbps
    
    # Make a response sending the file and the json
    response = send_file(absolutePath, as_attachment=True)

    # Remove or replace unsupported characters
    json_encoded = json.dumps(json_response, ensure_ascii=True).encode("utf-8")

    # Add the "json" header to the response
    response.headers["json"] = json_encoded

    # Return the response
    return response, 200
