using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootStateMachine
{
    void IEnterState ();
    void IUpdateActive ( float _deltaTime );
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

    public void IUpdateActive ( float _deltaTime )
    {
        UpdateActive ( _deltaTime );
    }
}