/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

/// <summary>
/// Interface for <see cref="CalibrateState"/>
/// </summary>
public interface ICalibrateState
{
    /// <summary>
    /// Transitions to <see cref="BuildStateMachine"/>
    /// </summary>
    void DoStart();

    /// <summary>
    /// Restarts the AR Session and resets the calibration progress
    /// </summary>
    void Restart();
}

/// <summary>
/// Resets the AR session to calibrate ARFoundation
/// </summary>
public class CalibrateState : State, ICalibrateState
{
    private ARSession m_arSession;

    #region DI

    [Inject]
    private void Construct( ARSession _arSession )
    {
        m_arSession = _arSession;
    }

    #endregion

    public override void EnterState()
    {
        base.EnterState ();
        Debug.Log ("CalibrateState entered!");
        m_arSession.Reset ();
    }

    public void DoStart()
    {
        if ( !Active ) return;
        m_stateMachine.TransitionToState (StateName.AR_SM);
    }

    public void Restart()
    {
        if ( !Active ) return;
        EnterState ();
    }
}
