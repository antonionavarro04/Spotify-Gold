def decode_unicode(data: str) -> str:
        import codecs
        """
        Decode the unicode characters in the string

        Parameters
        ----------
        data : str
            The string to decode

        Returns
        -------
        str
            The decoded string
        """

        return codecs.decode(data, 'unicode_escape')

def decode_json(json_str: str) -> dict:
    import json
    import html
    """
    Decode the json string from the header, this is because it contains characters like \u0026, unreadable for the json module

    Parameters
    ----------
    json_str : str
        The json string to decode

    Returns
    -------
    dict
        The decoded json
    """
        
    # Delete the first 2 characters and the last one
    json_str = json_str[2:-1]

    # Load JSON data
    json_con = json.loads(json_str)

    # Decode Unicode characters in the parsed data
    json_con['title'] = decode_unicode(json_con['title'])
    json_con['channel'] = decode_unicode(json_con['channel'])

    # Reverse the .escape function
    json_con['title'] = html.unescape(json_con['title'])
    json_con['channel'] = html.unescape(json_con['channel'])

    return json_con

def decode_json_list(json_str: str) -> list:
    import json
    """
    Decode the json string from the header, this is because it contains characters like \u0026, unreadable for the json module

    Parameters
    ----------
    json_str : str
        The json string to decode

    Returns
    -------
    dict
        The decoded json
    """
        
    # Load JSON data
    json_con = json.loads(json_str)

    return json_con
