using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplineManager : MonoBehaviour
{
    private Vector3 [] m_points;
    private Spline m_spline;

    void Start ()
    {
        InitSpline ();
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
    }


}
