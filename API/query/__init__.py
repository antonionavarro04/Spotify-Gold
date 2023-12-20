from flask import Blueprint, jsonify, request, send_file
import pytube as pt
import os
import multiprocessing as mp
import time
import file_manager as fm
from secret_variables import youtube_data_api_key as api_key
import googleapiclient.discovery as YTAPIdiscovery
from ..music import deleteFile, reduceKBPS

queryBP = Blueprint('query', __name__)

@queryBP.get('/<string:query>')
def get(query: str):
    # Strip the query
    query = query.strip()
    print(f"Query is: {query} with a length of {len(query)}")

    # Create the service object
    results = YTAPIdiscovery.build("youtube", "v3", developerKey=api_key)

    # ! Make the request
    request = results.search().list(
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