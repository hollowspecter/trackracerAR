/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObserveUseCase
{
    private TracksRepository m_repository;

    public ObserveUseCase( TracksRepository _repository )
    {
        m_repository = _repository;
    }

    public IObservable<TrackData> GetTrack( string _key )
    {
        return m_repository.GetTrackOnce (_key);
    }

    public IObservable<TrackData> ObserveTrack( string _key )
    {
        return m_repository.ObserveTrack (_key);
    }
}
