/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Class to calculate the feature points of a curve,
/// represented as a point set.
/// </summary>
public class FeaturePointUtility
{
    /// <summary>
    /// Takes in an array of points and returns
    /// the characteristic points of the point set.
    /// </summary>
    /// <param name="_p">The point list</param>
    /// <param name="_featureP">The returned characteristic points</param>
    public static void IdentifyFeaturePoints( ref Vector3 [] _p, out Vector3 [] _featureP)
    {
        // check for null
        if ( _p == null || _p.Length < 1 )
        {
            Debug.LogWarning ( "The Points were null or empty, therefore no FeaturePoints were detected." );
            _featureP = new Vector3 [0];
            return;
        }

        using ( var block = DisposeBlock.Spawn() )
        {
            var result = block.Spawn ( ListPool<Vector3>.Instance );

            Vector3 lastCharPoint = _p [ 0 ];
            result.Add ( lastCharPoint );

            /* Step 1: Find the characteristic points */

            for ( int i = 1; i < _p.Length - 1; ++i )
            {
                // Pass the minimal distance test?
                float distance = Vector3.Distance ( _p [ i ], lastCharPoint );
                if ( distance < Configuration.DistanceThreshold )
                    continue;

                // Pass the angle test?
                Vector3 segment1 = _p [ i ] - lastCharPoint;
                Vector3 segment2 = _p [ i + 1 ] - _p [ i ];
                float angle = Vector3.Angle ( segment1.normalized, segment2.normalized );
                if ( angle < Configuration.AngleThreshold )
                    continue;

                lastCharPoint = _p [ i ];
                result.Add ( lastCharPoint );
            }

            // fix the last point
            Vector3 lastPoint = _p [ _p.Length - 1 ];

            // is it already the last point? you're good! Else...
            if ( lastCharPoint != lastPoint )
            {
                // is the last added point too close? remove the lastCharPoint and replace by last point
                float dist = Vector3.Distance ( lastCharPoint, lastPoint );
                if ( dist < Configuration.DistanceThreshold )
                    result [ result.Count - 1 ] = lastPoint;
                else
                    result.Add ( lastPoint );
            }

            // Last Step: Assign the result
            _featureP = result.ToArray ();
        }
    }
}
