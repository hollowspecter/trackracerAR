/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using Zenject;
using UniRx;
using Baguio.Splines;

/// <summary>
/// Interface for <see cref="BuildObserveState"/>
/// </summary>
public interface IBuildObserveState
{
    /// <summary>
    /// Races the current status of the observed track
    /// </summary>
    void Race();

    /// <summary>
    /// Returns to the <see cref="BuildDialogState"/>
    /// </summary>
    void Back();

    /// <summary>
    /// Copies the current status of the observed track
    /// into the <see cref="BuildEditorState"/>
    /// </summary>
    void EditCopy();
}

/// <summary>
/// State to view and receive live updates for a track
/// that was uploaded to the cloud.
/// </summary>
public class BuildObserveState : State, IBuildObserveState
{
    private IBuildStateMachine m_buildSM;
    private IBuildObserveDialogUI m_observeDialogUI;
    private CompositeDisposable m_subscriptions;
    private ObserveUseCase m_useCase;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private ISplineManager m_splineManager;
    private StreetView m_streetView;
    private SignalBus m_signalBus;

    #region DI

    [Inject]
    private void Construct( IBuildObserveDialogUI _buildObserveDialogUI,
                            ObserveUseCase _useCase,
                            DialogBuilder.Factory _dialogBuilderFactory,
                            [Inject (Id = "TrackParent")] ISplineManager _splineManager,
                            [Inject (Id = "TrackParent")] StreetView _streetView,
                            SignalBus _signalBus)
    {
        m_observeDialogUI = _buildObserveDialogUI;
        m_useCase = _useCase;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_splineManager = _splineManager;
        m_streetView = _streetView;
        m_signalBus = _signalBus;
    }

    #endregion

    #region State methods

    protected override void Initialise()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState()
    {
        base.EnterState ();
        Debug.Log ("BuildObserveState entered!");
        m_subscriptions = new CompositeDisposable ();

        IObservable<TrackData> observable;
        if ( m_observeDialogUI.DoReceiveLiveUpdates ) {
            observable = m_useCase.ObserveTrack (m_observeDialogUI.KeyToDownload);
        } else {
            observable = m_useCase.GetTrack (m_observeDialogUI.KeyToDownload);
        }

        // update session and rebuild track when an update from the server was received
        m_subscriptions.Add(observable
            .SubscribeOn (Scheduler.ThreadPool)
            .ObserveOnMainThread ()
            .Subscribe (
                UpdateTrack,
                e => { m_dialogBuilderFactory.Create ().MakeGenericExceptionDialog (e); },
                () => { }));

        // rebuild track if the track move tool was used
        m_subscriptions.Add(m_signalBus.GetStream<FeaturePointChanged>()
            .SubscribeOn (Scheduler.ThreadPool)
            .ObserveOnMainThread ()
            .Subscribe (
                _ => RebuildTrack(),
                e => { m_dialogBuilderFactory.Create ().MakeGenericExceptionDialog (e); },
                () => { }));
    }

    public override void ExitState()
    {
        base.ExitState ();
        m_subscriptions?.Dispose ();
    }

    #endregion

    #region Private Methods

    private void UpdateTrack(TrackData _track )
    {
        m_buildSM.CurrentTrackData = _track;
        RebuildTrack ();
    }

    private void RebuildTrack()
    {
        m_streetView.ToggleAppearance (false, () => {
            m_splineManager.GenerateTrackFromTrackData ();
            m_streetView.ToggleAppearance (true, null);
        });
    }

    #endregion

    #region Callbacks

    public void Race()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.RACE_SM);
    }

    public void Back()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.BUILD_DIALOG_STATE);
    }

    public void EditCopy()
    {
        if ( !Active ) return;
        m_buildSM.CurrentTrackData.m_dbKey = null;
        m_stateMachine.TransitionToState (StateName.BUILD_EDITOR_STATE);
    }

    #endregion
}
