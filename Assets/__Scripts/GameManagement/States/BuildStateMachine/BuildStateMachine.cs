/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildStateMachine
{
    // events
    event State.TouchHandler m_touchDetected;
    TrackData CurrentTrackData { get; set; }
    Vector3 CurrentFeaturePointOffset { get; set; }
}

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public TrackData CurrentTrackData { get { return m_trackData; } set { m_trackData = value; } }
    public Vector3 CurrentFeaturePointOffset { get; set; }

    public event TouchHandler m_touchDetected;

    private TrackData m_trackData;
    private ITrackBuilderManager m_trackBuilder;

    #region Di

    [Inject]
    protected void Construct( ITrackBuilderManager _trackBuilder )
    {
        m_trackBuilder = _trackBuilder;
    }

    #endregion

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

#if !UNITY_EDITOR
        TouchInput();
#else
        EditorInput ();
#endif
    }

    public override void EnterState()
    {
        base.EnterState ();
        m_trackBuilder.SetFeaturePointVisibility (true);
    }

    public override void ExitState()
    {
        base.ExitState ();
        m_trackBuilder.SetFeaturePointVisibility (false);
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
