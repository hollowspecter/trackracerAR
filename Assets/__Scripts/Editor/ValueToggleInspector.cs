/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEditor;

/// <summary>
/// Custom inspector to override the Toggle
/// inspector to use the default inspector
/// </summary>
[CustomEditor (typeof (ValueToggle))]
public class ValueToggleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ValueToggle myTarget = (ValueToggle)target;

        DrawDefaultInspector ();
    }
}