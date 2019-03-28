public enum StateName
{
    AR_SM,
    BUILD_SM, BUILD_DIALOG_STATE, BUILD_PAINT_STATE, BUILD_EDITOR_STATE, BUILD_SAVE_STATE, BUILD_LOAD_STATE, BUILD_STREET_STATE,
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

    protected bool m_active = false;
    protected StateMachine m_stateMachine; // parent state machine

    public void SetParentStateMachine ( StateMachine _sm )
    {
        m_stateMachine = _sm;
        Initialise ();
    }

    public virtual void SetActive ( bool _active )
    {
        m_active = _active;
    }

    /// <summary>
    /// Override this as your Awake/Start/Init Function.
    /// Is called when this State is added to a StateMachine
    /// </summary>
    protected virtual void Initialise () { }

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
