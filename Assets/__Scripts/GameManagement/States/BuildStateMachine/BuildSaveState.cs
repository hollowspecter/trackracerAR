/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using UnityEditor;
using Zenject;

public interface IBuildSaveState
{
    void OnDone();
    bool OnSave ( string trackName );
    void OnCancel ();
    void OnNewTrack();
    string OnShare();
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

    /// <summary>
    /// Attempts to save a track.
    /// </summary>
    /// <returns>true if save was successful, false it an exception was thrown,
    /// or the state wasn't currently active</returns>
    public bool OnSave ( string trackName )
    {
        if ( !Active ) return false;
        Debug.Log ( "BuildSaveState: OnSave" );
        if (m_buildSM.CurrentTrackData.SaveAsJson ( trackName )) {
            #if UNITY_EDITOR
                        AssetDatabase.Refresh ();
            #endif
            return true;
        }
        return false;
    }

    public void OnCancel ()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildSaveState: OnCancel" );
        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    public void OnNewTrack()
    {
        if ( !Active ) return;
        Debug.Log ("BuildSaveState: OnNewTrack");
        m_stateMachine.TransitionToState (StateName.BUILD_DIALOG_STATE);
    }

    public void OnDone()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildSaveState: OnDone" );
        m_stateMachine.TransitionToState ( StateName.RACE_SM );
    }

    public string OnShare()
    {
        if ( !Active ) return null;
        UniClipboard.SetText (m_buildSM.CurrentTrackData.m_dbKey);
        return m_buildSM.CurrentTrackData.m_dbKey;
    }

    #endregion
}
