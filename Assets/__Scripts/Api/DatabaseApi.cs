/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UniRx;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

/// <summary>
/// Initializes the firebase database connection
/// automatically and provides the neccessary
/// database references for the repositories.
/// </summary>
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
