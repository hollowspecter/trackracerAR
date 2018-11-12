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

        m_buildSM.m_touchDetected += OnTouchDetected;

        FireArrowChanged ( ArrowPosition.IN );
        FireArrowChanged ( ArrowPosition.OUT );
    }

    public override void ExitState ()
    {
        base.ExitState ();

        m_buildSM.m_touchDetected -= OnTouchDetected;
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

    // TODO GO ON HERE
    private void OnTouchDetected ( float x, float y )
    {
        RaycastHit hit;
        if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( new Vector3 ( x, y, 0f ) ),
                              out hit,
                              50f,
                              Configuration.ArrowLayer ) )
        {
            Debug.Log ( "Arrow hit!" );

            // instantiate a new track part

            // add it to the track model (build_sm.track)

            // reposition the corresponding arrow

        }
        else if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( new Vector3 ( x, y, 0f ) ),
                              out hit,
                              50f,
                              Configuration.TrackLayer ) )
        {
            // switch out this trackpart with the next one in line?
            Debug.Log ( "TrackPart hit!" );
        }
    }
}
