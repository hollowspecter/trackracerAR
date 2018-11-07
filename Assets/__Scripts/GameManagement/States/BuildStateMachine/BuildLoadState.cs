using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildLoadState { }

public class BuildLoadState : State, IBuildLoadState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildLoadState" );
    }

    public override void ExitState ()
    {
    }
}
