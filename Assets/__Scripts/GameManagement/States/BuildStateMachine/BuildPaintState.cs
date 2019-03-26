/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildPaintState
{
    void OnCancel();
    void OnDone();
}

public class BuildPaintState : State, IBuildPaintState
{
    private IBuildStateMachine m_buildSM;

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;

        // TODO Create a Point Recorder and Call the Record Function for every Touch Input
    }

    public override void ExitState()
    {
        // TODO

        // Dispose of the Point Recorder here

        base.ExitState ();
    }

    public void OnCancel()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnDone()
    {
        if ( !m_active ) return;

        // TODO

        // Dump the Points of the Point Recorder

        // Create the Prefabs with the Factory at the Positions to a certain injected root object

        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }
}
