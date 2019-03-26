/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public interface IBuildStreetViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildStreetViewModel : MonoBehaviour, IBuildStreetViewModel
{
    private IBuildStreetState m_state;

    [Inject]
    private void Construct( IBuildStreetState _state,
                            [Inject ( Id = "ButtonBack" )] Button _backButton,
                            [Inject ( Id = "ButtonSave" )] Button _saveButton )
    {
        m_state = _state;

        // Listen for state events
        GetComponent<UIFader> ().RegisterCallbacks ( ( State ) m_state );

        // register callbacks
        _backButton.onClick.AddListener ( OnBackButtonPressed );
        _saveButton.onClick.AddListener ( OnSaveButtonPressed );

        // turn off this gameobject in case it is active
        gameObject.SetActive ( false );
    }

    #region Button Callbacks

    private void OnBackButtonPressed()
    {
        m_state.OnBack ();
    }

    private void OnSaveButtonPressed()
    {
        m_state.OnSave ();
    }

    #endregion
}
