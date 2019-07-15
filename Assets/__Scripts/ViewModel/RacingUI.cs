/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent (typeof (UIFader))]
public class RacingUI : MonoBehaviour
{
    private IRacingState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct( IRacingState _state )
    {
        m_state = _state;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        // turn off this gameobject in case it is active
        gameObject.SetActive (false);
    }

    public void BackButtonPressed()
    {
        m_state.OnBack ();
    }
}
