using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    private Sequence m_sequence;
    private Image [] m_images;
    private Text [] m_texts;

    private void Awake ()
    {
        // setup references
        m_images = GetComponentsInChildren<Image> ();
        m_texts = GetComponentsInChildren<Text> ();

        // setup tweens
        m_sequence = DOTween.Sequence ();
        m_sequence.SetAutoKill ( false );
        for ( int i = 0; i < m_images.Length; ++i ) m_sequence.Join ( m_images [ i ].DOFade ( 0f, 1f ).From () ); // fade out images
        for ( int i = 0; i < m_texts.Length; ++i ) m_sequence.Join ( m_texts [ i ].DOFade ( 0f, 1f ).From () ); // fade out textx
        m_sequence.OnPause ( () => { if ( m_sequence.isBackwards ) gameObject.SetActive ( false ); } ); // turn of GO on complete in backwards
    }

    public void RegisterCallbacks ( State state )
    {
        Debug.Log ( "Callbacks registered" );
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
