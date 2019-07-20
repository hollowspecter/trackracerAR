/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface for <see cref="RaceOverState"/>
/// </summary>
public interface IRaceOverState
{
    /// <summary>
    /// Returns to the <see cref="RaceSetupState"/>
    /// </summary>
    void OnRetry();

    /// <summary>
    /// Returns to the <see cref="BuildDialogState"/>
    /// </summary>
    void OnExit();
}

/// <summary>
/// State displays the racing score and you can
/// see the vehicle still racing.
/// </summary>
public class RaceOverState : State, IRaceOverState
{
    private SignalBus m_signalBus;

    [Inject]
    private void Construct(SignalBus _signalBus )
    {
        m_signalBus = _signalBus;
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceOverState" );
        base.EnterState ();
    }

    public override void ExitState()
    {
        base.ExitState ();
        m_signalBus.Fire<DestroyVehicleSignal> ();
    }

    public void OnRetry()
    {
        m_stateMachine.TransitionToState (StateName.RACE_SETUP);
    }

    public void OnExit()
    {
        m_stateMachine.TransitionToState (StateName.BUILD_SM);
    }
}
