/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseApi : IDisposable
{
    public DatabaseReference TracksReference { get; private set; }

    private IDisposable m_subscription;
    private Settings m_settings;
    private DatabaseReference m_rootReference;

    public DatabaseApi(FirebaseApi _firebaseApi, Settings _settings )
    {
        m_settings = _settings;
        m_subscription = _firebaseApi.Initialized
            .Where (init => init == true)
            .Subscribe (init => {
                Initialize ();
            });
    }

    #region Lifecycle

    private void Initialize()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (m_settings.DatabaseUrl);
        m_rootReference = FirebaseDatabase.DefaultInstance.RootReference;
        TracksReference = m_rootReference.Child (m_settings.TracksReference);
    }

    public void Dispose()
    {
        m_subscription?.Dispose ();
    }

    #endregion

    #region Settings

    [Serializable]
    public class Settings
    {
        public string DatabaseUrl;
        public string TracksReference;
    }

    #endregion
}
