/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;

/// <summary>
/// Extension for string
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Removes white space from a string
    /// </summary>
    public static string RemoveWhitespace ( this string str )
    {
        return string.Join ( "", str.Split ( default ( string [] ), StringSplitOptions.RemoveEmptyEntries ) );
    }
}
