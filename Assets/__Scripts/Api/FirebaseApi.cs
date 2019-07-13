/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FirebaseApi : IInitializable
{
    private Firebase.FirebaseApp m_app;

    public FirebaseApi() { }

    public void Initialize()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith (task => {
            var dependencyStatus = task.Result;
            if ( dependencyStatus == Firebase.DependencyStatus.Available ) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                m_app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError (System.String.Format (
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                m_app = null;
            }
        });

    }
}
