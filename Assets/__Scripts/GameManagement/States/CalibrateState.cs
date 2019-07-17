/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARExtensions;
using Zenject;

public interface ICalibrateState
{
    void DoStart();
    void Restart();
}

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
