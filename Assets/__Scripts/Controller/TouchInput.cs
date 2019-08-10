/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Tickable class that simulates the behaviour of the unity
/// axis input, to use on button or mouse press.
/// </summary>
public class TouchInput
{
    /// <summary>
    /// Clamped between 0 and 1
    /// </summary>
    public float Value { get; private set; }

    private Settings m_settings;

    public TouchInput( Settings _settings )
    {
        m_settings = _settings;
    }

    #region Public methods

    /// <summary>
    /// Call this in Update populate the Value with
    /// either Mouseclick (in Editor) or Touch Input
    /// (on device)
    /// </summary>
    /// <param name="_deltaTime"></param>
    public void Tick( double _deltaTime )
    {
#if !UNITY_EDITOR
        Touch(_deltaTime);
#else
        Editor (_deltaTime);
#endif
    }

    /// <summary>
    /// Sets the value. Value will be clamped between 0 and 1
    /// </summary>
    /// <param name="_value"></param>
    public void SetValue( float _value )
    {
        Value = Mathf.Clamp01 (_value);
    }

    #endregion

    #region Private methods

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

    #endregion

    [System.Serializable]
    public class Settings
    {
        [Tooltip ("speed in units/sec the input falls towards 0")]
        public float Gravity;
        [Tooltip ("speed in units/sec the input reaches 1.0")]
        public float Sensitivity;
    }
}
