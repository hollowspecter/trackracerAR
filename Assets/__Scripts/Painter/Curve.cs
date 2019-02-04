/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baguio.Splines
{
    public class Curve
    {
        public OrientedPoint [] m_path;
        private const float ALPHA = 0.5f; // set betweeen 0 and 1

        #region Constructor

        public static Curve CatmulRom ( Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, int _numPoints )
        {
            Curve curve = new Curve ();
            OrientedPoint [] path = new OrientedPoint [ _numPoints + 1 ];

            int index = 0;

            float t0 = 0.0f;
            float t1 = GetT ( t0, _p0, _p1 );
            float t2 = GetT ( t1, _p1, _p2 );
            float t3 = GetT ( t2, _p2, _p3 );

            for ( float t = t1; t < t2; t += ( ( t2 - t1 ) / _numPoints ) )
            {
                path [ index++ ] = GetOrientedPoint ( t, t0, t1, t2, t3, _p0, _p1, _p2, _p3 );
            }

            // last point TODO maybe erase the last point bc the next section will have it included?
            path [ path.Length - 1 ] = GetOrientedPoint ( t2, t0, t1, t2, t3, _p0, _p1, _p2, _p3 );

            curve.SetPath ( ref path );
            return curve;
        }

        #endregion

        #region Public Functions

        public float GetLength ()
        {
            if ( m_path == null ) return 0f;

            float length = 0f;
            for ( int i = 0; i < m_path.Length - 1; i++ )
            {
                length += ( m_path [ i + 1 ].position - m_path [ i ].position ).magnitude;
            }
            return length;
        }

        #endregion

        #region Helper Functions

        private static float GetT ( float _t, Vector3 _p0, Vector3 _p1 )
        {
            float a = Mathf.Pow ( ( _p1.x - _p0.x ), 2.0f ) + Mathf.Pow ( ( _p1.y - _p0.y ), 2.0f ) + Mathf.Pow ( ( _p1.z - _p0.z ), 2.0f );
            float b = Mathf.Pow ( a, 0.5f );
            float c = Mathf.Pow ( b, ALPHA );

            return ( c + _t );
        }

        private static OrientedPoint GetOrientedPoint ( float t, float t0, float t1, float t2, float t3, Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3 )
        {
            OrientedPoint p = new OrientedPoint ();
            // Cache
            float t0_t = t0 - t;
            float t1_t = t1 - t;
            float t2_t = t2 - t;
            float t3_t = t3 - t;

            float t1_t0 = t1 - t0;
            float t2_t1 = t2 - t1;
            float t3_t2 = t3 - t2;
            float t2_t0 = t2 - t0;
            float t3_t1 = t3 - t1;

            // Calc the Position
            Vector3 A1 = t1_t / t1_t0 * _p0 + -t0_t / t1_t0 * _p1;
            Vector3 A2 = t2_t / t2_t1 * _p1 + -t1_t / t2_t1 * _p2;
            Vector3 A3 = t3_t / t3_t2 * _p2 + -t2_t / t3_t2 * _p3;
            Vector3 B1 = t2_t / t2_t0 * A1 + -t0_t / t2_t0 * A2;
            Vector3 B2 = t3_t / t3_t1 * A2 + -t1_t / t3_t1 * A3;
            Vector3 C = t2_t / t2_t1 * B1 + -t1_t / t2_t1 * B2;

            // Calc the Tangent
            Vector3 A1_ = 1f / t1_t0 * ( _p1 - _p0 );
            Vector3 A2_ = 1f / t2_t1 * ( _p2 - _p1 );
            Vector3 A3_ = 1f / t3_t2 * ( _p3 - _p2 );
            Vector3 B1_ = 1f / t2_t0 * ( A2 - A1 ) + t2_t / t2_t0 * A1_ + ( -t0_t / t2_t0 ) * A2_;
            Vector3 B2_ = 1f / t3_t1 * ( A3 - A2 ) + t3_t / t3_t1 * A2_ + ( -t1_t / t3_t1 ) * A3_;
            Vector3 C_ = 1f / t2_t1 * ( B2 - B1 ) + t2_t / t2_t1 * B1_ + ( -t1_t / t2_t1 ) * B2_;

            // Calc Bi Normal
            Vector3 binormal = Vector3.Cross ( Vector3.up, C_ ).normalized;
            Vector3 normal = Vector3.Cross ( C_, binormal ).normalized;

            // Calc Quaternion
            Quaternion rotation = Quaternion.LookRotation ( C_.normalized, normal );

            p.position = C;
            p.tangent = C_.normalized;
            p.normal = normal;
            p.rotation = rotation;

            return p;
        }

        #endregion

        #region Debug Functions

        public void DrawGizmos ( Color _color )
        {
            Color origCol = Gizmos.color;
            Gizmos.color = _color;
            for ( int i = 0; i < m_path.Length - 1; ++i )
            {
                OrientedPoint point = m_path [ i ];
                Gizmos.DrawLine ( point.position, m_path [ i + 1 ].position );
                if ( Configuration.ShowTangents ) Gizmos.DrawLine ( point.position, point.position + point.tangent );
                if ( Configuration.ShowNormals ) Gizmos.DrawLine ( point.position, point.position + point.normal );
            }
            Gizmos.color = origCol;
        }

        public void DebugPrintPositions ()
        {
            string result = "";
            for ( int i = 0; i < m_path.Length; ++i )
            {
                result += "i=" + i + ": " + m_path [ i ].position.ToString () + "\n";
            }
            Debug.Log ( result );
        }

        #endregion

        #region Getter and Setter

        private void SetPath ( ref OrientedPoint [] _path )
        {
            m_path = _path;
        }

        #endregion
    }

}
