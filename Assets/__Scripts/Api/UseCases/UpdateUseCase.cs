/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;

/// <summary>
/// UseCase to update trackdata in the database;
/// </summary>
public class UpdateUseCase
{
    private TracksRepository m_repository;

    public UpdateUseCase( TracksRepository _repository )
    {
        m_repository = _repository;
    }

    /// <summary>
    /// Pushes the TrackData object to the database.
    /// If the track already exists in the DB, it will be updated,
    /// using the provided key.
    /// It also updates the TrackData with the generated key.
    /// </summary>
    /// <returns>Returns an Observable that completes, once the operation
    /// has succeeded, with the key (as a string) to the database.</returns>
    /// <summary>
    public IObservable<string> UpdateTrackToCloud( TrackData _trackData )
    {
        return m_repository.PushTrack (_trackData);
    }
}
