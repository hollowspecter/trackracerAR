using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARStateMachine : StateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered ARStateMachine" );
        base.EnterState ();
    }
}
