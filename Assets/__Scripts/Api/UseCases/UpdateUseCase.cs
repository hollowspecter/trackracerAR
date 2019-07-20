/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUseCase
{
    private TracksRepository m_repository;

    public UpdateUseCase( TracksRepository _repository )
    {
        m_repository = _repository;
    }

    public IObservable<string> UpdateTrackToCloud( TrackData _trackData )
    {
        return m_repository.PushTrack (_trackData);
    }
}
