/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (ValueToggle))]
public class ValueToggleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ValueToggle myTarget = (ValueToggle)target;

        DrawDefaultInspector ();
    }
}