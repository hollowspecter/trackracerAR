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

[RequireComponent (typeof (UIFader))]
public class RaceSetupUI : MonoBehaviour
{
    public TextMeshProUGUI m_countdownText;
    public Button m_startButton;
    public TextMeshProUGUI m_startButtonText;

    private IRaceSetupState m_state;
    private UIFader m_fader;
    private Settings m_settings;
    private IDisposable m_subscription;

    [Inject]
    private void Construct( IRaceSetupState _state, Settings _settings )
    {
        m_state = _state;
        m_settings = _settings;
        m_startButton.onClick.AddListener (StartButtonPressed);

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        // turn off this gameobject in case it is active
        gameObject.SetActive (false);
    }

    public void BackButtonPressed()
    {
        m_subscription?.Dispose ();
        m_subscription = null;
        m_state.OnBack ();
    }

    public void StartButtonPressed()
    {
        if ( m_subscription == null ) {
            // Start Countdown
            m_countdownText.text = (m_settings.CountdownDurationInSeconds + 1).ToString ();
            m_subscription =
                Observable.Timer (TimeSpan.FromSeconds (1), TimeSpan.FromSeconds (1))
                .Select (i => m_settings.CountdownDurationInSeconds - i)
                .Take (m_settings.CountdownDurationInSeconds + 1)
                .Subscribe (i => m_countdownText.text = i.ToString (), //onNext
                    () => {
                        m_startButtonText.text = "Start";
                        m_state.OnStart ();
                    }); //doOnComplete

            m_startButtonText.text = "Abort";
        } else {
            m_subscription.Dispose ();
            m_subscription = null;
            m_startButtonText.text = "Start";
            m_countdownText.text = "";
        }
    }

    private void OnEnable()
    {
        m_countdownText.text = "";
    }

    private void OnDisable()
    {
        m_subscription?.Dispose ();
        m_subscription = null;
    }

    [Serializable]
    public class Settings
    {
        public int CountdownDurationInSeconds = 2;
    }
}
