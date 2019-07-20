/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARPaintBrush : MonoBehaviour
{
    public LineRenderer m_line;
    public float m_minDistance = 0.05f;
    public Text m_debugText;

    private Vector3 m_lastPosition;

    private void Awake ()
    {
        m_lastPosition = transform.position;
    }

    private void Update ()
    {
        if ( Input.touchCount > 0 )
        {
            Vector3 newPosition = transform.position;
            float distance = ( newPosition - m_lastPosition ).magnitude;
            if ( distance > m_minDistance )
            {
                m_line.SetPosition ( m_line.positionCount++, newPosition );
                m_lastPosition = newPosition;
            }
        }

        m_debugText.text = "PositionCount: " + m_line.positionCount;
    }
}
