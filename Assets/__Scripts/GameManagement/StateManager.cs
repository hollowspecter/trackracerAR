using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StateManager : MonoBehaviour
{
    private IRootStateMachine m_stateMachine;
    private Text m_debugText;

    [Inject]
    private void Construct ( IRootStateMachine _sm,
                            [Inject ( Id = "DebugStateText", Optional = true )]Text _debugText )
    {
        m_stateMachine = _sm;
        m_debugText = _debugText;
    }

    private void Start ()
    {
        m_stateMachine.IEnterState ();
    }

    private void Update ()
    {
        m_stateMachine.IUpdateActive ( Time.deltaTime );

        if ( m_debugText != null )
            m_debugText.text = m_stateMachine.IGetCurrentStateName ();
    }
}
