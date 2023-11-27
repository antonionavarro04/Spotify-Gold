from pytube import YouTube

# Download only audio
url = 'https://www.youtube.com/watch?v=QU1pPzEGrqw'
yt = YouTube(url)
vid = yt.streams.filter(only_audio=True).first()
vid.download()
print('Downloaded')
