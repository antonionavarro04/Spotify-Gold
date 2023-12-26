import requests as rq
import json
import functions as jm

if __name__ == "__main__":
    query = str(input("Search: "))

    response = rq.get(f"http://localhost:7483/query/{query}")

    # Get the json in the header
    json_str: str = response.headers.get("json")

    # Save it in raw
    with open("song/raw", "w", encoding="utf-8") as f:
        f.write(json_str)

    # Decode the json
    json_con: dict = jm.decode_json(json_str)

    # Save it in test.json
    with open("song/response.json", "w", encoding="utf-8") as f:
        json.dump(json_con, f, indent=4, ensure_ascii=False)

    # Save the content in test.mp3
    with open("song/response.mp3", "wb") as f:
        f.write(response.content)
