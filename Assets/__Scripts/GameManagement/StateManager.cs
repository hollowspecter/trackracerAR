using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StateManager : MonoBehaviour
{
    [Inject]
    private IRootStateMachine m_stateMachine;

    private void Start ()
    {
        m_stateMachine.IEnterState ();
    }

    private void Update ()
    {
        m_stateMachine.IUpdateActive ( Time.deltaTime );
    }
}
