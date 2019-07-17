/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildObserveDialogState
{
    void ObserveTrack();
    void Back();
}

public class BuildObserveDialogState : State, IBuildObserveDialogState
{
    private IBuildStateMachine m_buildSM;

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        Debug.Log ("BuildObserveDialogState entered!");
    }

    public void Back()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_DIALOG_STATE);
    }

    public void ObserveTrack()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_OBSERVE_STATE);
    }
}
