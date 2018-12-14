using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplineManager : UniqueMesh
{
    [SerializeField]
    private ShapeData m_shape;
    [SerializeField]
    private Vector2 m_scale;

    private Vector3 [] m_points;
    private Spline m_spline;

    void Start ()
    {
        InitSpline ();
        GenerateStreet ();
    }

    void InitSpline ()
    {
        m_points = new Vector3 [ transform.childCount ];
        for ( int i = 0; i < m_points.Length; ++i )
        {
            m_points [ i ] = transform.GetChild ( i ).transform.position;
        }
        m_spline = new Spline ( m_points );
    }

    void GenerateStreet ()
    {
        OrientedPoint [] path;
        m_spline.GetOrientedPath ( out path );
        Extruder.Extrude ( mesh, m_shape, path, m_scale );
    }

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
        if ( m_shape != null ) m_shape.DrawGizmos ( transform.position );
    }
}
