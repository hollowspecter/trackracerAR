/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEditor;

namespace Baguio.Splines
{
    /// <summary>
    /// Custom inspector for the spline manager to add generate
    /// buttons and instructions
    /// </summary>
    [CustomEditor ( typeof ( SplineManager ) )]
    public class SplineManagerInspector : Editor
    {
        public override void OnInspectorGUI ()
        {
            SplineManager myTarget = ( SplineManager ) target;

            EditorGUILayout.HelpBox ( "How to use:\n" +
                                        "Start off by creating empty GameObjects as Child Objects." +
                                        "The Spline will connect the GameObjects in their hierarchy order." +
                                        "Then assign a configured TrackData object or use the SampleTrack." +
                                        "Lastly, press \"Rebuild mesh!\" to create the track.",
                                      MessageType.Info,
                                      true );

            EditorGUILayout.HelpBox ( "This root object MUST be on position (0,0,0) or else the scaling will mess up the splines positio!",
                                    MessageType.Warning,
                                    true );

            if ( GUILayout.Button ( "Rebuild mesh!" ) )
            {
                myTarget.GenerateTrack ();
            }
            if ( GUILayout.Button ("Rebuild mesh using TrackData's FeaturePoints") ) {
                myTarget.GenerateTrackFromTrackData ();
            }

            DrawDefaultInspector ();
        }
    }
}
