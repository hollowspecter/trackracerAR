/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using Zenject;

public interface IBuildSaveState
{
    void OnDone();
    void OnSave ( string trackName );
    void OnCancel ();
}

public class BuildSaveState : State, IBuildSaveState
{
    private IBuildStateMachine m_buildSM;

    #region Di

    #endregion

    #region State Lifecycle

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();
    }

    #endregion

    #region Callbacks

    public void OnSave ( string trackName )
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildSaveState: OnSave" );
        //m_buildSM.Track.SaveAsJson ( trackName.ConvertToJsonFileName () );
    }

    public void OnCancel ()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildSaveState: OnCancel" );
        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    public void OnDone()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildSaveState: OnDone" );
        m_stateMachine.TransitionToState ( StateName.RACE_SM );
    }

    #endregion
}
