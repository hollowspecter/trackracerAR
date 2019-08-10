/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

namespace Baguio.Splines
{
    /// <summary>
    /// Extension functions for float arrays
    /// </summary>
    public static class FloatArrayExtension
    {

        /// <summary>
        /// Samples a (preferrably) sorted and evenly spaced out float array with a t value
        /// between 0 and 1. Lerps between to elements if t sits between them.
        /// </summary>
        /// <returns></returns>
        public static float Sample ( this float [] _array, float t )
        {
            int count = _array.Length;
            if ( count == 0 )
                throw new System.Exception ( "Unable to sample array - it has no elements" );
            if ( count == 1 ) return _array [ 0 ];

            float iFloat = t * ( count - 1 );
            int idLower = Mathf.FloorToInt ( iFloat );
            int idUpper = Mathf.FloorToInt ( iFloat + 1 );
            if ( idUpper >= count ) return _array [ count - 1 ];
            if ( idLower < 0 ) return _array [ 0 ];

            return Mathf.Lerp ( _array [ idLower ], _array [ idUpper ], iFloat - idLower );
        }
    }
}