/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEditor;

/// <summary>
/// Interface for the <see cref="BuildSaveState"/>
/// </summary>
public interface IBuildSaveState
{
    /// <summary>
    /// Call for ending this state and go to race state
    /// </summary>
    void OnDone();

    /// <summary>
    /// Call to attempt to save the current track data to device
    /// </summary>
    bool OnSave ( string trackName );

    /// <summary>
    /// Returns back to the <see cref="BuildEditorState"/>
    /// </summary>
    void OnCancel ();

    /// <summary>
    /// Returns back to the <see cref="BuildDialogState"/>
    /// </summary>
    void OnNewTrack();

    /// <summary>
    /// Tries to copy the database key to clipboard, also returns it.
    /// </summary>
    string OnShare();
}

/// <summary>
/// State to handle saving the currently built track to device and cloud.
/// </summary>
public class BuildSaveState : State, IBuildSaveState
{
    private IBuildStateMachine m_buildSM;

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

    /// <summary>
    /// Copies the database key to clipboard and returns it
    /// </summary>
    /// <returns></returns>
    public string OnShare()
    {
        if ( !Active ) return null;
        UniClipboard.SetText (m_buildSM.CurrentTrackData.m_dbKey);
        return m_buildSM.CurrentTrackData.m_dbKey;
    }

    #endregion
}
