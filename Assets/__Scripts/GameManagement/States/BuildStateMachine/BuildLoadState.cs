using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildLoadState { }

public class BuildLoadState : State, IBuildLoadState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildLoadState" );
        base.EnterState ();
    }
}
