///<summary>
///<copyright>(c) Vivien Baguio</copyright>
///http://www.vivienbaguio.com
///</summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;

    public OrientedPoint ( Vector3 position, Quaternion rotation )
    {
        this.position = position;
        this.rotation = rotation;
    }

    public Vector3 LocalToWorld ( Vector3 point )
    {
        return position + rotation * point;
    }

    public Vector3 WorldToLocal ( Vector3 point )
    {
        return Quaternion.Inverse ( rotation ) * ( point - position );
    }

    public Vector3 LocalToWorldDirection ( Vector3 dir )
    {
        return rotation * dir;
    }
}

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
public class Bezier
{
    #region variables (private)

    public Vector3 [] m_points;

    #endregion

    #region Properties (public)

    public Vector3 p0 { get { return m_points [ 0 ]; } }

    #endregion

    #region Methods
    public OrientedPoint [] GetBezierPath ( int _numOfSegments )
    {
        OrientedPoint [] path = new OrientedPoint [ _numOfSegments + 1 ];
        float quotient = 1f / ( float ) _numOfSegments;

        for ( int i = 0; i < _numOfSegments + 1; i++ )
        {
            path [ i ] = new OrientedPoint ();
            path [ i ].position = GetPoint ( m_points, i * quotient );
            path [ i ].rotation = GetOrientation3D ( m_points, i * quotient, Vector3.up );
        }

        return path;
    }

    public float GetLength ()
    {
        int precision = 128;
        float quotient = 1f / ( float ) precision;
        float length = 0;

        for ( int i = 0; i < precision + 1; i += 2 )
        {
            Vector3 p1 = GetPoint ( m_points, i * quotient );
            Vector3 p2 = GetPoint ( m_points, ( i + 1 ) * quotient );
            length += ( p2 - p1 ).magnitude;
        }

        return length;
    }

    public static float GetLength ( OrientedPoint [] _path )
    {
        float length = 0f;

        for ( int i = 0; i < _path.Length - 1; i++ )
        {
            length += ( _path [ i + 1 ].position - _path [ i ].position ).magnitude;
        }

        return length;
    }

    Vector3 GetPoint ( Vector3 [] _points, float t )
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return _points [ 0 ] * ( omt2 * omt ) +
               _points [ 1 ] * ( 3f * omt2 * t ) +
               _points [ 2 ] * ( 3f * omt * t2 ) +
               _points [ 3 ] * ( t2 * t );
    }

    Vector3 GetTangent ( Vector3 [] _points, float t )
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
            _points [ 0 ] * ( -omt2 ) +
            _points [ 1 ] * ( 3f * omt2 - 2 * omt ) +
            _points [ 2 ] * ( -3f * t2 + 2 * t ) +
            _points [ 3 ] * ( t2 );
        return tangent.normalized;
    }

    Vector3 GetNormal2D ( Vector3 [] _points, float t )
    {
        Vector3 tng = GetTangent ( _points, t );
        return new Vector3 ( -tng.y, tng.x, 0f ); // rotated by 90 degrees
    }

    Vector3 GetNormal3D ( Vector3 [] _points, float t, Vector3 up )
    {
        Vector3 tng = GetTangent ( _points, t );
        Vector3 binormal = Vector3.Cross ( up, tng ).normalized;
        return Vector3.Cross ( tng, binormal );
    }

    Quaternion GetOrientation2D ( Vector3 [] _points, float t )
    {
        Vector3 tng = GetTangent ( _points, t );
        Vector3 nrm = GetNormal2D ( _points, t );
        return Quaternion.LookRotation ( tng, nrm );
    }

    Quaternion GetOrientation3D ( Vector3 [] _points, float t, Vector3 up )
    {
        Vector3 tng = GetTangent ( _points, t );
        Vector3 nrm = GetNormal3D ( _points, t, up );
        return Quaternion.LookRotation ( tng, nrm );
    }
    #endregion
}