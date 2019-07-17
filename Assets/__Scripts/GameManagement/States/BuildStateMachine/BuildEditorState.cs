/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using Zenject;
using System;
using UniRx;


public interface IBuildEditorState
{
    event State.InputActionHandler m_onShowPreview;
    void OnShowPreview();
    void OnCancel();
    void OnSave();
    void OnRace();
}

public class BuildEditorState : State, IBuildEditorState
{
    public event InputActionHandler m_onShowPreview;

    private IBuildStateMachine m_buildSM;
    private ITrackBuilderManager m_trackBuilder;
    private SignalBus m_signalBus;
    private IDisposable m_updateToCloudSubscription;
    private IDisposable m_trackChangesSubscription;
    private UpdateUseCase m_useCase;

    #region DI

    [Inject]
    private void Construct(ITrackBuilderManager _trackBuilder,
                           SignalBus _signalBus,
                           UpdateUseCase _useCase)
    {
        m_trackBuilder = _trackBuilder;
        m_signalBus = _signalBus;
        m_useCase = _useCase;
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

        if (m_buildSM.CurrentTrackData.m_updateToCloud) {
            Debug.Log ("Update To Cloud is true!");
            UpdateToCloud ();
            m_updateToCloudSubscription = m_signalBus
                .GetStream<FeaturePointMovedSignal> ()
                .Select(_ => new Unit())
                .Merge(m_signalBus.GetStream<SettingsChangedSignal>().Select(_=>new Unit()))
                .Throttle (TimeSpan.FromSeconds (1))
                .Subscribe (_ => UpdateToCloud ());
        }
    }

    public override void ExitState ()
    {
        base.ExitState ();

        m_buildSM.m_touchDetected -= OnTouchDetected;

        m_buildSM.CurrentTrackData.m_featurePoints = m_trackBuilder.GetFeaturePoints ();

        m_updateToCloudSubscription?.Dispose ();
        m_trackChangesSubscription?.Dispose ();
}

    #endregion

    #region Private Functions

    private void OnTouchDetected ( float x, float y )
    {

    }

    private void UpdateToCloud()
    {
        m_trackChangesSubscription?.Dispose ();

        m_trackChangesSubscription = m_useCase
            .UpdateTrackToCloud (m_buildSM.CurrentTrackData)
            .Subscribe (key => Debug.Log ("Successful update! " + key));
    }

    #endregion

    #region UI Callbacks

    public void OnShowPreview()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildEditorState: OnShowPreview" );
        m_onShowPreview?.Invoke ();
    }

    public void OnCancel()
    {
        if ( !Active ) return;
        Debug.Log ( "BuildEditorState: OnCancel" );
        m_trackBuilder.ClearFeaturePoints ();
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

    #endregion
}
