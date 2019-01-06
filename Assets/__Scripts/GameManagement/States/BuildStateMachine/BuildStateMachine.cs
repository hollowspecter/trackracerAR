using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine
{
    // events
    event State.TouchHandler m_touchDetected;

    // propertoes
    TrackModel Track { get; }
    Transform TrackTransform { get; }

    // functions
    void StartNewTrack ( TrackPart _start );
    void StartTrackFromLoad ( TrackDataStructure _trackData );
}

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public event TouchHandler m_touchDetected;

    private TrackModel m_track;
    private Transform m_trackTransform;

    public TrackModel Track { get { return m_track; } }
    public Transform TrackTransform { get { return m_trackTransform; } }

    public override void EnterState ()
    {
        base.EnterState ();
        GameObject trackGO = new GameObject ();
        trackGO.name = "TrackRoot";
        m_trackTransform = trackGO.transform;
    }

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

#if !UNITY_EDITOR
        Touch touch;
        if ( Input.touchCount < 1 || ( touch = Input.GetTouch ( 0 ) ).phase != TouchPhase.Began )
        {
            return;
        }
        if ( m_touchDetected != null ) m_touchDetected ( touch.position.x, touch.position.y );
#else
        if ( !Input.GetMouseButtonDown ( 0 ) )
        {
            return;
        }
        if ( m_touchDetected != null ) m_touchDetected ( Input.mousePosition.x, Input.mousePosition.y );
#endif
    }

    public void StartNewTrack ( TrackPart _start )
    {
        if ( _start == null )
        {
            throw new System.ArgumentNullException ( "start" );
        }

        m_track = new TrackModel ( _start );
    }

    public void StartTrackFromLoad ( TrackDataStructure _trackData )
    {
        if ( _trackData == null )
        {
            throw new System.ArgumentNullException ( "_trackData" );
        }

        m_track = new TrackModel ( _trackData );
    }
}
