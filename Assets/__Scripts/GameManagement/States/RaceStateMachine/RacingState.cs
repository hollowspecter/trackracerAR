using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRacingState { }

public class RacingState : State, IRacingState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RacingState" );
        base.EnterState ();
    }
}
