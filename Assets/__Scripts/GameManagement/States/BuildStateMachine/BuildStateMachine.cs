using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine { }

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildState" );
    }

    public override void ExitState ()
    {
    }
}
