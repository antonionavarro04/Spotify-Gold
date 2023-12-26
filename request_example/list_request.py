import requests as rq
import functions as jm
import json

if __name__ == '__main__':
    query = str(input("Search: "))

    response = rq.get(f"http://localhost:7483/query/list/{query}")

    # Save response.content in raw.txt
    with open("list/raw", "wb") as f:
        f.write(response.content)
    
    decoded_response = jm.decode_json_list(response.content)

    with open("list/response.json", "w", encoding="utf-8") as f:
        json.dump(decoded_response, f, indent=4, ensure_ascii=False)
