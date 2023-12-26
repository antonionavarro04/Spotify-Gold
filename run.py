from API import server

def deleteFiles() -> None:
    import os
    import glob

    # Get all the files
    files = glob.glob("*.mp3")

    # Delete them
    for file in files:
        os.remove(file)

if __name__ == '__main__':
    # Delete *.mp3 files
    deleteFiles()

    server.run(host="0.0.0.0", port=7483, debug=True)
