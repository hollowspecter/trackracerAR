using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// StateMachine that can be derived from.
/// Is extending the State class to enable
/// hierarchical StateMachines.
/// </summary>
public class StateMachine : State
{
    private Dictionary<StateName, State> m_states;

    protected StateName m_currentState = StateName.NONE;
    protected StateName m_defaultState = StateName.NONE;
    protected StateName m_previousState = StateName.NONE;

    public StateName PreviousState { get { return m_previousState; } }

    public StateMachine ()
    {
        m_states = new Dictionary<StateName, State> ();
    }

    /// <summary>
    /// Transitions to the new State.
    /// This will call the ExitState method if 
    /// </summary>
    /// <param name="_newState">New state.</param>
    public void TransitionToState ( StateName _newState )
    {
        // if this statemachine has a current state
        if ( IsState ( m_currentState ) )
        {
            m_states [ m_currentState ].ExitState ();
            m_states [ m_currentState ].SetActive ( false );
            m_previousState = m_currentState;
            m_currentState = StateName.NONE;
        }

        // if statemachine does contain the state to transition to, do it
        if ( m_states.ContainsKey ( _newState ) )
        {
            m_states [ _newState ].EnterState ();
            m_states [ _newState ].SetActive ( true );
            m_currentState = _newState;
        }
        else
        {
            // is there a layer above?
            if ( m_stateMachine != null )
                m_stateMachine.TransitionToState ( _newState );
            else
                Debug.LogErrorFormat ( "The state {0} has not been found.",
                                      System.Enum.GetName ( typeof ( StateName ), _newState ) );
        }
    }

    public void AddState ( StateName _stateName, State _state )
    {
        // check for traps
        Assert.IsNotNull ( _state );
        Assert.IsNotNull ( m_states );
        if ( !IsState ( _stateName ) )
        {
            Debug.LogError ( "StateMachineSetupError: StateName was set to NONE." );
            return;
        }

        // add the state
        m_states.Add ( _stateName, _state );
        _state.SetParentStateMachine ( this );
        if ( !IsState ( m_defaultState ) )
        {
            m_defaultState = _stateName;
        }
    }

    #region State Functions

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

        // no current state? transition to default state
        if ( !IsState ( m_currentState ) )
        {
            TransitionToState ( m_defaultState );
        }

        // call update tick of current state
        m_states [ m_currentState ].UpdateActive ( _deltaTime );
    }

    public override void EnterState ()
    {
        base.EnterState ();

        // if there is no current state upon entering this sm
        // enter default state
        if ( !IsState ( m_currentState ) )
        {
            TransitionToState ( m_defaultState );
        }
    }

    public override void ExitState ()
    {
        base.ExitState ();

        // if there is a current state upon exiting,
        // make it the defaultstate so you reenter this state
        if ( IsState ( m_currentState ) )
        {
            m_defaultState = m_currentState;
        }
    }

    public override void SetActive ( bool _active )
    {
        // set this active and the current state
        base.SetActive ( _active );
        if ( IsState ( m_currentState ) )
        {
            m_states [ m_currentState ].SetActive ( _active );
        }
    }

    #endregion

    #region Helper Functions

    public string GetCurrentStateName ()
    {
        if ( !IsState ( m_currentState ) )
        {
            return "None";
        }

        StateMachine sm = m_states [ m_currentState ] as StateMachine;
        string lowerState = "";
        if ( sm != null )
        {
            lowerState = "/" + sm.GetCurrentStateName ();
        }

        return System.Enum.GetName ( typeof ( StateName ), m_currentState ) + lowerState;
    }

    /// <returns><c>true</c>, if state is valid, <c>false</c> is statename is NONE.</returns>
    private static bool IsState ( StateName _stateName )
    {
        return _stateName != StateName.NONE;
    }

    #endregion
}
