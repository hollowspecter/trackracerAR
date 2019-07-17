﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IBuildPaintState
{
    void OnCancel();
    void OnDone();
}

public class BuildPaintState : State, IBuildPaintState
{
    private PointRecorder.Factory m_pointRecorderFactory;
    private IBuildStateMachine m_buildSM;
    private PointRecorder m_pointRecorder;
    private ITrackBuilderManager m_trackBuilder;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    #region Di

    [Inject]
    protected void Construct( PointRecorder.Factory _pointRecorderFactory,
                              ITrackBuilderManager _trackBuilder,
                              DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_pointRecorderFactory = _pointRecorderFactory;
        m_trackBuilder = _trackBuilder;
        m_dialogBuilderFactory = _dialogBuilderFactory;
    }

    #endregion

    #region State Lifecycle

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        m_buildSM.m_touchDetected += OnTouchDetected;

        // TODO Create a Point Recorder and Call the Record Function for every Touch Input
        m_pointRecorder = m_pointRecorderFactory.Create ();
        m_pointRecorder.ThrowIfNull ( nameof ( m_pointRecorder ) );
    }

    public override void ExitState()
    {
        base.ExitState ();
        // Dispose of the Point Recorder here
        m_pointRecorder = null;
        m_buildSM.m_touchDetected -= OnTouchDetected;
    }

    #endregion

    #region Callbacks

    public void OnCancel()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnDone()
    {
        if ( !Active ) return;

        if (m_pointRecorder.PointCount < 2) {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Keep drawing!")
                .SetIcon (DialogBuilder.Icon.ALERT)
                .SetMessage ("It seems you did not draw enough to generate a track!")
                .Build ();
            return;
        }

        // Dump the Points and give them to the track builder
        Vector3 [] points, featurePoints;
        m_pointRecorder.DumpPoints ( out points, true );

        // Identify the feature points
        FeaturePointUtil.IdentifyFeaturePoints ( ref points, out featurePoints );
        m_buildSM.CurrentTrackData.m_featurePoints = featurePoints;

        points = null;
        featurePoints = null;

        m_stateMachine.TransitionToState ( StateName.BUILD_EDITOR_STATE );
    }

    private void OnTouchDetected( float x, float y )
    {
        m_pointRecorder.RecordPoint ();
    }

    #endregion
}
