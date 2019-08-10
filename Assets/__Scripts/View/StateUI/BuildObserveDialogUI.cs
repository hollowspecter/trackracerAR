/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using UniRx;

/// <summary>
/// Interface for <see cref="BuildObserveDialogState"/>
/// </summary>
public interface IBuildObserveDialogUI
{
    /// <summary>
    /// The string of the inputfield where the user types in
    /// what track they want to download
    /// </summary>
    string KeyToDownload { get; }

    /// <summary>
    /// Returns whether the player wants to receive live updates
    /// for the downloaded track
    /// </summary>
    bool DoReceiveLiveUpdates { get; }
}

/// <summary>
/// Manages the UI for the <see cref="BuildObserveDialogState"/>
/// </summary>
[RequireComponent(typeof(UIFader))]
public class BuildObserveDialogUI : MonoBehaviour, IBuildObserveDialogUI
{
    public string KeyToDownload { get => m_keyInputField.text; }
    public bool DoReceiveLiveUpdates { get => m_receiveLiveUpdatesToggle.isOn; }

    public TMP_InputField m_keyInputField;
    public Toggle m_receiveLiveUpdatesToggle;
    public Button m_downloadButton;
    public Button m_backButton;

    private IBuildObserveDialogState m_state;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private ObserveDialogUseCase m_useCase;
    private IDisposable m_subscription;

    #region DI

    [Inject]
    private void Construct(IBuildObserveDialogState _state,
                           DialogBuilder.Factory _dialogBuilderFactory,
                           ObserveDialogUseCase _useCase)
    {
        m_state = _state;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_useCase = _useCase;

        m_keyInputField.onValueChanged.AddListener (text => {
            if ( String.IsNullOrWhiteSpace (text) ) {
                m_downloadButton.interactable = false;
            } else {
                m_downloadButton.interactable = true;
            }
        });

        m_downloadButton.onClick.AddListener(OnDownloadButtonPressed);
        m_downloadButton.interactable = false;
        m_backButton.onClick.AddListener (OnBackButtonPressed);
        
        UIFader fader = GetComponent<UIFader> ();
        fader.RegisterStateCallbacks ((State)m_state);
        gameObject.SetActive (false);
    }

    #endregion

    #region Unity methods

    private void OnDisable()
    {
        m_subscription?.Dispose ();
    }

    #endregion

    #region Button Callback

    private void OnDownloadButtonPressed()
    {
        // evaluate key. show dialog if key is not valid, else, push key to the state
        m_keyInputField.interactable = false;
        m_subscription = m_useCase.EvaluateKey (KeyToDownload)
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe (keyExists => {
                if ( keyExists ) {
                    m_state.ObserveTrack ();
                } else {
                    ShowInvalidKeyDialog ();
                }
                m_keyInputField.interactable = true;
            }, 
            e => {
                m_dialogBuilderFactory.Create ().MakeGenericExceptionDialog (e);
                m_keyInputField.interactable = true;
            },
            () => { m_keyInputField.interactable = true; })
            .AddTo (this);
    }

    private void OnBackButtonPressed()
    {
        m_state.Back ();
    }

    private void ShowInvalidKeyDialog()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Invalid key")
            .SetMessage ("The provided track key does not exist.")
            .SetIcon (DialogBuilder.Icon.ERROR)
            .Build ();
    }

    #endregion
}
