using System;

public static class StringExtension
{
    public static string RemoveWhitespace ( this string str )
    {
        return string.Join ( "", str.Split ( default ( string [] ), StringSplitOptions.RemoveEmptyEntries ) );
    }
}
