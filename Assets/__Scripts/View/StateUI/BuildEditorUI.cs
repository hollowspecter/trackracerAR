/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;
using Baguio.Splines;

/// <summary>
/// Interface for <see cref="BuildEditorUI"/>
/// </summary>
public interface IBuildEditorUI { }

/// <summary>
/// Manages the UI Screen for <see cref="BuildEditorState"/>
/// </summary>
[RequireComponent ( typeof ( UIFader ) )]
public class BuildEditorUI : MonoBehaviour, IBuildEditorUI
{
    private IBuildEditorState m_state;
    private UIFader m_fader;
    private StreetView m_streetView;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    [Inject]
    private void Construct( IBuildEditorState _state,
                             [Inject (Id = "TrackParent")] ISplineManager _splineMgr,
                             [Inject (Id = "TrackParent")] StreetView _streetView,
                             DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_state = _state;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ( ( State ) m_state );
        gameObject.SetActive ( false );
        m_streetView = _streetView;
        m_dialogBuilderFactory = _dialogBuilderFactory;
    }

    public void OnCancelButtonPressed()
    {
        m_state.OnCancel ();
    }

    public void OnSaveButtonPressed ()
    {
        m_state.OnSave ();
    }

    public void OnRaceButtonPressed()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Ready to race?")
            .SetMessage ("Are you ready to start the race?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("Cancel")
            .AddButton ("Yes", m_state.OnRace)
            .Build ();
    }

    public void OnShareButtonPressed()
    {
        string key = m_state.OnShare ();
        if ( string.IsNullOrWhiteSpace (key) ) {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Not uploaded yet!")
                .SetIcon (DialogBuilder.Icon.INFO)
                .SetMessage ("If you would like to share this track, go to the save menu and upload this track to the cloud.")
                .Build ();
        } else {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Success!")
                .SetMessage (string.Format ("The key to the track\n{0}\nwas copied into your clipboard. Share it with your friends now!", key))
                .Build ();
        }
    }
}
