/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ThrowIfNullExtension
{
    public static void ThrowIfNull<T>( this T value, string argument )
    {
        if ( value == null )
        {
            throw new ArgumentNullException ( argument );
        }
    }
}
