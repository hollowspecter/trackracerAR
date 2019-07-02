/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine
{
    // events
    event State.TouchHandler m_touchDetected;
    TrackData CurrentTrackData { get; set; }
}

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public TrackData CurrentTrackData { get { return m_trackData; } set { m_trackData = value; } }

    public event TouchHandler m_touchDetected;

    //public event TouchHandler m_touchPressed;
    //public event TouchHandler m_touchReleased;
    //public event TouchHandler m_touch;

    private TrackData m_trackData;

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

#if !UNITY_EDITOR
        TouchInput();
#else
        EditorInput ();
#endif
    }

    private void TouchInput()
    {
        if ( Input.touchCount < 1)
        {
            return;
        }
        m_touchDetected?.Invoke ( Input.mousePosition.x, Input.mousePosition.y );
    }

    private void EditorInput()
    {
        if ( !Input.GetMouseButtonDown ( 0 ) )
        {
            return;
        }
        m_touchDetected?.Invoke ( Input.mousePosition.x, Input.mousePosition.y );
    }
}
