using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildLoadState
{
    void CancelLoading ();
    void Load ( string fileName );
}

public class BuildLoadState : State, IBuildLoadState
{
    private IBuildStateMachine m_buildSM;
    private ITrackBuilderManager m_trackBuilder;

    #region Di

    [Inject]
    protected void Construct( ITrackBuilderManager _trackBuilder )
    {
        m_trackBuilder = _trackBuilder;
    }

    #endregion

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public void CancelLoading ()
    {
        if ( !m_active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void Load ( string fileName )
    {
        if ( !m_active ) return;

        // try load track data
        m_buildSM.CurrentTrackData = SaveExtension.LoadTrackData ( fileName.ConvertToJsonFileName () );
        m_trackBuilder.InstantiateFeaturePoints ( ref m_buildSM.CurrentTrackData.m_featurePoints );

        // switch to editor
        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }
}
