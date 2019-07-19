/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using Zenject;
using System;
using UniRx;
using Baguio.Splines;

public interface IBuildEditorState
{
    void OnCancel();
    void OnSave();
    void OnRace();
    string OnShare();
}

public class BuildEditorState : State, IBuildEditorState
{
    private IBuildStateMachine m_buildSM;
    private ITrackBuilderManager m_trackBuilder;
    private SignalBus m_signalBus;
    private IDisposable m_trackChangedSubscription;
    private IDisposable m_trackUploadSubscription;
    private UpdateUseCase m_useCase;
    private StreetView m_streetView;
    private ISplineManager m_splineManager;

    #region DI

    [Inject]
    private void Construct(ITrackBuilderManager _trackBuilder,
                           SignalBus _signalBus,
                           UpdateUseCase _useCase,
                           [Inject(Id="TrackParent")] StreetView _streetView,
                           [Inject(Id="TrackParent")] ISplineManager _splineManager)
    {
        m_trackBuilder = _trackBuilder;
        m_signalBus = _signalBus;
        m_useCase = _useCase;
        m_splineManager = _splineManager;
        m_streetView = _streetView;
    }

    #endregion

    #region State Methods

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();

        m_buildSM.m_touchDetected += OnTouchDetected;

        // Instantiate the Feature Points
        m_trackBuilder.InstantiateFeaturePoints ( ref m_buildSM.CurrentTrackData.m_featurePoints );

        OnTrackChanged ();
        m_trackChangedSubscription = m_signalBus
            .GetStream<FeaturePointMovedSignal> ()
            .Select(_ => new Unit())
            .Merge(m_signalBus.GetStream<SettingsChangedSignal>().Select(_=>new Unit()))
            .Throttle (TimeSpan.FromSeconds (1))
            .Subscribe (_ => OnTrackChanged());
    }

    public override void ExitState ()
    {
        base.ExitState ();

        m_buildSM.m_touchDetected -= OnTouchDetected;

        m_buildSM.CurrentTrackData.m_featurePoints = m_trackBuilder.GetFeaturePoints ();

        m_trackChangedSubscription?.Dispose ();
        m_trackUploadSubscription?.Dispose ();
}

    #endregion

    #region Private Functions

    private void OnTouchDetected ( float x, float y )
    {

    }

    private void OnTrackChanged()
    {
        if ( m_buildSM.CurrentTrackData.m_updateToCloud ) {
            UpdateToCloud ();
        }
        UpdateTrackMesh ();
    }

    private void UpdateTrackMesh()
    {
        m_streetView.ToggleAppearance (false, () =>
        {
            m_splineManager.GenerateTrack ();
            m_streetView.ToggleAppearance (true, null);
        });
    }

    private void UpdateToCloud()
    {
        m_trackUploadSubscription?.Dispose ();
        m_buildSM.CurrentTrackData.m_featurePoints = m_trackBuilder.GetFeaturePoints ();
        m_trackUploadSubscription = m_useCase
            .UpdateTrackToCloud (m_buildSM.CurrentTrackData)
            .Subscribe (key => Debug.Log ("Successful update! " + key));
    }

    #endregion

    #region UI Callbacks

    public void OnCancel()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildEditorState: OnCancel" );
        //m_trackBuilder.ClearFeaturePoints ();
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnSave()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildEditorState: OnSave" );
        m_stateMachine.TransitionToState ( StateName.BUILD_SAVE_STATE );
    }

    public void OnRace()
    {
        if ( !Active ) return;
        Debug.Log ("BuildEditorState: OnRace");
        m_stateMachine.TransitionToState (StateName.RACE_SM);
    }

    public string OnShare()
    {
        if ( !Active ) return null;
        UniClipboard.SetText (m_buildSM.CurrentTrackData.m_dbKey);
        return m_buildSM.CurrentTrackData.m_dbKey;
    }

    #endregion
}
