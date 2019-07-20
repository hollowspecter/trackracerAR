﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class PaintHelpView : MonoBehaviour
{
    public Animator m_animator;
    public ButtonTouchInput m_buttonTouchInput;
    public Graphic [] m_graphics;
    public Graphic m_shadow;

    private IDisposable m_subscription;
    private Sequence m_sequence;

    private void Awake()
    {
        m_sequence = DOTween.Sequence ();
        m_sequence.SetAutoKill (false);
        m_sequence.Join (m_shadow.DOFade (0, 1f));
        for ( int i = 0; i < m_graphics.Length; ++i ) {
            m_sequence.Join (m_graphics [i].DOFade (0f, 1f))
                .Pause ()
                .OnComplete (() => m_animator.SetTrigger ("Stop"));
        }
    }

    private void FadeOut()
    {
        m_subscription?.Dispose ();
        if (!m_sequence.IsPlaying()) {
            m_sequence.PlayForward ();
        }
    }

    private void OnEnable()
    {
        for ( int i = 0; i < m_graphics.Length; ++i ) {
            m_graphics [i].color = Color.white;
        }
        m_shadow.color = new Color (0f, 0f, 0f, 0.7f);
        m_animator.SetTrigger ("Start");
        m_sequence.Rewind ();
        m_subscription = m_buttonTouchInput.IsPressedx
            .Where(isPressed => isPressed)
            .Delay(TimeSpan.FromMilliseconds(250))
            .Subscribe (_ => FadeOut ());
    }

    private void OnDisable()
    {
        m_subscription?.Dispose ();
    }
}