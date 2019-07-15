/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent (typeof (UIFader))]
public class RaceSetupUI : MonoBehaviour
{
    private IRaceSetupState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct(IRaceSetupState _state)
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

    public void StartButtonPressed()
    {
        // TODO START COUNTDOWN, and only when it is not canceled, start the race!
        m_state.OnStart ();
    }
}
