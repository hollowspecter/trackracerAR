/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using UniRx;
using Zenject;
using Firebase.Auth;
using UnityEngine;

public class AuthApi : IDisposable
{
    public bool SignedIn { get { return m_auth?.CurrentUser != null; } }

    private FirebaseAuth m_auth;

    private BehaviorSubject<FirebaseAuth> m_authStateSubject;
    private AsyncSubject<FirebaseUser> m_userSubject = null;
    private IDisposable m_subscription;

    public AuthApi(FirebaseApi firebaseApi)
    {
        /*
            Behavior Subject:
            Upon subscription, emits the most recent item then continue to
            emit item emitted by the source Observable.
         */
        m_authStateSubject = new BehaviorSubject<FirebaseAuth> (null);

        // Automatically sign in once firebase is initialized
        m_subscription = firebaseApi.Initialized
            .Where (init => init == true)
            .Subscribe (init => {
                Initialize ();
                signInAnonymously ();
            });
    }

    public void Initialize()
    {
        m_auth = FirebaseAuth.DefaultInstance;

        /*
            AuthStateListener fires:
            * Right after the listener has been registered
            * When a user is signed in
            * When the current user is signed out
            * When the current user changes
        */
        m_auth.StateChanged += (o, e) => m_authStateSubject.OnNext (m_auth);
    }

    public void Dispose()
    {
        m_subscription.Dispose ();
    }

    /// <summary>
    /// It emits FirebaseUser object depends on user's signed-in state.
    /// It will emit null values if no user is signed in.
    /// </summary>
    public IObservable<FirebaseUser> observeCurrentUser()
    {
        return m_authStateSubject.Select (auth => auth?.CurrentUser);
    }

    /// <summary>
    /// Emits the FirebaseUser on successful authentication.
    /// Emits an Exception if unsuccessful.
    /// </summary>
    public IObservable<FirebaseUser> signInAnonymously()
    {
        if (m_userSubject != null) {
            return m_userSubject;
        }

        /*  Async Subject:
            Emits the last item emitted by the source Observable after it's completes emission. */
        m_userSubject = new AsyncSubject<FirebaseUser> ();

        if (m_auth.CurrentUser != null) {
            Debug.LogFormat ("User signed in successfully: {0} ({1})",
                m_auth.CurrentUser.DisplayName, m_auth.CurrentUser.UserId);
            m_userSubject.OnNext (m_auth.CurrentUser);
            // late observers get the last cached item too!
            m_userSubject.OnCompleted ();
        } else {
            m_auth.SignInAnonymouslyAsync ().ContinueWith (task => {
                if ( task.IsCanceled ) {
                    Debug.LogError ("SignInAnonymouslyAsync was canceled.");
                    m_userSubject.OnError (task.Exception);
                    return;
                }
                if ( task.IsFaulted ) {
                    Debug.LogError ("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    m_userSubject.OnError (task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.LogFormat ("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                m_userSubject.OnNext (newUser);
                m_userSubject.OnCompleted ();
            });
        }

        return m_userSubject;
    }
}
