using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildSaveState { }

public class BuildSaveState : State, IBuildSaveState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildSaveState" );
        base.EnterState ();
    }
}
