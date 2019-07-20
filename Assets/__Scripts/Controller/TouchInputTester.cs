/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
