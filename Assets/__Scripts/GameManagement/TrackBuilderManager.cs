/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ITrackBuilderManager
{
    void InstantiateFeaturePoints( ref Vector3 [] points );
}

public class TrackBuilderManager : ITrackBuilderManager
{
    private Point3DFactory.Factory m_point3DFactory;
    private Transform m_trackTransform;
    private LineRenderer m_line;

    public TrackBuilderManager(Point3DFactory.Factory _point3DFactory,
                               [Inject ( Id = "TrackParent" )] Transform _trackTransform,
                               [Inject ( Id = "TrackParent" )] LineRenderer _line )
    {
        m_point3DFactory = _point3DFactory;
        m_trackTransform = _trackTransform;
        m_line = _line;
    }

    public void InstantiateFeaturePoints(ref Vector3[] points)
    {
        points.ThrowIfNull ( nameof ( points ) );

        Debug.LogFormat ( "Recorded {0} points with the point recorder!", points.Length );
        Vector3 [] featurePoints;
        FeaturePointUtil.IdentifyFeaturePoints ( ref points, out featurePoints );

        // Create the Prefabs with the Factory at the Positions to a certain injected root object
        Transform currentPoint;
        for ( int i = 0; i < featurePoints.Length; ++i )
        {
            currentPoint = m_point3DFactory.Create ( new Point3DFactory.Params () );
            currentPoint.position = featurePoints [ i ];
            currentPoint.parent = m_trackTransform;
        }
    }
}
