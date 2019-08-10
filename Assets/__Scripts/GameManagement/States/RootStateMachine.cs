/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

 /// <summary>
 /// Interface for the <see cref="RootStateMachine"/>
 /// </summary>
public interface IRootStateMachine
{
    /// <summary>
    /// Call this to kick off the state machine and enter the first state
    /// </summary>
    void IEnterState ();

    /// <summary>
    /// Call this from a tick function to pass the ticks down the state machine
    /// </summary>
    void IUpdateActive ( float _deltaTime );

    /// <summary>
    /// Returns the names of the current states
    /// </summary>
    string IGetCurrentStateName ();
}

/// <summary>
/// The root statemachine
/// </summary>
public class RootStateMachine : StateMachine, IRootStateMachine
{
    public override void EnterState ()
    {
        base.EnterState ();
    }

    public void IEnterState ()
    {
        EnterState ();
    }

    public string IGetCurrentStateName ()
    {
        return GetCurrentStateName ();
    }

    public void IUpdateActive ( float _deltaTime )
    {
        UpdateActive ( _deltaTime );
    }
}