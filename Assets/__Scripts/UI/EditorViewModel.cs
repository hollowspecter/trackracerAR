using UnityEngine;
using System.Collections;
using Zenject;

public interface IEditorViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class EditorViewModel : MonoBehaviour, IEditorViewModel
{
    private IBuildEditorState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct ( IBuildEditorState _state )
    {
        m_state = _state;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterCallbacks ( ( State ) m_state );
        gameObject.SetActive ( false );
    }

    public void OnDoneButtonPressed ()
    {
        m_state.OnTrackDone ();
    }
}
