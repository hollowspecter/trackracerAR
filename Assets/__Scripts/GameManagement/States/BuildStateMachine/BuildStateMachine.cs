/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface for <see cref="BuildStateMachine"/>
/// </summary>
public interface IBuildStateMachine
{
    /// <summary>
    /// Gets fired when a touch input was detected.
    /// </summary>
    event State.TouchHandler m_touchDetected;

    /// <summary>
    /// The current session's trackdata.
    /// </summary>
    TrackData CurrentTrackData { get; set; }

    /// <summary>
    /// The current session's feature point offset.
    /// This is important for the <see cref="BuildObserveState"/>
    /// as there are no gameobjects for the feature points.
    /// It's cached here to enable the <see cref="CenterTrackTool"/>.
    /// </summary>
    Vector3 CurrentFeaturePointOffset { get; set; }
}

/// <summary>
/// Overall statemachine for the build process
/// </summary>
public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public TrackData CurrentTrackData { get; set; }
    public Vector3 CurrentFeaturePointOffset { get; set; }

    public event TouchHandler m_touchDetected;

    private IFeaturePointsManager m_trackBuilder;

    #region Di

    [Inject]
    protected void Construct( IFeaturePointsManager _trackBuilder )
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
