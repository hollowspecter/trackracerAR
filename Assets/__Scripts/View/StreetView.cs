/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using DG.Tweening;

/// <summary>
/// View to show and hide the street view using
/// a dissolve animation
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class StreetView : MonoBehaviour
{
    private const string DISSOLVE_ID = "_Dissolve";

    /// <summary>
    /// True if the street is currently visible,
    /// false if the street is dissolved
    /// </summary>
    public bool IsOn { get; private set; }

    /// <summary>
    /// Determines how long the dissolve takes to complete
    /// in seconds
    /// </summary>
    [SerializeField]
    protected float m_dissolveDuration = 2f;

    protected MeshRenderer m_renderer;

    protected void Awake()
    {
        m_renderer = GetComponent<MeshRenderer> ();
    }

    /// <summary>
    /// Shows or hides the street mesh.
    /// </summary>
    /// <param name="_isOn">if true, the street will show up, if false, the street will dissolve</param>
    /// <param name="_onComplete">will be called after completion of the transition</param>
    public void ToggleAppearance(bool _isOn, TweenCallback _onComplete )
    {
        // if there is nothing to turn off, skip
        if ( !IsOn && !_isOn )
        {
            m_renderer.material.SetFloat ( DISSOLVE_ID, 0f );
            _onComplete?.Invoke ();
            return;
        }
        IsOn = _isOn;

        // tween on the material
        float endvalue = _isOn ? 1.1f : 0f;

        // reset value to zero because material has changed
        if (_isOn) {
            m_renderer.material.SetFloat (DISSOLVE_ID, 0f);
        }

        m_renderer.material.DOFloat (endvalue, DISSOLVE_ID, m_dissolveDuration )
                           .OnComplete ( _onComplete );
    }
}
