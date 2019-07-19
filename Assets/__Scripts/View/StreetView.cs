/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class StreetView : MonoBehaviour
{
    public bool IsOn { get; private set; }

    private const string DISSOLVE_ID = "_Dissolve";

    [SerializeField] protected float m_dissolveDuration = 2f;

    protected MeshRenderer m_renderer;

    protected void Awake()
    {
        m_renderer = GetComponent<MeshRenderer> ();
    }

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
