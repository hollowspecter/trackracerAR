/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Interface for the <see cref="BuildObserveDialogState"/>
/// </summary>
public interface IBuildObserveDialogState
{
    /// <summary>
    /// Transitions to the <see cref="BuildObserveState"/>
    /// </summary>
    void ObserveTrack();

    /// <summary>
    /// Returns to the <see cref="BuildDialogState"/>
    /// </summary>
    void Back();
}

/// <summary>
/// Manages the dialog to download and observe track data from the cloud.
/// </summary>
public class BuildObserveDialogState : State, IBuildObserveDialogState
{
    private IBuildStateMachine m_buildSM;

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        Debug.Log ("BuildObserveDialogState entered!");
    }

    public void Back()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_DIALOG_STATE);
    }

    public void ObserveTrack()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_OBSERVE_STATE);
    }
}
