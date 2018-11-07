using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildDialogState
{
    void StartNewTrack ();
    void LoadTrack ();
}

/// <summary>
/// TODO:
/// Make Actions that are called in EnterState.
/// Then next: call them, so that the ViewModel registers to those actions!
/// </summary>
public class BuildDialogState : State, IBuildDialogState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildDialogState" );
        //m_viewModel.Activate ();
    }

    public override void ExitState ()
    {
        //m_viewModel.Deactivate ();
    }

    public void StartNewTrack ()
    {
        Debug.Log ( "BuildDialogState: StartNewTrack" );
        m_stateMachine.TransitionToState ( StateName.BUILD_START_STATE );
    }

    public void LoadTrack ()
    {
        Debug.Log ( "BuildDialogState: Load Track" );
        m_stateMachine.TransitionToState ( StateName.BUILD_LOAD_STATE );
    }
}
