/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using UnityEditor;

namespace Baguio.Splines
{
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

            DrawDefaultInspector ();
        }
    }
}
