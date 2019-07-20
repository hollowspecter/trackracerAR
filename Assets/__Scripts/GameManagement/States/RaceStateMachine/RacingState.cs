/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface for <see cref="RacingState"/>
/// </summary>
public interface IRacingState
{
    /// <summary>
    /// Transitions to <see cref="RaceSetupState"/>
    /// </summary>
    void OnBack();

    /// <summary>
    /// Transitions to <see cref="RaceOverState"/>
    /// </summary>
    void OnFinish();
}

/// <summary>
/// State during the race. Special race input will be
/// enabled, and the time will be recorded in this state.
/// </summary>
public class RacingState : State, IRacingState
{
    private TouchInput m_input;
    private SignalBus m_signalBus;

    [Inject]
    private void Construct(TouchInput _input, SignalBus _signalBus)
    {
        m_input = _input;
        m_signalBus = _signalBus;
    }

    public override void EnterState()
    {
        Debug.Log ("Entered RacingState");
        base.EnterState ();
    }

    public override void UpdateActive( double _deltaTime )
    {
        base.UpdateActive (_deltaTime);
        m_input.Tick (_deltaTime);
    }

    public void OnBack()
    {
        m_signalBus.Fire<DestroyVehicleSignal> ();
        m_stateMachine.TransitionToState (StateName.RACE_SETUP);
    }

    public void OnFinish()
    {
        m_input.SetValue (0.5f);
        m_stateMachine.TransitionToState (StateName.RACE_OVER);
    }
}
