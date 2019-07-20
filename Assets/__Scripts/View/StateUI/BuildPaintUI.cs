/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public interface IBuildPaintUI { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildPaintUI : MonoBehaviour
{
    private IBuildPaintState m_state;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    [Inject]
    private void Construct( IBuildPaintState _state,
                            [Inject ( Id = "ButtonCancel" )] Button _cancelButton,
                            [Inject ( Id = "ButtonDone" )] Button _doneButton,
                            [Inject (Id = "ButtonClear")] Button _clearButton,
                            DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_state = _state;
        m_dialogBuilderFactory = _dialogBuilderFactory;

        // Listen for state events
        GetComponent<UIFader> ().RegisterStateCallbacks ( ( State ) m_state );

        // register callbacks
        _cancelButton.onClick.AddListener ( OnCancelButtonPressed );
        _doneButton.onClick.AddListener ( OnDoneButtonPressed );
        _clearButton.onClick.AddListener (OnClearButtonPressed);

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

    public void OnClearButtonPressed()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Delete all points?")
            .SetIcon (DialogBuilder.Icon.WARNING)
            .SetMessage ("Are you sure you would like to delete all the points? This is a destructive operation.")
            .AddButton ("Delete", () => m_state.Clear())
            .AddButton ("Cancel")
            .Build ();
    }

    #endregion
}
