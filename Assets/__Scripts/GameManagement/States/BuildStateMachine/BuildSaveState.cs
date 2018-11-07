using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildSaveState { }

public class BuildSaveState : State, IBuildSaveState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildSaveState" );
    }

    public override void ExitState ()
    {
    }
}
