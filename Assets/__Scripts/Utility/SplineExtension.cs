using UnityEngine;
using System.Collections;

public static class SplineExtension
{
    public static float GetSplineLength ( this OrientedPoint [] _path )
    {
        float length = 0f;

        for ( int i = 0; i < _path.Length - 1; i++ )
        {
            length += ( _path [ i + 1 ].position - _path [ i ].position ).magnitude;
        }

        return length;
    }

}
