using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootStateMachine
{
}

public class RootStateMachine : StateMachine, IRootStateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RootStateMachine" );
        base.EnterState ();
    }
}