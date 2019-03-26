/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PointRecorder : MonoBehaviour
{
    [SerializeField]
    protected float m_minDistance = 0.01f;
    [SerializeField]
    protected LineRenderer m_line;
    [SerializeField]
    protected KeyCode m_debugKeyCode = KeyCode.P;

    protected List<Vector3> m_points;

    private Vector3 m_lastPosition;
    private float m_minSqrDistance;

    #region Unity Lifecycle

    protected void Awake()
    {
        m_points = new List<Vector3> ();
        m_lastPosition = transform.position;
        m_minSqrDistance = m_minDistance * m_minDistance;
    }

    protected void Update()
    {
        if ( Input.touchCount > 0 ||
            Input.GetKey(m_debugKeyCode))
        {
            Vector3 newPosition = transform.position;
            float distance = ( newPosition - m_lastPosition ).sqrMagnitude;
            if ( distance > m_minSqrDistance )
            {
                m_points.Add ( newPosition );
                m_lastPosition = newPosition;
                m_line?.SetPosition ( m_line.positionCount++, newPosition );
            }
        }
    }

    #endregion

    #region Public Functions

    public void DumpPoints(out Vector3[] _points )
    {
        _points = m_points.ToArray ();
        m_points.Clear ();
    }

    #endregion
}
