/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using UnityEngine;
using System.Collections;

namespace Baguio.Splines
{
    public static class SplineExtension
    {
        public static float GetSplineLength ( this OrientedPoint [] _path )
        {
            float length = 0f;

            for ( int i = 0; i < _path.Length - 1; i++ )
            {
                length += ( _path [ i + 1 ].position - _path [ i ].position ).magnitude;
            }

            return length;
        }

    }

}
