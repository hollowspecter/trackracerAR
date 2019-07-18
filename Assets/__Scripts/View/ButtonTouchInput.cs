/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonTouchInput : Button
{
    IBuildPaintState m_state;

    [Inject]
    public void Construct(IBuildPaintState _state)
    {
        m_state = _state;
    }

    private void Update()
    {
        if (this.currentSelectionState == SelectionState.Pressed) {
            m_state.OnTouchDetected (Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}
