/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using Firebase.Database;
using UniRx;

/// <summary>
/// Manages the communication to the database regarding track data.
/// </summary>
public class TracksRepository
{
    private DatabaseApi m_remote;

    public TracksRepository(DatabaseApi databaseApi)
    {
        m_remote = databaseApi;
    }

    /// <summary>
    /// Pushes the TrackData object to the remote database.
    /// If the track already exists in the DB, it will be updated,
    /// using the provided key.
    /// It also updates the TrackData with the generated key.
    /// </summary>
    public IObservable<string> PushTrack(TrackData _trackData)
    {
        string key;
        if (String.IsNullOrWhiteSpace(_trackData.m_dbKey)) {
            key = m_remote.TracksReference.Push ().Key;
            _trackData.m_dbKey = key;
        } else {
            key = _trackData.m_dbKey;
        }
        string json = JsonUtility.ToJson (_trackData);
        var task = m_remote.TracksReference.Child (key).SetRawJsonValueAsync (json);
        return task.ToObservable ().Select (unit => key);
    }

    /// <summary>
    /// Fetches the trackdata of the given key at once.
    /// </summary>
    public IObservable<TrackData> GetTrackOnce(string _key)
    {
        return m_remote.TracksReference.Child (_key)
            .GetValueAsync ()
            .ToObservable ()
            .Select (datasnapshot => JsonUtility.FromJson<TrackData> (datasnapshot.GetRawJsonValue ()));
    }

    /// <summary>
    /// Observe the track with the given key.
    /// </summary>
    /// <returns>an Observable that pushes a new TrackData object everytime
    /// the database entry of the given key gets changed.</returns>
    public IObservable<TrackData> ObserveTrack(string _key )
    {
        DatabaseReference reference = m_remote.TracksReference.Child (_key);

        return Observable.FromEvent<EventHandler<ValueChangedEventArgs>, ValueChangedEventArgs> (
            handler => {
                EventHandler<ValueChangedEventArgs> _handler = ( sender, e ) => handler (e);
                return _handler;
             },
            h => reference.ValueChanged += h,
            h => reference.ValueChanged -= h)
            //.Where (args => args.DatabaseError == null) don't filter errors out, but handle them later in subscription
            .Select (args => JsonUtility.FromJson<TrackData> (args.Snapshot.GetRawJsonValue ()));
    }

    /// <summary>
    /// Checks if a key exists in the firebase.
    /// </summary>
    /// <returns>an Observable that emits true if key exists, or else if doesn't or problems occured</returns>
    public IObservable<bool> EvaluateKey(string _key )
    {
        return m_remote.TracksReference.Child (_key)
            .GetValueAsync ()
            .ToObservable ()
            .Select (datasnapshot => datasnapshot.Exists);
    }
}
