/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using Zenject;

public interface IBuildDialogViewModel
{
}

[RequireComponent ( typeof ( UIFader ) )]
public class BuildDialogViewModel : MonoBehaviour, IBuildDialogViewModel
{
    private IBuildDialogState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct ( IBuildDialogState _state )
    {
        m_state = _state;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterCallbacks ( ( State ) m_state );

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
        m_state.StartNewTrack ();
    }

    public void OnLoadTrackButtonPressed ()
    {
        m_state.LoadTrack ();
    }

    #endregion
}
