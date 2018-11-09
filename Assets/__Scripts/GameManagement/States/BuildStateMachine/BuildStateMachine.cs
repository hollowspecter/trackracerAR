using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine { }

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildStateMachine" );
        base.EnterState ();
    }
}
