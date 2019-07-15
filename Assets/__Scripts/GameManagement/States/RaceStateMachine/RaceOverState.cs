/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceOverState
{
    void OnRetry();
    void OnExit();
}

public class RaceOverState : State, IRaceOverState
{
    public override void EnterState ()
    {
        Debug.Log ( "Entered RaceOverState" );
        base.EnterState ();
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
