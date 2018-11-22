using UnityEngine;

public interface IBuildSaveState
{
    void OnSave ( string trackName );
    void OnCancel ();
}

public class BuildSaveState : State, IBuildSaveState
{
    private IBuildStateMachine m_buildSM;

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();
    }

    public void OnSave ( string trackName )
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildSaveState: OnSave" );

        m_buildSM.Track.SaveAsJson ( trackName.ConvertToJsonFileName () );

        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    public void OnCancel ()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildSaveState: OnCancel" );
        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }
}
