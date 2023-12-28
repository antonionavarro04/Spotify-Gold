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
