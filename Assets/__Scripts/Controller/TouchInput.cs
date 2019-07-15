/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TouchInput
{
    /// <summary>
    /// Clamped between 0 and 1
    /// </summary>
    public float Value { get; private set; }

    private Settings m_settings;

    public TouchInput( Settings _settings)
    {
        m_settings = _settings;
    }

    public void Tick(float _deltaTime)
    {
#if !UNITY_EDITOR
        Touch(_deltaTime);
#else
        Editor (_deltaTime);
#endif
    }

    private void Touch( float _deltaTime )
    {
        if ( Input.touchCount < 1 ) {
            SlowDown (_deltaTime);
            return;
        }
        SpeedUp (_deltaTime);
    }

    private void Editor( float _deltaTime )
    {
        if ( !Input.GetMouseButton (0) ) {
            SlowDown (_deltaTime);
            return;
        }
        SpeedUp (_deltaTime);
    }

    private void SpeedUp( float _deltaTime )
    {
        Value = Mathf.Clamp01 (Value + Time.deltaTime * m_settings.Sensitivity);
    }

    private void SlowDown( float _deltaTime )
    {
        Value = Mathf.Clamp01 (Value - Time.deltaTime * m_settings.Gravity);
    }

    [System.Serializable]
    public class Settings
    {
        [Tooltip ("speed in units/sec the input falls towards 0")]
        public float Gravity;
        [Tooltip ("speed in units/sec the input reaches 1.0")]
        public float Sensitivity;
    }
}
