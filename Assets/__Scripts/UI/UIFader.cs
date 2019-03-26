/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using System.Collections;
/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
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
        m_sequence.OnPause ( () => { if ( m_sequence.isBackwards ) gameObject.SetActive ( false ); } ); // turn of GO on complete in backwards
    }

    public void RegisterCallbacks ( State state )
    {
        state.m_enteredState += Activate;
        state.m_exitedState += Deactivate;
    }

    private void Activate ()
    {
        gameObject.SetActive ( true );
        m_sequence.PlayForward ();
    }

    private void Deactivate ()
    {
        m_sequence.PlayBackwards ();
    }
}
