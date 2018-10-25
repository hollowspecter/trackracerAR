using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class ARManager : MonoBehaviour
{
    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
    /// </summary>
    private bool m_IsQuitting = false;

    /// <summary>
    /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
    /// the application to avoid per-frame allocations.
    /// </summary>
    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane> ();
    private ReadOnlyReactiveProperty<bool> ShowSearchingUI { get; set; }

    private GameObject m_Snackbar;

    [Inject]
    public void SetUp ( Inputs _input, [Inject ( Id = "snackbar" )]RectTransform _snackbar )
    {
        // Exit app when 'back' button is pressed
        _input.Escape
               .Where ( x => x )
              .Subscribe ( _ =>
        {
            Debug.Log ( "Quitting the Application" );
            _DoQuit ();
        } );

        // Injecting the snackbar
        m_Snackbar = _snackbar.gameObject;
    }

    private void Update ()
    {
#if UNITY_ANDROID
        _UpdateApplicationLifecycle ();

        Session.GetTrackables<DetectedPlane> ( m_AllPlanes );

        _ShowHideSearchingForPlaneUI ();
#endif
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle ()
    {
        // Only allow the screen to sleep when not tracking.
        if ( Session.Status != SessionStatus.Tracking )
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if ( m_IsQuitting )
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if ( Session.Status == SessionStatus.ErrorPermissionNotGranted )
        {
            AnroidUtility.ShowAndroidToastMessage ( "Camera permission is needed to run this application." );
            m_IsQuitting = true;
            Invoke ( "_DoQuit", 0.5f );
        }
        else if ( Session.Status.IsError () )
        {
            AnroidUtility.ShowAndroidToastMessage ( "ARCore encountered a problem connecting.  Please start the app again." );
            m_IsQuitting = true;
            Invoke ( "_DoQuit", 0.5f );
        }
    }

    private void _ShowHideSearchingForPlaneUI ()
    {
        // Hide snackbar when currently tracking at least one plane.

        bool showSearchingUI = true;
        for ( int i = 0; i < m_AllPlanes.Count; i++ )
        {
            if ( m_AllPlanes [ i ].TrackingState == TrackingState.Tracking )
            {
                showSearchingUI = false;
                break;
            }
        }

        m_Snackbar.SetActive ( showSearchingUI );
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit ()
    {
        Application.Quit ();
    }
}
