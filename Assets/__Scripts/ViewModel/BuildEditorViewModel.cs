/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using System.Collections;
using Zenject;

public interface IBuildEditorViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildEditorViewModel : MonoBehaviour, IBuildEditorViewModel
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
        m_state.OnShowPreview ();
    }
}
