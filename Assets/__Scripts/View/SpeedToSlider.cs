/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Takes the TouchInput and displays it on a <see cref="Slider"/>.
/// </summary>
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
