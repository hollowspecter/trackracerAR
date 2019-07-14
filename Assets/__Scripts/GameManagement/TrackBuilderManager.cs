/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public interface ITrackBuilderManager
{
    void InstantiateFeaturePoints( ref Vector3 [] points );
    Vector3 [] GetFeaturePoints();
    void ClearFeaturePoints();
    void SetFeaturePointVisibility(bool _visible);
}

public class TrackBuilderManager : ITrackBuilderManager, IInitializable, IDisposable
{
    private Point3DFactory.Factory m_point3DFactory;
    private Transform m_trackTransform;
    private LineRenderer m_line;
    private SignalBus m_signalBus;

    private List<GameObject> m_pointGOs;

    public TrackBuilderManager(Point3DFactory.Factory _point3DFactory,
                               [Inject ( Id = "TrackParent" )] Transform _trackTransform,
                               [Inject ( Id = "TrackParent" )] LineRenderer _line,
                               SignalBus _signalBus)
    {
        m_point3DFactory = _point3DFactory;
        m_trackTransform = _trackTransform;
        m_line = _line;
        m_pointGOs = new List<GameObject> ();
        m_signalBus = _signalBus;
    }

    public void Initialize()
    {
        m_signalBus.Subscribe<FeaturePointMovedSignal> ( FeaturePointMoved );
    }

    public void Dispose()
    {
        m_signalBus.Unsubscribe<FeaturePointMovedSignal> ( FeaturePointMoved );
    }

    public void ClearFeaturePoints()
    {
        foreach ( GameObject obj in m_pointGOs )
        {
            UnityEngine.Object.Destroy ( obj );
        }
        m_pointGOs.Clear ();
        m_line.positionCount = 0;
    }

    public void InstantiateFeaturePoints(ref Vector3[] featurePoints)
    {
        featurePoints.ThrowIfNull ( nameof ( featurePoints ) );
        Debug.LogFormat ( "TrackBuilderManager: Instantiating {0} feature points", featurePoints.Length );

        // Destroy all previous feature points if there are any
        ClearFeaturePoints ();

        // Create the Prefabs with the Factory at the Positions to a certain injected root object
        Transform currentPoint;
        m_line.positionCount = featurePoints.Length;
        for ( int i = 0; i < featurePoints.Length; ++i )
        {
            currentPoint = m_point3DFactory.Create ( new Point3DFactory.Params () );
            currentPoint.position = featurePoints [ i ];
            currentPoint.parent = m_trackTransform;
            m_line.SetPosition ( i, featurePoints [ i ] );
            m_pointGOs.Add ( currentPoint.gameObject );
        }
        Debug.LogFormat ( "TrackBuilderManager: managing {0} pointGOs!", m_pointGOs.Count );
    }

    public void SetFeaturePointVisibility( bool _visible )
    {
        foreach ( GameObject obj in m_pointGOs ) {
            obj.SetActive (_visible);
        }
        m_line.positionCount = (_visible) ? m_pointGOs.Count : 0;
    }

    public Vector3[] GetFeaturePoints()
    {
        Vector3 [] featurePoints = new Vector3[ m_pointGOs.Count ];
        for (int i=0; i<m_pointGOs.Count;++i )
        {
            featurePoints [ i ] = m_pointGOs [ i ].transform.position;
        }
        Debug.LogFormat ( "TrackBuilderManager/GetFeaturePoints found {0} points", featurePoints.Length );
        return featurePoints;
    }

    private void FeaturePointMoved()
    {
        for (int i=0; i<m_pointGOs.Count;++i )
        {
            m_line.SetPosition ( i, m_pointGOs [ i ].transform.position );
        }
    }
}
