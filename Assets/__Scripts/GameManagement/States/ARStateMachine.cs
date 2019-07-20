/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

/// <summary>
/// Interface for the <see cref="ARStateMachine"/>
/// </summary>
public interface IARStateMachine
{
}

/// <summary>
/// Parent state machine for build and race state machines
/// </summary>
public class ARStateMachine : StateMachine, IARStateMachine
{
    #region State Methods

    public override void EnterState ()
    {
        base.EnterState ();
        Debug.Log ("ARStateMachine entered!");
    }

    #endregion
}