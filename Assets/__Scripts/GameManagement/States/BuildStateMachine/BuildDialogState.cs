/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Baguio.Splines;
using Zenject;

public interface IBuildDialogState
{
    void StartNewTrack ();
    void LoadTrack ();
    void ObserveTrack();
    void Recalibrate();
}

/// <summary>
/// TODO:
/// Make Actions that are called in EnterState.
/// Then next: call them, so that the ViewModel registers to those actions!
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
