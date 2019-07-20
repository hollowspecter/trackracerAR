/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RaceOverUI : MonoBehaviour
{
    private IRaceOverState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct( IRaceOverState _state )
    {
        m_state = _state;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        // turn off this gameobject in case it is active
        gameObject.SetActive (false);
    }

    public void OnRetryButtonPressed()
    {
        m_state.OnRetry ();
    }

    public void OnExitButtonPressed()
    {
        m_state.OnExit ();
    }
}
