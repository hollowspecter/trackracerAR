/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

/// <summary>
/// Extends a <see cref="Button"/> to fire every Update the
/// Button is in its pressed state.
/// </summary>
public class ButtonTouchInput : Button
{
    /// <summary>
    /// Fires the state of the button every frame
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsPressedx { get => m_isPressed; }

    private ReactiveProperty<bool> m_isPressed = new ReactiveProperty<bool>(false);

    IBuildPaintState m_state;

    [Inject]
    public void Construct(IBuildPaintState _state)
    {
        m_state = _state;
    }

    private void Update()
    {
        if (IsPressedx != null) { // update method gets also called in editor mode and results in nullreference exceptions
            m_isPressed.Value = (currentSelectionState == SelectionState.Pressed);
            if ( m_isPressed.Value ) {
                m_state.OnTouchDetected (Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }
}
