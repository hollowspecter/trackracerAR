/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StateManager : IInitializable, ITickable
{
    private IRootStateMachine m_stateMachine;
    private Text m_debugText;

    public StateManager( IRootStateMachine _sm,
                         [Inject ( Id = "DebugStateText", Optional = true )]Text _debugText )
    {
        m_stateMachine = _sm;
        m_debugText = _debugText;
    }

    public void Initialize()
    {
        m_stateMachine.IEnterState ();
    }

    public void Tick()
    {
        m_stateMachine.IUpdateActive ( Time.deltaTime );

        if ( m_debugText != null )
            m_debugText.text = m_stateMachine.IGetCurrentStateName ();
    }
}
