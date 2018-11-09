using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStartState { }

public class BuildStartState : State, IBuildStartState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildStartState" );
        base.EnterState ();
    }
}
