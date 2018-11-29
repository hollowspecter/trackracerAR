///<summary>
///<copyright>(c) Vivien Baguio</copyright>
///http://www.vivienbaguio.com
///</summary>

using UnityEngine;
using UnityEditor;
using System.Collections;

// Create a simple move handle (Twice as big) on the 
// target object that lets you freely move the Object
// Without having the "Move" button selected

[CustomEditor(typeof(BezierHandle))]
class FreeMoveHandle : Editor
{
    void OnSceneGUI()
    {
        BezierHandle myTarget = (BezierHandle)target;
        myTarget.transform.localScale = Handles.FreeMoveHandle(myTarget.transform.localScale,
                        Quaternion.identity,
                        .5f,
                        Vector3.zero,
                        Handles.SphereCap);
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}