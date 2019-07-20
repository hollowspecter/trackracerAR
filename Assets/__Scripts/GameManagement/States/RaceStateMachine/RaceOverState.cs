/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IRaceOverState
{
    void OnRetry();
    void OnExit();
}

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
