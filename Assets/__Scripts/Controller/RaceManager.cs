﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UniRx;
using Zenject;
using Baguio.Splines;

/// <summary>
/// Automatically starts the race when the RacingState is entered.
/// Ends the race once the number of laps are reached by the player.
/// Measures the duration of the race.
/// </summary>
public class RaceManager
{
    public IReadOnlyReactiveProperty<int> Laps { get { return m_laps; } }
    public int MaxLaps { get; private set; }
    public float StartTime { get; private set; }
    public float EndTime { get; private set; }

    private IRacingState m_state;
    private ReactiveProperty<int> m_laps;
    private int m_respawns;
    private SignalBus m_signalBus;
    private ISplineManager m_splineMgr;
    private Settings m_settings;

    public RaceManager( IRacingState _state,
                       SignalBus _signalBus,
                       [Inject (Id = "TrackParent")]ISplineManager _splineMgr,
                       Settings _settings )
    {
        m_state = _state;
        ((State)m_state).m_enteredState += StartRace;
        ((State)m_state).m_exitedState += UnSubscribe;
        m_laps = new ReactiveProperty<int> (0);
        m_signalBus = _signalBus;
        m_splineMgr = _splineMgr;
        m_settings = _settings;
    }

    private void StartRace()
    {
        m_laps.Value = 0;
        m_respawns = 0;
        StartTime = Time.time;
        MaxLaps = (m_splineMgr.ClosedTrack) ? m_settings.MaxLaps : 1; // for closed tracks just one lap

        m_signalBus.Subscribe<LapSignal> (LapSignalReceived);
        m_signalBus.Subscribe<RespawnSignal> (RespawnSignalReceived);
    }

    private void UnSubscribe()
    {
        m_signalBus.Unsubscribe<LapSignal> (LapSignalReceived);
        m_signalBus.Unsubscribe<RespawnSignal> (RespawnSignalReceived);
    }

    private void EndRace()
    {
        EndTime = Time.time;
        m_state.OnFinish ();
    }

    private void LapSignalReceived()
    {
        m_laps.Value++;
        if ( m_laps.Value == MaxLaps ) {
            EndRace ();
        }
    }

    private void RespawnSignalReceived()
    {
        m_respawns++;
    }

    [System.Serializable]
    public class Settings
    {
        public int MaxLaps = 3;
    }
}
