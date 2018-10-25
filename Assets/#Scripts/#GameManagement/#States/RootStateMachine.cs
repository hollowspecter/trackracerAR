using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootStateMachine : StateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RootStateMachine" );
        base.EnterState ();
    }
}
