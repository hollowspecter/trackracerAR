/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
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
