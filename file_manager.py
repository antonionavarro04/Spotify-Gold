import string
import secrets as sc

def read(file_name: str):
    with open(file_name, 'r') as f:
        return f.read()
    
def write(file_name: str, content: str):
    with open(file_name, 'w') as f:
        f.write(content)

def randomName(extension: str):
    return f"{''.join(sc.choice(string.ascii_letters) for i in range(10))}.{extension}"
