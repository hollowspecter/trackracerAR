/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface for <see cref="BuildPaintState"/>
/// </summary>
public interface IBuildPaintState
{
    /// <summary>
    /// Transitions back to the <see cref="BuildDialogState"/>
    /// </summary>
    void OnCancel();

    /// <summary>
    /// Converts the recorded points into the feature points of the track.
    /// Transitions to <see cref="BuildEditorState"/>
    /// </summary>
    void OnDone();

    /// <summary>
    /// Clears all recorded points for a fresh start
    /// </summary>
    void Clear();

    /// <summary>
    /// Callback for alternative input sources to fire that a touch
    /// point was detected
    /// </summary>
    /// <param name="x">in screenspace</param>
    /// <param name="y">in screenspace</param>
    void OnTouchDetected( float x, float y );
}

/// <summary>
/// Track creating state where players can draw with their
/// device through the air
/// </summary>
public class BuildPaintState : State, IBuildPaintState
{
    private PointRecorder.Factory m_pointRecorderFactory;
    private IBuildStateMachine m_buildSM;
    private PointRecorder m_pointRecorder;
    private IFeaturePointsManager m_trackBuilder;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    #region Di

    [Inject]
    protected void Construct( PointRecorder.Factory _pointRecorderFactory,
                              IFeaturePointsManager _trackBuilder,
                              DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_pointRecorderFactory = _pointRecorderFactory;
        m_trackBuilder = _trackBuilder;
        m_dialogBuilderFactory = _dialogBuilderFactory;
    }

    #endregion

    #region State Lifecycle

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();

        m_pointRecorder = m_pointRecorderFactory.Create ();
        m_pointRecorder.ThrowIfNull ( nameof ( m_pointRecorder ) );
    }

    public override void ExitState()
    {
        base.ExitState ();
        m_pointRecorder = null;
    }

    #endregion

    #region Callbacks

    public void OnCancel()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnDone()
    {
        if ( !Active ) return;

        if (m_pointRecorder.PointCount < 2) {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Keep drawing!")
                .SetIcon (DialogBuilder.Icon.ALERT)
                .SetMessage ("It seems you did not draw enough to generate a track!")
                .Build ();
            return;
        }

        // Dump the Points and give them to the track builder
        Vector3 [] points, featurePoints;
        m_pointRecorder.DumpPoints ( out points );

        // Identify the feature points
        FeaturePointUtil.IdentifyFeaturePoints ( ref points, out featurePoints );

        // Check number of feature points
        if ( featurePoints.Length < 2) {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Keep drawing!")
                .SetIcon (DialogBuilder.Icon.ALERT)
                .SetMessage ("It seems you did not draw enough to generate a track!")
                .Build ();
            return;
        }

        m_buildSM.CurrentTrackData.m_featurePoints = featurePoints;

        points = null;
        featurePoints = null;

        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    public void Clear()
    {
        if ( !Active ) return;
        m_pointRecorder.ClearPoints ();
    }

    public void OnTouchDetected( float x, float y )
    {
        if ( !Active ) return;
        m_pointRecorder.RecordCurrentPoint ();
    }

    #endregion
}
