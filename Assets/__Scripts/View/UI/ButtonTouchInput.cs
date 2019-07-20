/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class ButtonTouchInput : Button
{
    public ReactiveProperty<bool> IsPressedx { get; private set; }

    IBuildPaintState m_state;

    [Inject]
    public void Construct(IBuildPaintState _state)
    {
        m_state = _state;
        IsPressedx = new ReactiveProperty<bool> (false);
    }

    private void Update()
    {
        if (IsPressedx != null) { // update method gets also called in editor moce
            IsPressedx.Value = (currentSelectionState == SelectionState.Pressed);
            if ( IsPressedx.Value ) {
                m_state.OnTouchDetected (Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }
}
