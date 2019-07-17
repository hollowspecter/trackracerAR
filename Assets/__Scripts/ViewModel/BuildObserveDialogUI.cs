/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using UniRx;

[RequireComponent(typeof(UIFader))]
public class BuildObserveDialogUI : MonoBehaviour
{
    public TMP_InputField m_keyInputField;
    public Toggle m_receiveLiveUpdatesToggle;
    public Button m_downloadButton;

    private IBuildObserveDialogState m_state;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private ObserveDialogUseCase m_useCase;
    private IDisposable m_subscription;

    private string KeyToDownload { get => m_keyInputField.text; }
    private bool DoReceiveLiveUpdates { get => m_receiveLiveUpdatesToggle.isOn; }
    
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
        
        UIFader fader = GetComponent<UIFader> ();
        fader.RegisterStateCallbacks ((State)m_state);
        gameObject.SetActive (false);
    }

    private void OnDownloadButtonPressed()
    {
        // evaluate key. show dialog if key is not valid, else, push key to the state
        m_keyInputField.interactable = false;
        m_subscription = m_useCase.EvaluateKey (KeyToDownload)
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe (keyExists => {
                if ( keyExists ) {
                    m_state.ObserveTrack (KeyToDownload, DoReceiveLiveUpdates);
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

    private void OnDisable()
    {
        m_subscription.Dispose ();
    }

    private void ShowInvalidKeyDialog()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Invalid key")
            .SetMessage ("The provided track key does not exist.")
            .SetIcon (DialogBuilder.Icon.ERROR)
            .Build ();
    }
}
