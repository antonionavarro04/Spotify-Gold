# SPOTIFY GOLD API Repository
Author: Antonio Navarro | 2ºDAM IES Nervión

## General Information
This repository has the source code of the whole API of Spotify Gold Repository. It uses Flask to handle the requests comings from multiple devices.

## Installation
Because this is a source code and it will be so on, you'll need to have python installed on your desktop. I've used Python 3.9 during development and tests. Once you've clones the repository you have to go to the root directory of the project and execute
```bash 
pip install -r requirements.txt
```
this will install all required pip dependencies for this server to work properly.

To run the server just execute 
```bash
python run.py`
``

## Usage
The server/API has a static web in the root directory that allows to use it more convenient, just open a browser and go to localhost and the port of the server (127.0.0.1:7483) then you have two text inputs. One that gets a mp3 from a YouTube share link (the one with youtu.be) and the other one that gets a mp3 from the video id (the one next to the v= parameter in the url) It'll process the information and return an mp3 in the maximum quality possible (320kbps, 128kbps or 64kbps).
API can raise some errors although I want to control them just catching the exception and returning an html with the proper info.

You can also request a mp3 using a http get requests with the following url. http://127.0.0.0.1:7483/music/<id>

### Technical issues I've encountered
1. This types of libraries of pip are easy to use and ready to go, with just a few lines of code you can make big things to work out.
2. Youtube V3 API is limited to just 10.000 units per day, this will make the service to stop working after 100 queries a day. In fact the day I'll create the Android App I won't implement the typical search that returns a list for each character you type, I'll just implement a search that returns a list of 50 songs when pressing a button cause it doesn't matter the number of results, just the number of queries.

### Next features
I want to implement a get response that return a JSON that return a list of songs with a term of search. In that manner it'll work just as YouTube. This will also be useful for the use of this API in my Kotlin App