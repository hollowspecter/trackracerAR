/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;

/// <summary>
/// Usecase to get or observe track data.
/// </summary>
public class ObserveUseCase
{
    private TracksRepository m_repository;

    public ObserveUseCase( TracksRepository _repository )
    {
        m_repository = _repository;
    }

    /// <summary>
    /// Fetches the trackdata of the given key at once.
    /// Observable will at most emit one item.
    /// </summary>
    public IObservable<TrackData> GetTrack( string _key )
    {
        return m_repository.GetTrackOnce (_key);
    }

    /// <summary>
    /// Observe the track with the given key.
    /// </summary>
    /// <returns>an Observable that pushes a new TrackData object everytime
    /// the database entry of the given key gets changed.</returns>
    public IObservable<TrackData> ObserveTrack( string _key )
    {
        return m_repository.ObserveTrack (_key);
    }
}
