using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public interface IBuildMenuViewModel
{
}

public class BuildMenuViewModel : MonoBehaviour, IBuildMenuViewModel
{
    private IBuildDialogState m_dialogState;

    private Sequence m_sequence;
    private Image [] m_images;
    private Text [] m_texts;

    [Inject]
    private void Construct ( IBuildDialogState _state )
    {
        m_dialogState = _state;

        // Listen for state events
        ( ( State ) m_dialogState ).m_enteredState += Activate;
        ( ( State ) m_dialogState ).m_exitedState += Deactivate;

        // turn off this gameobject in case it is active
        gameObject.SetActive ( false );
    }


    #region Unity Functions

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

    #endregion

    #region Public Functions
    #endregion

    #region Private Methods

    private void Activate ()
    {
        gameObject.SetActive ( true );
        m_sequence.PlayForward ();
    }

    private void Deactivate ()
    {
        Debug.Log ( "Deactivate Menu View" );
        m_sequence.PlayBackwards ();
    }

    #endregion

    #region Button Callbacks

    public void OnNewTrackButtonPressed ()
    {
        m_dialogState.StartNewTrack ();
    }

    public void OnLoadTrackButtonPressed ()
    {
        m_dialogState.LoadTrack ();
    }

    #endregion
}
