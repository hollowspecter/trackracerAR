/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public interface IBuildPaintViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildPaintViewModel : MonoBehaviour
{
    private IBuildPaintState m_state;

    [Inject]
    private void Construct( IBuildPaintState _state,
                            [Inject ( Id = "ButtonCancel" )] Button _cancelButton,
                            [Inject ( Id = "ButtonDone" )] Button _doneButton )
    {
        m_state = _state;

        // Listen for state events
        GetComponent<UIFader> ().RegisterCallbacks ( ( State ) m_state );

        // register callbacks
        _cancelButton.onClick.AddListener ( OnCancelButtonPressed );
        _doneButton.onClick.AddListener ( OnDoneButtonPressed );

        // turn off this gameobject in case it is active
        gameObject.SetActive ( false );
    }

    #region Button Callbacks

    public void OnCancelButtonPressed()
    {
        m_state.OnCancel ();
    }

    public void OnDoneButtonPressed()
    {
        m_state.OnDone ();
    }

    #endregion
}
