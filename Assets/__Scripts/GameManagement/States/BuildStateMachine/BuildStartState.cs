using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStartState { }

public class BuildStartState : State, IBuildStartState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceOverState" );
    }

    public override void ExitState ()
    {
    }
}
