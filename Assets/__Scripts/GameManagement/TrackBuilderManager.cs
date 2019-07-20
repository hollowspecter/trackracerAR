/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
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
    void UpdateFeaturePoints( ref Vector3 [] _featurePoints );
}

public class TrackBuilderManager : ITrackBuilderManager, IInitializable, IDisposable
{
    private Point3DFactory.Factory m_point3DFactory;
    private Transform m_trackTransform;
    private LineRenderer m_line;
    private SignalBus m_signalBus;

    private List<GameObject> m_pointGOs;
    private List<Point3DView> m_pointViews;

    public TrackBuilderManager(Point3DFactory.Factory _point3DFactory,
                               [Inject ( Id = "TrackParent" )] Transform _trackTransform,
                               [Inject ( Id = "TrackParent" )] LineRenderer _line,
                               SignalBus _signalBus)
    {
        m_point3DFactory = _point3DFactory;
        m_trackTransform = _trackTransform;
        m_line = _line;
        m_pointGOs = new List<GameObject> ();
        m_pointViews = new List<Point3DView> ();
        m_signalBus = _signalBus;
    }

    public void Initialize()
    {
        m_signalBus.Subscribe<FeaturePointChanged> ( FeaturePointChanged );
    }

    public void Dispose()
    {
        m_signalBus.Unsubscribe<FeaturePointChanged> ( FeaturePointChanged );
    }

    public void ClearFeaturePoints()
    {
        foreach ( GameObject obj in m_pointGOs )
        {
            UnityEngine.Object.Destroy ( obj );
        }
        m_pointGOs.Clear ();
        m_pointViews.Clear ();
        m_line.positionCount = 0;
    }

    public void InstantiateFeaturePoints(ref Vector3[] featurePoints)
    {
        featurePoints.ThrowIfNull ( nameof ( featurePoints ) );

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
            m_pointViews.Add (currentPoint.GetComponent<Point3DView> ());
        }
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

    /// <summary>
    /// Updates the featurepoints positions. if the array sizes
    /// don't match (so featurepoints got added or removed),
    /// all featurepoints will be cleared and instantiated newly.
    /// </summary>
    /// <param name="_featurePoints">Do nothing if param is null!</param>
    public void UpdateFeaturePoints( ref Vector3 [] _featurePoints )
    {
        if (_featurePoints == null) {
            return;
        }

        // instantiate new if the array lengths don't match
        if ( _featurePoints.Length != m_pointGOs.Count) {
            InstantiateFeaturePoints (ref _featurePoints);
        } 
        // else, just update the positions
        else {
            for (int i=0; i<_featurePoints.Length; ++i ) {
                m_pointGOs [i].transform.position = _featurePoints [i];
            }
        }

        FeaturePointChanged ();
    }

    private void FeaturePointChanged()
    {
        for (int i=0; i<m_pointViews.Count; ++i) {
            if (m_pointViews[i].IsDirty) {
                if ( m_pointViews.Count > 3 ) {
                    UnityEngine.Object.Destroy (m_pointGOs [i]);
                    m_pointViews.RemoveAt (i);
                    m_pointGOs.RemoveAt (i);
                } else {
                    m_pointViews [i].IsDirty = false;
                }
                break;
            } else if (m_pointViews[i].IsCopied) {
                m_pointViews [i].IsCopied = false;
                Transform newPoint = m_point3DFactory.Create (new Point3DFactory.Params ());
                newPoint.parent = m_trackTransform;
                newPoint.SetSiblingIndex (i+1);
                newPoint.position = m_pointGOs [i].transform.position + Vector3.up * 0.05f;
                m_pointGOs.Insert (i+1, newPoint.gameObject);
                m_pointViews.Insert (i+1, newPoint.GetComponent<Point3DView> ());
                break;
            }
        }

        m_line.positionCount = m_pointGOs.Count;
        for (int i=0; i<m_pointGOs.Count;++i )
        {
            m_line.SetPosition ( i, m_pointGOs [ i ].transform.position );
        }
    }
}
