using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public interface IBuildMenuViewModel
{
}

[RequireComponent ( typeof ( UIFader ) )]
public class BuildMenuViewModel : MonoBehaviour, IBuildMenuViewModel
{
    private IBuildDialogState m_dialogState;
    private UIFader m_fader;

    [Inject]
    private void Construct ( IBuildDialogState _state )
    {
        m_dialogState = _state;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterCallbacks ( ( State ) m_dialogState );

        // turn off this gameobject in case it is active
        gameObject.SetActive ( false );
    }

    #region Unity Functions
    #endregion

    #region Public Functions
    #endregion

    #region Private Methods
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
