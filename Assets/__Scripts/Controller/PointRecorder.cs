/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PointRecorder
{
    public int PointCount { get { return m_points.Count; } }

    protected Settings m_settings;
    protected Transform m_transform;
    protected LineRenderer m_line;
    protected List<Vector3> m_points;

    private Vector3 m_lastPosition;
    private float m_minSqrDistance;

    public PointRecorder( [Inject] Settings _settings,
                         [Inject ( Id = "ARCamera" )] Transform _cameraTransform,
                         [Inject ( Id = "TrackParent" )] LineRenderer _line )
    {
        m_settings = _settings;
        m_transform = _cameraTransform;
        m_points = new List<Vector3> ();
        m_line = _line;
        m_lastPosition = m_transform.position;
        m_minSqrDistance = m_settings.MinDistance * m_settings.MinDistance;

        _line.positionCount = 0;
    }

    #region Public Functions

    public void RecordPoint()
    {
        Vector3 newPosition = m_transform.position + m_transform.forward * m_settings.zOffset;
        float distance = ( newPosition - m_lastPosition ).sqrMagnitude;
        if ( distance > m_minSqrDistance )
        {
            m_points.Add ( newPosition );
            m_lastPosition = newPosition;
            m_line?.SetPosition ( m_line.positionCount++, newPosition );
        }
    }

    public void DumpPoints( out Vector3 [] _points)
    {
        _points = m_points.ToArray ();
        m_points.Clear ();
    }

    public void ClearPoints()
    {
        m_points.Clear ();
        if (m_line) {
            m_line.positionCount = 0;
        }
    }

    #endregion

    #region Settings

    [System.Serializable]
    public class Settings
    {
        public float MinDistance = 0.01f;
        public KeyCode DebugKeyCode = KeyCode.P;
        public float zOffset = 0.05f;
    }

    #endregion

    #region Factory

    public class Factory : PlaceholderFactory<PointRecorder> { }

    #endregion
}
