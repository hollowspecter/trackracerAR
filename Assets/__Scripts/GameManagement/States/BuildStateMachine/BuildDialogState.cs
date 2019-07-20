/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Baguio.Splines;
using Zenject;

/// <summary>
/// Interface for <see cref="BuildDialogState"/>
/// </summary>
public interface IBuildDialogState
{
    /// <summary>
    /// Creates new session data and transitions to <see cref="BuildPaintState"/>
    /// </summary>
    void StartNewTrack ();

    /// <summary>
    /// Transitions to <see cref="BuildLoadState"/>
    /// </summary>
    void LoadTrack ();

    /// <summary>
    /// Transitions to <see cref="BuildObserveDialogState"/>
    /// </summary>
    void ObserveTrack();

    /// <summary>
    /// Transitions back to <see cref="CalibrateState"/>
    /// </summary>
    void Recalibrate();
}

/// <summary>
/// Start point of the build process. Player can choose
/// whether to create a new track, load a track or download
/// a track from the cloud.
/// </summary>
public class BuildDialogState : State, IBuildDialogState
{
    private IBuildStateMachine m_buildSM;
    private ISplineManager m_splineManager;
    private ITrackBuilderManager m_trackBuilder;

    #region DI

    [Inject]
    private void Construct( [Inject (Id = "TrackParent")] ISplineManager _splineManager,
                            ITrackBuilderManager _trackBuilder)
    {
        m_splineManager = _splineManager;
        m_trackBuilder = _trackBuilder;
    }

    #endregion

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();
        Debug.Log ( "Entering BuildDialogState" );
        m_buildSM.CurrentFeaturePointOffset = Vector3.zero;
    }

    public void StartNewTrack ()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildDialogState: StartNewTrack" );
        m_buildSM.CurrentTrackData = TrackData.CreateRandomizedTrackData ();
        m_splineManager.ClearMesh ();
        m_trackBuilder.ClearFeaturePoints ();
        m_stateMachine.TransitionToState ( StateName.BUILD_PAINT_STATE );
    }

    public void LoadTrack ()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildDialogState: LoadTrack" );
        m_stateMachine.TransitionToState ( StateName.BUILD_LOAD_STATE );
    }

    public void ObserveTrack()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_OBSERVE_DIALOG_STATE);
    }

    public void Recalibrate()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.CALIBRATE_STATE);
    }
}
