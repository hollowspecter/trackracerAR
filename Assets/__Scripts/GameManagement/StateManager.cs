using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StateManager : MonoBehaviour
{
    [Inject]
    private RootStateMachine m_stateMachine;

    private void Start ()
    {
        m_stateMachine.EnterState ();
    }

    private void Update ()
    {
        m_stateMachine.UpdateActive ( Time.deltaTime );
    }
}
