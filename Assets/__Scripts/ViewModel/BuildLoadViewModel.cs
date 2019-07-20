/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public interface IBuildLoadViewModel { }

[RequireComponent (typeof (UIFader))]
public class BuildLoadViewModel : MonoBehaviour, IBuildLoadViewModel
{
    [System.Serializable]
    public class Settings
    {
        public GameObject ListItem;
    }

    private Settings m_settings;
    private IBuildLoadState m_state;
    private UIFader m_fader;
    private RectTransform m_contentRect;
    private LoadListItemView [] m_list = null;
    private Button m_loadButton;
    private Button m_raceButton;

    private int? m_selected;

    #region Di

    [Inject]
    private void Construct( IBuildLoadState _state,
                            [Inject (Id = "Content")] RectTransform _contentRect,
                            [Inject (Id = "LoadButton")] Button _loadButton,
                            [Inject (Id = "CancelButton")] Button _cancelButton,
                            [Inject (Id = "RaceButton")] Button _raceButton,
                            Settings _settings )
    {
        m_selected = null;
        m_state = _state;
        ((State)m_state).m_enteredState += LoadList;
        ((State)m_state).m_exitedState += Unload;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ((State)m_state);

        m_contentRect = _contentRect;
        m_loadButton = _loadButton;
        m_loadButton.interactable = false;
        m_loadButton.onClick.AddListener (OnEditPressed);
        _cancelButton.onClick.AddListener (OnCancelPressed);
        m_raceButton = _raceButton;
        m_raceButton.interactable = false;
        m_raceButton.onClick.AddListener (OnRacePressed);
        m_settings = _settings;

        gameObject.SetActive (false);
    }

    #endregion

    private void LoadList()
    {
        Debug.Log ("BuildLoadViewModel: Load list");
        try {
            // Check if the directory exists.
            if ( !Directory.Exists (SaveExtension.m_path) ) {
                // if not, create the directories
                (new FileInfo (SaveExtension.m_path)).Directory.Create ();
                Debug.Log ("Created neccessary folder structure");
            }

            // load all json files in the directory
            string [] fileNames = Directory.GetFiles (SaveExtension.m_path, "*.json");

            // Create Buttons
            m_list = new LoadListItemView [fileNames.Length];
            LoadListItemView currentListItem;
            string fileName;
            for ( int i = 0; i < fileNames.Length; ++i ) {
                int closureIndex = i;
                fileName = Path.GetFileNameWithoutExtension (fileNames [i]);
                currentListItem = (Instantiate (m_settings.ListItem, m_contentRect) as GameObject).GetComponent<LoadListItemView> ();
                currentListItem.name = fileName;
                currentListItem.m_itemText.text = fileName;
                currentListItem.m_itemButton.onClick.AddListener (() => { OnItemSelected (closureIndex); });
                currentListItem.m_trashButton.onClick.AddListener (() => { OnItemDelete (closureIndex); });
                m_list [i] = currentListItem;
            }
        } catch ( System.Exception e ) {
            Debug.LogError (e);
        }
    }

    private void Unload()
    {
        for ( int i = 0; i < m_list.Length; ++i ) {
            Destroy (m_list [i].gameObject);
        }
    }

    #region Callbacks

    private void OnItemSelected( int index )
    {
        if ( m_selected != null && m_selected < m_list.Length ) {
            m_list [(int)m_selected].m_itemButton.interactable = true;
            m_list [(int)m_selected].m_trashButton.interactable = false;
        }
        m_selected = index;
        m_loadButton.interactable = true;
        m_raceButton.interactable = true;
        m_list [(int)m_selected].m_trashButton.interactable = true;
    }

    private void OnItemDelete( int index )
    {
        Destroy (m_list [index].gameObject);
        m_loadButton.interactable = false;
        m_raceButton.interactable = false;
        m_selected = null;
    }

    private void OnEditPressed()
    {
        if (m_selected == null) {
            return;
        }
        m_state.LoadAndEdit (m_list [(int)m_selected].name);
    }

    private void OnCancelPressed()
    {
        m_state.CancelLoading ();
    }

    private void OnRacePressed()
    {
        if ( m_selected == null ) {
            return;
        }
        m_state.LoadAndRace (m_list [(int)m_selected].name);
    }

    #endregion
}
