/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildObserveDialogState
{
    void ObserveTrack( string _key, bool _withLiveUpdates );
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
        Debug.Log ("BuildObserveDialogState Entered");
    }

    public void Back()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_DIALOG_STATE);
    }

    public void ObserveTrack( string _key, bool _withLiveUpdates )
    {
        if ( !Active ) return;
        // todo save key and with live updates somewhere! in the observe state?
        m_stateMachine.TransitionToState (StateName.BUILD_OBSERVE_STATE);
    }
}
