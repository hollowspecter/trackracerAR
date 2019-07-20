/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.Assertions;

[RequireComponent(typeof(UIFader))]
public class TimerView : MonoBehaviour
{
    public TextMeshProUGUI m_timerText;

    private RaceManager m_raceManager;
    private IRacingState m_racingState;

    [Inject]
    private void Construct(RaceManager _raceManager,
                           IRaceSetupState raceSetupState,
                           IRacingState _racingState,
                           IRaceOverState _raceOverState)
    {
        m_raceManager = _raceManager;
        m_racingState = _racingState;

        UIFader fader = GetComponent<UIFader> ();
        fader.RegisterStateCallbacks (
            (State)_racingState,
            (State)_raceOverState);
        ((State)raceSetupState).m_enteredState += fader.Deactivate;

        gameObject.SetActive (false);
    }

    private void Update()
    {
        TimeSpan timespan;
        if (((State)m_racingState).Active) {
            timespan = TimeSpan.FromSeconds (Time.time - m_raceManager.StartTime);
            m_timerText.text = timespan.ToString (@"mm\:ss");
        } else {
            timespan = TimeSpan.FromSeconds (m_raceManager.EndTime - m_raceManager.StartTime);
            m_timerText.text = timespan.ToString ("g");
        }
    }
}
