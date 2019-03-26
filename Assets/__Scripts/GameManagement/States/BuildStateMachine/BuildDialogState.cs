using UnityEngine;

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
    public override void EnterState ()
    {
        base.EnterState ();
    }

    public void StartNewTrack ()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_PAINT_STATE );
    }

    public void LoadTrack ()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_LOAD_STATE );
    }
}
