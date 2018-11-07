using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildDialogState { }

public class BuildDialogState : State, IBuildDialogState
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
