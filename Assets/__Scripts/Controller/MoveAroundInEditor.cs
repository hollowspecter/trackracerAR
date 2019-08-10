/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Moves the attached transform on a circle to
/// simulate movement of the AR camera in editor mode
/// </summary>
public class MoveAroundInEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private float m_scale = 1f;
    private Vector3 m_pos = Vector3.zero;

    private void Start()
    {
        Debug.Log ( "Executing Move Script only in Unity Editor Mode" );
    }

    void Update()
    {
        m_pos.x = Mathf.Sin ( Time.time ) * m_scale;
        m_pos.z = Mathf.Cos ( Time.time ) * m_scale;

        transform.position = m_pos;
    }
#endif
}
