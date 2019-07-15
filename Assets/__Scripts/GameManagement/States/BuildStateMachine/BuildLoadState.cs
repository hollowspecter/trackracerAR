using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildLoadState
{
    void CancelLoading ();
    void LoadAndEdit( string fileName );
    void LoadAndRace( string fileName );
}

public class BuildLoadState : State, IBuildLoadState
{
    private IBuildStateMachine m_buildSM;
    private ITrackBuilderManager m_trackBuilder;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    #region Di

    [Inject]
    protected void Construct( ITrackBuilderManager _trackBuilder,
                              DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_trackBuilder = _trackBuilder;
        m_dialogBuilderFactory = _dialogBuilderFactory;
    }

    #endregion

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

    public void CancelLoading ()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildLoadState: CancelLoading" );
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void LoadAndEdit( string fileName )
    {
        if ( !m_active ) return;
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
        if ( !m_active ) return;
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

    private void Load ( string fileName )
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildLoadSTate: Load fileName" );

        // try load track data
        m_buildSM.CurrentTrackData = SaveExtension.LoadTrackData ( fileName );
        m_trackBuilder.InstantiateFeaturePoints ( ref m_buildSM.CurrentTrackData.m_featurePoints );
    }
}
