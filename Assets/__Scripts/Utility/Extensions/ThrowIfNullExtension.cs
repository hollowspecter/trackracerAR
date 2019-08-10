/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;

/// <summary>
/// Extension class for every class
/// </summary>
public static class ThrowIfNullExtension
{

    /// <summary>
    /// Throw an <see cref="ArgumentNullException"/> if null.
    /// </summary>
    /// <param name="argument">Put in the exception message</param>
    public static void ThrowIfNull<T>( this T value, string argument )
    {
        if ( value == null )
        {
            throw new ArgumentNullException ( argument );
        }
    }
}
