using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStreetState
{
    void OnBack();
    void OnSave();
}

public class BuildStreetState : State, IBuildStreetState
{
    public void OnBack()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_PAINT_STATE );
    }

    public void OnSave()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_SAVE_STATE );
    }
}
