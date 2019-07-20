/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    private Sequence m_sequence;
    private Graphic [] m_graphics;

    protected virtual void Awake ()
    {
        // setup references
        m_graphics = GetComponentsInChildren<Graphic> ();

        // setup tweens
        m_sequence = DOTween.Sequence ();
        m_sequence.SetAutoKill ( false );
        for ( int i = 0; i < m_graphics.Length; ++i ) m_sequence.Join ( m_graphics [ i ].DOFade ( 0f, 0.5f ).From () ); // fade out graphics
    }

    public void RegisterStateCallbacks ( State state )
    {
        state.m_enteredState += Activate;
        state.m_exitedState += Deactivate;
    }

    public void RegisterStateCallbacks(State enterState, State exitState )
    {
        enterState.m_enteredState += Activate;
        exitState.m_exitedState += Deactivate;
    }

    public void Activate ()
    {
        gameObject.SetActive ( true );
        m_sequence.Rewind ();
        m_sequence.PlayForward ();
    }

    public void Deactivate ()
    {
        gameObject.SetActive ( false );
    }
}
