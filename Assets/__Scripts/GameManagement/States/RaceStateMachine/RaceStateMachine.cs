using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceStateMachine { }

public class RaceStateMachine : StateMachine, IRaceStateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RcaeStateMachine" );
        base.EnterState ();
    }
}
