/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using UnityEngine;
using System.Collections.Generic;

namespace Baguio.Splines
{
    /// <summary>
    /// A Spline consists of several <see cref="Curve"/>s.
    /// </summary>
    public class Spline
    {
        private Vector3 [] m_points;
        private int m_pointsPerCurve;
        private Curve [] m_curves;

        #region Constructor

        /// <summary>
        /// Generate an new spline through the given points.
        /// </summary>
        /// <param name="_points">The points the spline will pass through</param>
        /// <param name="closed">Determins whether the spline is closed or not</param>
        /// <param name="_precision">2 to the power of n - times points will be calculated on one curve</param>
        public Spline ( Vector3 [] _points, bool closed = true, int _precision = 5 )
        {
            // check for traps
            if ( _points == null ) throw new System.ArgumentNullException ( "_points" );
            if ( _points.Length < 2 ) throw new System.ArgumentException ( "Array must contain at least 2 points.", "_points" );
            if ( _precision < 1 ) _precision = 2; // minimum precision results in 4 points

            // init member
            m_pointsPerCurve = 2 << _precision;

            m_points = new Vector3 [ _points.Length ];
            for ( int i = 0; i < _points.Length; ++i )
            {
                m_points [ i ] = _points [ i ];
            }

            if ( closed ) CalculateClosedSpline ();
            else CalculateSpline ();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the length of the spline by accumulating
        /// the lengths of the curves.
        /// </summary>
        public float GetLength()
        {
            if ( m_curves == null ) return 0f;

            float length = 0f;

            for ( int i = 0; i < m_curves.Length; ++i ) {
                length += m_curves [i].GetLength ();
            }

            return length;
        }

        /// <summary>
        /// Returns the complete path of the spline.
        /// </summary>
        public void GetOrientedPath( out OrientedPoint [] _path )
        {
            List<OrientedPoint> pathList = new List<OrientedPoint> ();

            for ( int i = 0; i < m_curves.Length; ++i ) {
                pathList.AddRange (m_curves [i].m_path);
            }

            _path = pathList.ToArray ();
        }

        #endregion

        #region Private Methods

        private void CalculateSpline ()
        {
            m_curves = new Curve [ m_points.Length - 1 ];
            Vector3 p0, p1, p2, p3; // line will be drawn between p1 and p2
            for ( int i = 0; i < m_points.Length - 1; ++i )
            {
                p1 = m_points [ i ];
                p2 = m_points [ i + 1 ];

                // first curve exception
                if ( i == 0 )
                    p0 = p1 + ( p1 - p2 );
                else
                    p0 = m_points [ i - 1 ];

                // last curve exception
                if ( i == m_points.Length - 2 )
                    p3 = p2 + ( p2 - p1 );
                else
                    p3 = m_points [ i + 2 ];

                // generate curve
                m_curves [ i ] = Curve.CatmulRom ( p0, p1, p2, p3, m_pointsPerCurve );
            }
        }

        private void CalculateClosedSpline ()
        {
            //// check if last and first point are on the same position
            Vector3 [] points = m_points;
            m_curves = new Curve [ points.Length ];

            // generate spline
            Vector3 p0, p1, p2, p3;
            for ( int i = 0; i < points.Length; ++i )
            {
                p1 = points [ i ];
                p2 = points [ ( i + 1 ) % points.Length ];

                // first curve exception
                if ( i == 0 )
                    p0 = points [ points.Length - 1 ];
                else
                    p0 = points [ ( i - 1 ) % points.Length ];

                // last curve exception
                if ( i == points.Length - 2 )
                    p3 = points [ 0 ];
                else
                    p3 = points [ ( i + 2 ) % points.Length ];

                // generate curve
                m_curves [ i ] = Curve.CatmulRom ( p0, p1, p2, p3, m_pointsPerCurve );
            }
        }

        #endregion

        #region Debug Methods

        public void DrawGizmos ()
        {
            Color c = Color.white;
            for ( int i = 0; i < m_curves.Length; ++i )
            {
                switch ( i )
                {
                    case 0: c = Color.red; break;
                    case 1: c = Color.yellow; break;
                    case 2: c = Color.green; break;
                    case 3: c = Color.blue; break;
                    case 4: c = Color.magenta; break;
                    case 5: c = Color.cyan; break;
                }

                m_curves [ i ].DrawGizmos ( c );
            }
        }

        #endregion

    }

}
