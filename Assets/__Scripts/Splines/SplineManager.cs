/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Baguio.Splines
{
    public interface ISplineManager
    {
        void GenerateTrack();
        List<OrientedPoint> GetWaypoints();
    }

    [ExecuteInEditMode]
    public class SplineManager : UniqueMesh, ISplineManager
    {
        [SerializeField]
        protected TrackData m_trackData;

        protected Vector3 [] m_points;
        protected OrientedPoint [] m_path;
        protected List<OrientedPoint> m_waypoints;
        protected IBuildStateMachine m_session;

        private Spline m_spline;

        #region Di

        [Inject]
        private void Construct( IBuildStateMachine _session )
        {
            m_session = _session;
        }

        #endregion

        #region Public Functions

        public virtual void GenerateTrack ()
        {
            if ( m_session != null ) m_trackData = m_session.CurrentTrackData;

            InitPoints ();
            InitPath ();
            GenerateWaypoints ();
            GenerateMesh (mesh);
        }

        public virtual List<OrientedPoint> GetWaypoints ()
        {
            if (m_waypoints == null || m_waypoints.Count <= 0 )
            {
                GenerateTrack ();
            }

            return m_waypoints;
        }

        #endregion

        #region Protected Functions

        protected virtual void InitPoints()
        {
            m_points = new Vector3 [ transform.childCount ];
            Debug.LogFormat ( "SplineManager: found {0} points as children", m_points.Length );
            for ( int i = 0; i < m_points.Length; ++i )
            {
                m_points [ i ] = transform.GetChild ( i ).transform.position;
            }
        }

        protected virtual void InitPath ()
        {
            m_spline = new Spline ( m_points, m_trackData.m_closed, m_trackData.m_precision );
            m_spline.GetOrientedPath ( out m_path );
        }

        protected virtual void GenerateMesh (Mesh _mesh)
        {
            Extruder.Extrude ( _mesh, m_trackData.m_shape, m_path, m_trackData.m_scale );
        }

        protected virtual void GenerateWaypoints ()
        {
            if ( m_path == null || m_path.Length == 0 )
            {
                Debug.LogWarning ( "The Path was either null or empty!" );
                return;
            }
            m_waypoints = new List<OrientedPoint> ();

            // Todo: hardcoding rausnehmen
            for ( int i = 0; i < m_path.Length; i += 8 )
            {
                m_waypoints.Add ( m_path [ i ] );
            }
        }

        #endregion

        #region Debug Functions

        protected virtual void OnDrawGizmos ()
        {
            // Draw Lines between points
            if ( m_points != null && m_points.Length >= 2 )
            {
                for ( int i = 0; i < m_points.Length - 1; ++i )
                {
                    Gizmos.DrawLine ( m_points [ i ], m_points [ i + 1 ] );
                }
            }

            // Draw Spline
            if ( m_spline == null ) return;
            m_spline.DrawGizmos ();

            // Draw Shape
            if ( m_trackData.m_shape != null ) m_trackData.m_shape.DrawGizmos ( transform.position );

            // Draw WayPoints
            if ( m_waypoints != null )
            {
                foreach ( OrientedPoint p in m_waypoints )
                {
                    Gizmos.DrawSphere ( p.position, 0.1f );
                }
            }
        }

        #endregion
    }
}