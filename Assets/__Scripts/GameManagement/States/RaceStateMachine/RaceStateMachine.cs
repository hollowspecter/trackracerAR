/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Interface for the <see cref="RaceStateMachine"/>
/// </summary>
public interface IRaceStateMachine {

}

/// <summary>
/// Statemachine for the Racing Feature.
/// </summary>
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
