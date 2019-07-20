/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

public enum StateName
{
    AR_SM,
    CALIBRATE_STATE,
    BUILD_SM, BUILD_DIALOG_STATE, BUILD_PAINT_STATE, BUILD_EDITOR_STATE, BUILD_SAVE_STATE, BUILD_LOAD_STATE, BUILD_OBSERVE_DIALOG_STATE, BUILD_OBSERVE_STATE,
    RACE_SM, RACE_SETUP, RACE_RACING, RACE_OVER,
    NONE
}

/// <summary>
/// State Class
/// Is an abstract class that must be inherited by new
/// States. Initialised the states and wraps up the Update
/// function.
/// </summary>
public abstract class State
{
    public delegate void InputActionHandler ();
    public delegate void TouchHandler ( float x, float y );

    public event InputActionHandler m_enteredState;
    public event InputActionHandler m_exitedState;

    public bool Active { get; protected set; }
    protected StateMachine m_stateMachine; // parent state machine

    public void SetParentStateMachine ( StateMachine _sm )
    {
        m_stateMachine = _sm;
        Initialise ();
    }

    public virtual void SetActive ( bool _active )
    {
        Active = _active;
    }

    /// <summary>
    /// Override this as your Awake/Start/Init Function.
    /// Is called when this State is added to a StateMachine
    /// </summary>
    protected virtual void Initialise () {
        Active = false;
    }

    /// <summary>
    /// Override this as your Update Function in the State.
    /// Is only called when the current state is active
    /// </summary>
    public virtual void UpdateActive ( double _deltaTime ) { }

    /// <summary>
    /// Gets called when entering this state.
    /// For more info see GameManager class
    /// </summary>
    public virtual void EnterState ()
    {
        m_enteredState?.Invoke ();
    }

    /// <summary>
    /// Gets called when exiting this state.
    /// For more info see GameManager class
    /// </summary>
    public virtual void ExitState ()
    {
        m_exitedState?.Invoke ();
    }
}
