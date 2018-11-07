using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildEditorState { }

public class BuildEditorState : State, IBuildEditorState
{
    protected override void Initialise ()
    {
    }

    public override void UpdateActive ( double _deltaTime )
    {
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildEditorState" );
    }

    public override void ExitState ()
    {
    }
}
