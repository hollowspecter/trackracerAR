using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildEditorState { }

public class BuildEditorState : State, IBuildEditorState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildEditorState" );
        base.EnterState ();
    }
}
