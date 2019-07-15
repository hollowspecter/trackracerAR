/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceStateMachine {
}

public class RaceStateMachine : StateMachine, IRaceStateMachine
{
    public override void UpdateActive( double _deltaTime )
    {
        base.UpdateActive (_deltaTime);
    }

    public override void EnterState ()
    {
        Debug.Log ( "Entered RcaeStateMachine" );
        base.EnterState ();
    }
}
