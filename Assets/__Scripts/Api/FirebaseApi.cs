/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;
using Firebase;
using UniRx;

/// <summary>
/// Main Firebase API that manages the FirebaseApp.
/// </summary>
public class FirebaseApi : IInitializable
{
    public IReadOnlyReactiveProperty<bool> Initialized { get { return m_initialized; } }

    private FirebaseApp m_app;
    private ReactiveProperty<bool> m_initialized;

    public FirebaseApi() {
        m_initialized = new ReactiveProperty<bool> (false);
    }

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith (task => {
            var dependencyStatus = task.Result;
            if ( dependencyStatus == Firebase.DependencyStatus.Available ) {

                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                m_app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                m_initialized.Value = true;
            } else {
                Debug.LogError (System.String.Format (
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                m_app = null;
            }
        });
    }
}
