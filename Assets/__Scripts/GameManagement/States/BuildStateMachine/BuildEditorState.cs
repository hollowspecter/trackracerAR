using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildEditorState
{
    event BuildEditorState.PositioningHandler m_outArrowPositionChanged;
    event BuildEditorState.PositioningHandler m_inArrowPositionChanged;
}

public class BuildEditorState : State, IBuildEditorState
{
    public delegate void PositioningHandler ( Vector3 _pos, Quaternion _rot );

    public event PositioningHandler m_outArrowPositionChanged;
    public event PositioningHandler m_inArrowPositionChanged;

    private IBuildStateMachine m_buildSM;

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered BuildEditorState" );
        base.EnterState ();

        FireArrowChanged ( ArrowPosition.IN );
        FireArrowChanged ( ArrowPosition.OUT );
    }

    private void FireArrowChanged ( ArrowPosition _arrowPos )
    {
        Vector3 newPos;
        Quaternion newRot;
        m_buildSM.Track.GetArrowPositioning ( _arrowPos, out newPos, out newRot );

        switch ( _arrowPos )
        {
            case ArrowPosition.IN: m_inArrowPositionChanged ( newPos, newRot ); break;
            case ArrowPosition.OUT: m_outArrowPositionChanged ( newPos, newRot ); break;
            default: throw new System.NotImplementedException ( _arrowPos.ToString () );
        }
    }
}
