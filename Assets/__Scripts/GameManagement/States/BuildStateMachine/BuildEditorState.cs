using UnityEngine;

public interface IBuildEditorState
{
    void OnTrackDone ();
    event BuildEditorState.PositioningHandler m_outArrowPositionChanged;
    event BuildEditorState.PositioningHandler m_inArrowPositionChanged;
}

public class BuildEditorState : State, IBuildEditorState
{
    public delegate void PositioningHandler ( Vector3 _pos, Quaternion _rot );

    public event PositioningHandler m_outArrowPositionChanged;
    public event PositioningHandler m_inArrowPositionChanged;

    private IBuildStateMachine m_buildSM;

    #region State Methods

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
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

    #endregion

    #region Building Functions

    private void OnTouchDetected ( float x, float y )
    {
        RaycastHit hit;
        if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( new Vector3 ( x, y, 0f ) ),
                              out hit,
                              50f,
                              Configuration.ArrowLayer ) )
        {
            ArrowTouched ( hit );
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

    private void ArrowTouched ( RaycastHit hit )
    {
        // check which arrow it was
        ArrowPosition arrowPos = hit.collider.GetComponent<Arrow> ().ArrowPosition;

        // instantiate a new track part, and position it
        Vector3 pos;
        Quaternion rot;
        TrackPart trackPart = Object.Instantiate ( Configuration.TrackParts [ 1 ] ).GetComponent<TrackPart> ();
        m_buildSM.Track.GetPositioning ( arrowPos, out pos, out rot );
        trackPart.SetPositioning ( arrowPos, pos, rot );
        trackPart.transform.parent = m_buildSM.TrackTransform;

        // add it to the track model (build_sm.track)
        switch ( arrowPos )
        {
            case ArrowPosition.IN: m_buildSM.Track.PrependPart ( trackPart ); break;
            case ArrowPosition.OUT: m_buildSM.Track.AppendPart ( trackPart ); break;
            default: throw new System.NotImplementedException ( arrowPos.ToString () );
        }

        // reposition the corresponding arrow
        FireArrowChanged ( arrowPos );
    }

    private void FireArrowChanged ( ArrowPosition _arrowPos )
    {
        Vector3 newPos;
        Quaternion newRot;
        m_buildSM.Track.GetArrowPositioning ( _arrowPos, out newPos, out newRot );

        switch ( _arrowPos )
        {
            case ArrowPosition.IN: if ( m_inArrowPositionChanged != null ) m_inArrowPositionChanged ( newPos, newRot ); break;
            case ArrowPosition.OUT: if ( m_outArrowPositionChanged != null ) m_outArrowPositionChanged ( newPos, newRot ); break;
            default: throw new System.NotImplementedException ( _arrowPos.ToString () );
        }
    }

    #endregion

    #region UI Callbacks

    public void OnTrackDone ()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildEditorState: OnTrackDone" );
        m_stateMachine.TransitionToState ( StateName.BUILD_SAVE_STATE );
    }

    #endregion
}
