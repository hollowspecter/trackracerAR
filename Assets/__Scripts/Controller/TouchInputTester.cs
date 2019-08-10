/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Component to test the <see cref="TouchInput"/> functionality
/// in an extra scenario.
/// </summary>
public class TouchInputTester : MonoBehaviour
{
    private TouchInput m_input;

    [Inject]
    public void Construct(TouchInput _input )
    {
        m_input = _input;
    }

    void Update()
    {
        m_input.Tick (Time.deltaTime);
    }
}
