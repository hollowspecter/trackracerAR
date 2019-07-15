/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IRacingState
{
    void OnBack();
    void OnFinish();
}

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
