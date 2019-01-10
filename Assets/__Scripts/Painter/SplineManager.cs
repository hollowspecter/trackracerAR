using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplineManager : UniqueMesh
{
    [SerializeField]
    private TrackData m_trackData;

    public List<OrientedPoint> m_waypoints;

    private Vector3 [] m_points;
    private OrientedPoint [] m_path;
    private Spline m_spline;

    #region Unity Functions

    private void Awake()
    {
        InitSpline ();
        GenerateStreet ();
    }

    #endregion

    #region Public Functions

    #endregion

    #region Private Functions

    private void InitSpline()
    {
        m_points = new Vector3 [ transform.childCount ];
        for ( int i = 0; i < m_points.Length; ++i )
        {
            m_points [ i ] = transform.GetChild ( i ).transform.position;
        }
        m_spline = new Spline ( m_points );
    }

    private void GenerateStreet()
    {
        m_spline.GetOrientedPath ( out m_path );
        GenerateWaypoints ();
        Extruder.Extrude ( mesh, m_trackData.m_shape, m_path, m_trackData.m_scale );
    }

    private void GenerateWaypoints()
    {
        if ( m_path == null || m_path.Length == 0 ) return;
        m_waypoints = new List<OrientedPoint> ();

        // Todo: hardcoding rausnehmen
        for (int i=0; i<m_path.Length; i+=8 )
        {
            m_waypoints.Add ( m_path [ i ] );
        }
    }

    #endregion

    #region Debug Functions

    private void OnDrawGizmos ()
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
        if ( m_spline == null ) InitSpline ();
        m_spline.DrawGizmos ();

        // Draw Shape
        if ( m_trackData.m_shape != null ) m_trackData.m_shape.DrawGizmos ( transform.position );

        // Draw WayPoints
        if (m_waypoints != null)
        {
            foreach(OrientedPoint p in m_waypoints)
            {
                Gizmos.DrawSphere ( p.position, 0.1f );
            }
        }
    }

    #endregion
}
