/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class RaceManager
{
    public IReadOnlyReactiveProperty<int> Laps { get { return m_laps; } }

    public const int MAX_LAPS = 3;

    private IRacingState m_state;
    private ReactiveProperty<int> m_laps;
    private int m_respawns;
    private SignalBus m_signalBus;

    public RaceManager(IRacingState _state, SignalBus _signalBus)
    {
        m_state = _state;
        ((State)m_state).m_enteredState += StartRace;
        ((State)m_state).m_exitedState += UnSubscribe;
        m_laps = new ReactiveProperty<int> (0);
        m_signalBus = _signalBus;
    }

    private void StartRace()
    {
        m_laps.Value = 0;
        m_respawns = 0;

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
        m_state.OnFinish ();
    }

    private void LapSignalReceived()
    {
        m_laps.Value++;
        if (m_laps.Value == MAX_LAPS) {
            EndRace ();
        }
    }

    private void RespawnSignalReceived()
    {
        m_respawns++;
    }
}
