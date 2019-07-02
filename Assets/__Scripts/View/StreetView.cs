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
    private const string DISSOLVE_ID = "_Dissolve";

    [SerializeField] protected float m_dissolveDuration = 2f;

    protected MeshRenderer m_renderer;

    private bool m_isOn = false;

    protected void Awake()
    {
        m_renderer = GetComponent<MeshRenderer> ();
    }

    public void ToggleAppearance(bool _isOn, TweenCallback _onComplete )
    {
        // if there is nothing to turn off, skip
        if ( !m_isOn && !_isOn )
        {
            m_renderer.material.SetFloat ( DISSOLVE_ID, 0f );
            _onComplete?.Invoke ();
            return;
        }
        m_isOn = _isOn;

        // tween on the material
        float endvalue = _isOn ? 1f : 0f;
        m_renderer.material.DOFloat ( endvalue, DISSOLVE_ID, m_dissolveDuration )
                           .OnComplete ( _onComplete );
    }
}
