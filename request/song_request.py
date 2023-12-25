import requests as rq
import json

if __name__ == "__main__":
    response = rq.get("http://localhost:7483/query/空腹 まふまふ")

    # Main content is the song, json is in the header
    print(response.headers)

    json_string = response.headers["json"].replace("'", '"')
    print(json.loads(json_string))

    with open("test.mp3", "wb") as f:
        f.write(response.content)

    with open("test.json", "w") as f:
        f.write(json_string)
