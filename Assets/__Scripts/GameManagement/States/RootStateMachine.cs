using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootStateMachine
{
    void IEnterState ();
    void IUpdateActive ( float _deltaTime );
    string IGetCurrentStateName ();
}

public class RootStateMachine : StateMachine, IRootStateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RootStateMachine" );
        base.EnterState ();
    }

    public void IEnterState ()
    {
        EnterState ();
    }

    public string IGetCurrentStateName ()
    {
        return GetCurrentStateName ();
    }

    public void IUpdateActive ( float _deltaTime )
    {
        UpdateActive ( _deltaTime );
    }
}