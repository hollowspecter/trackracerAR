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

public interface IBuildLoadViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildLoadViewModel : MonoBehaviour, IBuildLoadViewModel
{
    private IBuildLoadState m_state;
    private UIFader m_fader;
    private RectTransform m_contentRect;
    private Button [] m_list = null;
    private Button m_loadButton;
    private Button m_cancelButton;

    private int m_selected;

    [Inject]
    private void Construct ( IBuildLoadState _state,
                            [Inject ( Id = "Content" )] RectTransform _contentRect,
                            [Inject ( Id = "LoadButton" )] Button _loadButton,
                            [Inject ( Id = "CancelButton" )] Button _cancelButton )
    {
        m_selected = -1;
        m_state = _state;
        ( ( State ) m_state ).m_enteredState += LoadList;
        ( ( State ) m_state ).m_exitedState += Unload;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterCallbacks ( ( State ) m_state );

        m_contentRect = _contentRect;
        m_loadButton = _loadButton;
        m_loadButton.interactable = false;
        m_loadButton.onClick.AddListener ( OnLoadPressed );
        _cancelButton.onClick.AddListener ( OnCancelPressed );

        gameObject.SetActive ( false );
    }

    private void LoadList ()
    {
        string [] fileNames = Directory.GetFiles ( SaveExtension.m_path, "*.json" );
        m_list = new Button [ fileNames.Length ];
        Button currentListItem;
        string fileName;
        for ( int i = 0; i < fileNames.Length; ++i )
        {
            int closureIndex = i;
            fileName = Path.GetFileNameWithoutExtension ( fileNames [ i ] );
            currentListItem = ( Instantiate ( Configuration.LoadItemList, m_contentRect ) as GameObject ).GetComponent<Button> ();
            currentListItem.name = fileName;
            currentListItem.GetComponentInChildren<Text> ().text = fileName;
            currentListItem.onClick.AddListener ( () => { OnItemSelected ( closureIndex ); } );
            m_list [ i ] = currentListItem;
        }
    }

    private void Unload ()
    {
        for ( int i = 0; i < m_list.Length; ++i )
        {
            Destroy ( m_list [ i ].gameObject );
        }
    }

    private void OnItemSelected ( int index )
    {
        if ( m_selected >= 0 && m_selected < m_list.Length )
        {
            m_list [ m_selected ].interactable = true;
        }
        m_selected = index;
        m_loadButton.interactable = true;
    }

    private void OnLoadPressed ()
    {
        Debug.Log ( "Load Selected " + m_list [ m_selected ].name );
        m_state.Load ( m_list [ m_selected ].name );
    }

    private void OnCancelPressed ()
    {
        Debug.Log ( "Cancel pressed" );
        m_state.CancelLoading ();
    }
}
