﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UniRx;

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

    public IObservable<TrackData> GetTrackOnce(string _key)
    {
        return m_remote.TracksReference.Child (_key)
            .GetValueAsync ()
            .ToObservable ()
            .Select (datasnapshot => JsonUtility.FromJson<TrackData> (datasnapshot.GetRawJsonValue ()));
    }

    //todo test if events get even fired
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
}
