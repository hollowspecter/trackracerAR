/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundInEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private float scale = 1f;
    private Vector3 pos = Vector3.zero;

    private void Start()
    {
        Debug.Log ( "Executing Move Script only in Unity Editor Mode" );
    }

    void Update()
    {
        pos.x = Mathf.Sin ( Time.time ) * scale;
        pos.z = Mathf.Cos ( Time.time ) * scale;

        transform.position = pos;
    }
#endif
}
