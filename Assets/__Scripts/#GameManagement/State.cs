public enum StateName
{
    AR_SM,
    TRACKBUILDER_BUILD,
    TRACKRACER_SM, TRACKRACER_SETUP, TRACKRACER_RACING, TRACKRACER_OVER,
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
    protected bool m_active = false;
    protected StateMachine m_stateMachine; // parent state machine

    public void SetParentStateMachine ( StateMachine _sm )
    {
        m_stateMachine = _sm;
        Initialise ();
    }

    virtual public void SetActive ( bool _active )
    {
        m_active = _active;
    }

    /// <summary>
    /// Override this as your Awake/Start/Init Function.
    /// Is called when this State is added to a StateMachine
    /// </summary>
    abstract protected void Initialise ();

    /// <summary>
    /// Override this as your Update Function in the State.
    /// Is only called when the current state is active
    /// </summary>
    abstract public void UpdateActive ( double _deltaTime );

    /// <summary>
    /// Gets called when entering this state.
    /// For more info see GameManager class
    /// </summary>
    abstract public void EnterState ();

    /// <summary>
    /// Gets called when exitigng this state.
    /// For more info see GameManager class
    /// </summary>
    abstract public void ExitState ();
}
