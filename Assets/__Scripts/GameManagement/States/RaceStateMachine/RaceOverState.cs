using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceOverState { }

public class RaceOverState : State, IRaceOverState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceOverState" );
        base.EnterState ();
    }
}
