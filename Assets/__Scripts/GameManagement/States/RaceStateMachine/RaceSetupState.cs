using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceSetupState { }

public class RaceSetupState : State, IRaceSetupState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceSetupState" );
        base.EnterState ();
    }
}
