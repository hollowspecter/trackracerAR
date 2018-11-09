using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildStartState { }

public class BuildStartState : State, IBuildStartState
{
    private IBuildStateMachine m_inputMachine;

    #region State Functions

    protected override void Initialise ()
    {
        base.Initialise ();
        m_inputMachine = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();
        m_inputMachine.m_touchDetected += OnTouchDetected;
    }

    public override void ExitState ()
    {
        base.ExitState ();
        m_inputMachine.m_touchDetected -= OnTouchDetected;
    }

    #endregion

    private void OnTouchDetected ( float x, float y )
    {
        RaycastHit hit;
        if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( new Vector3 ( x, y, 0 ) ), out hit, Configuration.PlaneLayer ) )
        {
            if ( Vector3.Dot ( Camera.main.transform.position - hit.point, hit.normal ) < 0 )
            {
                Debug.Log ( "Hit at back of the current Plane" );
                return;
            }

            GameObject trackStart = Object.Instantiate ( Configuration.StartPrefab, hit.point, Quaternion.identity );
            m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
        }
    }
}
