/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
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

    public void Tick(double _deltaTime)
    {
#if !UNITY_EDITOR
        Touch(_deltaTime);
#else
        Editor (_deltaTime);
#endif
    }

    public void ResetValue()
    {
        Value = 0f;
    }

    public void SetValue(float value )
    {
        Value = Mathf.Clamp01 (value);
    }

    private void Touch( double _deltaTime )
    {
        if ( Input.touchCount < 1 ) {
            SlowDown (_deltaTime);
            return;
        }
        SpeedUp (_deltaTime);
    }

    private void Editor( double _deltaTime )
    {
        if ( !Input.GetMouseButton (0) ) {
            SlowDown (_deltaTime);
            return;
        }
        SpeedUp (_deltaTime);
    }

    private void SpeedUp( double _deltaTime )
    {
        Value = Mathf.Clamp01 (Value + Time.deltaTime * m_settings.Sensitivity);
    }

    private void SlowDown( double _deltaTime )
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
