using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public interface IBuildMenuViewModel
{
    void Activate ();
    void Deactivate ();
}

public class BuildMenuViewModel : MonoBehaviour, IBuildMenuViewModel
{
    [Inject ( Id = "Canvas" )]
    private Transform m_canvas;
    [Inject]
    private IBuildDialogState m_dialogState;

    private Image m_bg;
    private Color m_origColor;

    private void Awake ()
    {
        m_bg = GetComponent<Image> ();
        m_origColor = m_bg.color;
        transform.SetParent ( m_canvas, false );
        gameObject.SetActive ( false );
    }

    public void Activate ()
    {
        gameObject.SetActive ( true );
        FadeIn ();
    }

    public void Deactivate ()
    {
        FadeOut ();
    }

    #region Private Methods

    private void FadeIn ()
    {
        m_bg.DOFade ( 0f, 1f ).From ();
    }

    private void FadeOut ()
    {
        m_bg.DOFade ( 0f, 1f ).OnComplete ( () =>
        {
            m_bg.color = m_origColor;
            gameObject.SetActive ( false );
        } );
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
