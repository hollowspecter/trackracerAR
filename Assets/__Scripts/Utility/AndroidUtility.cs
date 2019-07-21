﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Native Android Utility class
/// </summary>
public class AndroidUtility
{
    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    public static void ShowAndroidToastMessage ( string message )
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" );
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject> ( "currentActivity" );

        if ( unityActivity != null )
        {
            AndroidJavaClass toastClass = new AndroidJavaClass ( "android.widget.Toast" );
            unityActivity.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject> ( "makeText", unityActivity,
                    message, 0 );
                toastObject.Call ( "show" );
            } ) );
        }
    }
}
