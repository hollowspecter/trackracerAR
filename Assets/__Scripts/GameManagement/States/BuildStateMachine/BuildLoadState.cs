using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildLoadState
{
    void CancelLoading ();
    void Load ( string fileName );
}

public class BuildLoadState : State, IBuildLoadState
{
    private IBuildStateMachine m_buildSM;

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public void CancelLoading ()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void Load ( string fileName )
    {
        if ( !m_active ) return;

        // try load track data
        //m_buildSM.StartTrackFromLoad ( SaveExtension.LoadTrackData ( fileName.ConvertToJsonFileName () ) );

        // switch to editor
        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }
}
