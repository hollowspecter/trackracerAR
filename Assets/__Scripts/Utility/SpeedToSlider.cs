/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpeedToSlider : MonoBehaviour
{
    private Slider m_slider;
    private TouchInput m_input;

    [Inject]
    public void Construct(TouchInput _input)
    {
        m_input = _input;
    }

    private void Awake()
    {
        m_slider = GetComponent<Slider> ();
    }

    void Update()
    {
        m_slider.value = m_input.Value;
    }
}
