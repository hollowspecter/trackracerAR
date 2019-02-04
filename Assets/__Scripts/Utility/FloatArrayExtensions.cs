/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;

namespace Baguio.Splines
{
    public static class FloatArrayExtensions
    {
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
