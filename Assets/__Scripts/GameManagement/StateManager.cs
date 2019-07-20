/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Has a reference to the <see cref="IRootStateMachine"/> to kick it off
/// at the start of the application.
/// Receives init and tick calls from Zenject using <see cref="IInitializable"/>
/// and <see cref="ITickable"/>
/// </summary>
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
