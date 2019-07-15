/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;

public interface IARStateMachine
{
}

// TODO SUMMARY
public class ARStateMachine : StateMachine, IARStateMachine
{
    //private bool m_isQuitting = false;

    #region State Methods

    public override void EnterState ()
    {
        base.EnterState ();
    }

    //    public override void UpdateActive ( double _deltaTime )
    //    {
    //#if UNITY_ANDROID
    //        UpdateApplicationLifecycle ();
    //#endif
    //    base.UpdateActive ( _deltaTime );
    //}

    #endregion

    //private void UpdateApplicationLifecycle ()
    //{
    //    // Exit the app when the 'back' button is pressed.
    //    if ( Input.GetKey ( KeyCode.Escape ) )
    //    {
    //        Application.Quit ();
    //    }

    //    // Only allow the screen to sleep when not tracking.
    //    if ( Session.Status != SessionStatus.Tracking )
    //    {
    //        const int lostTrackingSleepTimeout = 15;
    //        Screen.sleepTimeout = lostTrackingSleepTimeout;
    //    }
    //    else
    //    {
    //        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //    }

    //    if ( m_isQuitting )
    //    {
    //        return;
    //    }

    //    // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
    //    if ( Session.Status == SessionStatus.ErrorPermissionNotGranted )
    //    {
    //        AnroidUtility.ShowAndroidToastMessage ( "Camera permission is needed to run this application. Please quit the Application." );
    //        m_isQuitting = true;
    //    }
    //    else if ( Session.Status.IsError () )
    //    {
    //        AnroidUtility.ShowAndroidToastMessage ( "ARCore encountered a problem connecting.  Please start the app again." );
    //        m_isQuitting = true;
    //    }

    //}

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    //    private void _DoQuit ()
    //    {
    //#if UNITY_EDITOR
    //        Debug.LogError ( "Quitting the application does not work in Unity Editor. Please stop PlayMode." );
    //#endif
    //    Application.Quit ();
    //}
}