/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface of the <see cref="BuildLoadState"/>
/// </summary>
public interface IBuildLoadState
{
    /// <summary>
    /// Returns to the <see cref="BuildDialogState"/>
    /// </summary>
    void CancelLoading ();

    /// <summary>
    /// Loads the trackdata with the given filename and
    /// transitions to <see cref="BuildEditorState"/>
    /// </summary>
    void LoadAndEdit( string fileName );

    /// <summary>
    /// Loads the trackdata with the given filename and
    /// transitions to the <see cref="RaceSetupState"/>
    /// </summary>
    /// <param name="fileName"></param>
    void LoadAndRace( string fileName );
}

/// <summary>
/// State that displays all tracks that are saved to the device.
/// From here players can load, edit, delete and race old tracks.
/// </summary>
public class BuildLoadState : State, IBuildLoadState
{
    private IBuildStateMachine m_buildSM;
    private IFeaturePointsManager m_trackBuilder;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    #region Di

    [Inject]
    protected void Construct( IFeaturePointsManager _trackBuilder,
                              DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_trackBuilder = _trackBuilder;
        m_dialogBuilderFactory = _dialogBuilderFactory;
    }

    #endregion

    #region State Methods

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        Debug.Log ( "BuildLoadSTate: EnterState" );
    }

    public override void ExitState()
    {
        base.ExitState ();
        Debug.Log ( "BuildLoadSTate: ExitState" );
    }

    #endregion

    #region Callbacks

    public void CancelLoading ()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildLoadState: CancelLoading" );
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void LoadAndEdit( string fileName )
    {
        if ( !Active ) return;
        Load (fileName);

        // check if the right button was pressed
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Edit the track?")
            .SetMessage ("Would you like to edit the loaded track?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("Cancel")
            .AddButton ("Yes", () => m_stateMachine.TransitionToState (StateName.BUILD_EDITOR_STATE))
            .Build ();
    }

    public void LoadAndRace( string fileName )
    {
        if ( !Active ) return;
        Load (fileName);

        // check if the right button was pressed
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Ready to race?")
            .SetMessage (string.Format("Are you ready to start the race on {0}?", fileName))
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("Cancel")
            .AddButton ("Yes", () => m_stateMachine.TransitionToState (StateName.RACE_SM))
            .Build ();
    }

    #endregion

    #region Helper methods

    private void Load ( string fileName )
    {
        if ( !Active ) return;
        Debug.Log ( "BuildLoadSTate: Load fileName" );

        // try load track data
        m_buildSM.CurrentTrackData = SaveExtension.LoadTrackData ( fileName );
        m_trackBuilder.InstantiateFeaturePoints ( ref m_buildSM.CurrentTrackData.m_featurePoints );
    }

    #endregion
}
