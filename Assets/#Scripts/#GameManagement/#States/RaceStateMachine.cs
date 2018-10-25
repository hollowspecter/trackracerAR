using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStateMachine : StateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RcaeStateMachine" );
        base.EnterState ();
    }
}
