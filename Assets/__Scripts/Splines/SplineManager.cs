/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Baguio.Splines
{
    /// <summary>
    /// Interface for <see cref="SplineManager"/>
    /// </summary>
    public interface ISplineManager
    {
        /// <summary>
        /// Property, if the track is currently closed.
        /// </summary>
        bool ClosedTrack { get; }

        /// <summary>
        /// Generates the track using the feature point gameobjects
        /// </summary>
        void GenerateTrack();

        /// <summary>
        /// Generates the track using the feature points from the
        /// <see cref="TrackData"/> session model
        /// </summary>
        void GenerateTrackFromTrackData();

        /// <summary>
        /// Returns the waypoints for the vehicles steering
        /// </summary>
        List<OrientedPoint> GetWaypoints();

        /// <summary>
        /// Clears the mesh
        /// </summary>
        void ClearMesh();
    }

    /// <summary>
    /// The SplineManager is the interface between the application
    /// and the spline and 
    /// </summary>
    [ExecuteInEditMode]
    public class SplineManager : UniqueMesh, ISplineManager
    {
        public bool ClosedTrack { get { return m_trackData.m_closed; } }
        public MeshRenderer m_meshRenderer;

        [SerializeField]
        protected TrackData m_trackData;

        protected Vector3 [] m_points;
        protected OrientedPoint [] m_path;
        protected List<OrientedPoint> m_waypoints;
        protected IBuildStateMachine m_session;

        private Spline m_spline;
        
        #region Di

        [Inject]
        private void Construct( [InjectOptional] IBuildStateMachine _session )
        {
            m_session = _session;
            m_meshRenderer = GetComponent<MeshRenderer> ();
        }

        #endregion

        #region Public Functions

        public void GenerateTrackFromTrackData()
        {
            // update trackdata with the data from current session
            if ( m_session != null ) m_trackData = m_session.CurrentTrackData;

            m_points = new Vector3 [m_trackData.m_featurePoints.Length];

            // add the feature point offset for the center-track-tool
            for (int i=0; i<m_points.Length;++i ) {
                m_points[i] = m_trackData.m_featurePoints[i] + m_session.CurrentFeaturePointOffset;
            }

            InitPath ();
            GenerateWaypoints ();
            GenerateMesh (mMesh);
        }

        public virtual void GenerateTrack ()
        {
            if ( m_session != null ) m_trackData = m_session.CurrentTrackData;

            InitPointsFromChildTransforms ();
            InitPath ();
            GenerateWaypoints ();
            GenerateMesh (mMesh);
        }

        public virtual List<OrientedPoint> GetWaypoints ()
        {
            if (m_waypoints == null || m_waypoints.Count <= 0 )
            {
                GenerateTrack ();
            }

            return m_waypoints;
        }

        public virtual void ClearMesh()
        {
            mMesh.Clear ();
        }

        #endregion

        #region Protected Functions

        protected virtual void InitPointsFromChildTransforms()
        {
            m_points = new Vector3 [ transform.childCount ];
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
            m_meshRenderer.material = m_trackData.Material;
            Extruder.Extrude ( _mesh, m_trackData.Shape, m_path, m_trackData.m_scale );
        }

        protected virtual void GenerateWaypoints ()
        {
            if ( m_path == null || m_path.Length == 0 )
            {
                Debug.LogWarning ( "The Path was either null or empty!" );
                return;
            }
            m_waypoints = new List<OrientedPoint> ();

            int stepSize = Configuration.WaypointStepSize;
            for ( int i = 0; i < m_path.Length; i += stepSize )
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
            if ( m_trackData.Shape != null ) m_trackData.Shape.DrawGizmos ( transform.position );

            // Draw WayPoints
            if ( m_waypoints != null )
            {
                foreach ( OrientedPoint p in m_waypoints )
                {
                    Gizmos.DrawSphere ( p.position, 0.01f );
                }
            }
        }

        #endregion
    }
}
