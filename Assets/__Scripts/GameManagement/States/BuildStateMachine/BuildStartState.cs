﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[System.Obsolete]
public interface IBuildStartState { }

[System.Obsolete]
public class BuildStartState : State, IBuildStartState
{
    private IBuildStateMachine m_buildSM;

    #region State Functions

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();
        m_buildSM.m_touchDetected += OnTouchDetected;
    }

    public override void ExitState ()
    {
        base.ExitState ();
        m_buildSM.m_touchDetected -= OnTouchDetected;
    }

    #endregion

    private void OnTouchDetected ( float x, float y )
    {
        RaycastHit hit;
        if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( new Vector3 ( x, y, 0 ) ),
                              out hit,
                              50f,
                              Configuration.PlaneLayer ) )
        {
            // Instantiate Starttrack
            GameObject trackStart = Object.Instantiate ( Configuration.TrackParts [ 0 ], hit.point, Quaternion.identity );
            TrackPart trackPart = trackStart.GetComponent<TrackPart> ();
            //trackPart.transform.parent = m_buildSM.TrackTransform;
            if ( trackPart == null ) Debug.LogError ( "The start tile does not have a TrackPart" );

            // Create the model
            //m_buildSM.StartNewTrack ( trackPart );

            // Change state
            m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
        }
    }
}
