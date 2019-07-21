/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using Zenject;
using TMPro;
using UniRx;

/// <summary>
/// Interface for <see cref="RacingUI"/>
/// </summary>
public interface IRacingUI { }

/// <summary>
/// Manages the UI for <see cref="RacingState"/>
/// </summary>
[RequireComponent (typeof (UIFader))]
public class RacingUI : MonoBehaviour
{
    public TextMeshProUGUI m_lapText;

    private IRacingState m_state;
    private UIFader m_fader;
    private RaceManager m_raceManager;
    private IDisposable m_subscription;

    [Inject]
    private void Construct( IRacingState _state,
                            RaceManager _raceManager)
    {
        m_state = _state;
        m_raceManager = _raceManager;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        // turn off this gameobject in case it is active
        gameObject.SetActive (false);
    }

    private void OnEnable()
    {
        m_subscription = m_raceManager.Laps
            .Subscribe (laps => m_lapText.text = string.Format ("{0}/{1} Laps", laps, m_raceManager.MaxLaps));
    }

    private void OnDisable()
    {
        m_subscription?.Dispose ();
    }

    public void BackButtonPressed()
    {
        m_state.OnBack ();
    }
}
