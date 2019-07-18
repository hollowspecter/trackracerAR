/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Baguio.Splines;

public interface IBuildObserveState
{
    void Race();
    void Back();
    void EditCopy();
}

public class BuildObserveState : State, IBuildObserveState
{
    private IBuildStateMachine m_buildSM;
    private BuildObserveDialogUI m_observeDialogUI;
    private IDisposable m_subscription;
    private ObserveUseCase m_useCase;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private ISplineManager m_splineManager;
    private StreetView m_streetView;

    #region DI

    [Inject]
    private void Construct( BuildObserveDialogUI _buildObserveDialogUI,
                            ObserveUseCase _useCase,
                            DialogBuilder.Factory _dialogBuilderFactory,
                            [Inject (Id = "TrackParent")] ISplineManager _splineManager,
                            [Inject (Id = "TrackParent")] StreetView _streetView )
    {
        m_observeDialogUI = _buildObserveDialogUI;
        m_useCase = _useCase;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_splineManager = _splineManager;
        m_streetView = _streetView;
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

        if (m_observeDialogUI.DoReceiveLiveUpdates) {
            m_subscription = m_useCase.ObserveTrack (m_observeDialogUI.KeyToDownload)
                .SubscribeOn (Scheduler.ThreadPool)
                .ObserveOnMainThread ()
                .Subscribe (
                    UpdateTrack,
                    e => { m_dialogBuilderFactory.Create ().MakeGenericExceptionDialog(e); },
                    () => { });
        } else {
            m_subscription = m_useCase.GetTrack (m_observeDialogUI.KeyToDownload)
                .SubscribeOn (Scheduler.ThreadPool)
                .ObserveOnMainThread ()
                .Subscribe (
                    UpdateTrack,
                    e => { m_dialogBuilderFactory.Create ().MakeGenericExceptionDialog (e); },
                    () => { });
        }
    }

    public override void ExitState()
    {
        base.ExitState ();
        m_subscription?.Dispose ();
    }

    #endregion

    #region Private Methods

    private void UpdateTrack(TrackData _track )
    {
        m_buildSM.CurrentTrackData = _track;
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
