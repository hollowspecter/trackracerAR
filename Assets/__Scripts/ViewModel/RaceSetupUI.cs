/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UniRx;

[RequireComponent (typeof (UIFader))]
public class RaceSetupUI : MonoBehaviour
{
    public TextMeshProUGUI m_countdownText;

    private IRaceSetupState m_state;
    private UIFader m_fader;
    private Settings m_settings;
    private IDisposable m_subscription;

    [Inject]
    private void Construct(IRaceSetupState _state, Settings _settings)
    {
        m_state = _state;
        m_settings = _settings;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        // turn off this gameobject in case it is active
        gameObject.SetActive (false);
    }

    public void BackButtonPressed()
    {
        if (m_subscription != null) {
            m_subscription.Dispose ();
            m_subscription = null;
        } else {
            m_state.OnBack ();
        }
    }

    public void StartButtonPressed()
    {
        if (m_subscription == null) {
            // Start Countdown
            m_subscription = 
                Observable.Timer (TimeSpan.FromSeconds (1), TimeSpan.FromSeconds (1))
                .Select (i => m_settings.CountdownDurationInSeconds - i)
                .Take (m_settings.CountdownDurationInSeconds + 1)
                .Subscribe (
                    i => m_countdownText.text=i.ToString(), //onNext
                    m_state.OnStart); //doOnComplete
        }
    }

    [Serializable]
    public class Settings
    {
        public int CountdownDurationInSeconds = 3;
    }
}
