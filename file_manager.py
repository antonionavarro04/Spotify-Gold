import string
import secrets as sc

def read(file_name: str) -> str:
    """
    Reads the file `file_name` and returns the content

    Parameters
    ----------
    file_name : str
        The file name to read

    Returns
    -------
    str
        The content of the file
    """
    with open(file_name, 'r') as f:
        return f.read()
    
def write(file_name: str, content: str) -> None:
    """
    Writes the content `content` to the file `file_name`
    
    Parameters
    ----------
    file_name : str
        The file name to write to
    content : str
        The content to overwrite

    Returns
    -------
    None
    """
    with open(file_name, 'w') as f:
        f.write(content)

def randomName(length: int) -> str:
    """
    Generates a random name of length `length`

    Parameters
    ----------
    length : int
        Length of the name

    Returns
    -------
    str
        The random name
    """
    return ''.join(sc.choice(string.ascii_lowercase) for i in range(length))
