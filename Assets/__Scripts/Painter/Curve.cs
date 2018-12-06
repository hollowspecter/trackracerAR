using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve
{
    private const float ALPHA = 0.5f; // set betweeen 0 and 1

    private OrientedPoint [] m_path;

    #region Constructor

    public static Curve CatmulRom ( Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, int _numPoints )
    {
        Curve curve = new Curve ();
        OrientedPoint [] path = new OrientedPoint [ _numPoints + 1 ];

        int index = 0;
        OrientedPoint p = new OrientedPoint ();
        float t0 = 0.0f;
        float t1 = GetT ( t0, _p0, _p1 );
        float t2 = GetT ( t1, _p1, _p2 );
        float t3 = GetT ( t2, _p2, _p3 );

        for ( float t = t1; t < t2; t += ( ( t2 - t1 ) / _numPoints ) )
        {
            Vector3 A1 = ( t1 - t ) / ( t1 - t0 ) * _p0 + ( t - t0 ) / ( t1 - t0 ) * _p1;
            Vector3 A2 = ( t2 - t ) / ( t2 - t1 ) * _p1 + ( t - t1 ) / ( t2 - t1 ) * _p2;
            Vector3 A3 = ( t3 - t ) / ( t3 - t2 ) * _p2 + ( t - t2 ) / ( t3 - t2 ) * _p3;

            Vector3 B1 = ( t2 - t ) / ( t2 - t0 ) * A1 + ( t - t0 ) / ( t2 - t0 ) * A2;
            Vector3 B2 = ( t3 - t ) / ( t3 - t1 ) * A2 + ( t - t1 ) / ( t3 - t1 ) * A3;

            Vector3 C = ( t2 - t ) / ( t2 - t1 ) * B1 + ( t - t1 ) / ( t2 - t1 ) * B2;

            p.position = C;
            path [ index++ ] = p;
        }

        // last point
        p.position = _p2;
        path [ path.Length - 1 ] = p;

        curve.SetPath ( ref path );
        return curve;
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

    #endregion

    #region Debug Functions

    public void DrawGizmos ( Color _color )
    {
        Color origCol = Gizmos.color;
        Gizmos.color = _color;
        for ( int i = 0; i < m_path.Length - 1; ++i )
        {
            Gizmos.DrawLine ( m_path [ i ].position, m_path [ i + 1 ].position );
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
