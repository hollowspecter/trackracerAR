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
}

public class RacingState : State, IRacingState
{
    private TouchInput m_input;

    [Inject]
    private void Construct( TouchInput _input)
    {
        m_input = _input;
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
        m_stateMachine.TransitionToState (StateName.RACE_SETUP);
    }
}
