using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRacingState { }

public class RacingState : State, IRacingState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RacingState" );
    }

    public override void ExitState ()
    {
    }
}
