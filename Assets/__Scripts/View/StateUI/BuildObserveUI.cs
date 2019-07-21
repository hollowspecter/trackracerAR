/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Interface for <see cref="BuildObserveUI"/>
/// </summary>
public interface IBuildObserveUI { }

/// <summary>
/// Manages the UI for the <see cref="BuildObserveState"/>
/// </summary>
[RequireComponent (typeof (UIFader))]
public class BuildObserveUI : MonoBehaviour, IBuildObserveUI
{
    public Button m_backButton;
    public Button m_editCopyButton;
    public Button m_raceButton;

    private IBuildObserveState m_state;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    [Inject]
    private void Construct(IBuildObserveState _state,
                           DialogBuilder.Factory _dialogBuilderFactory )
    {
        m_state = _state;
        m_dialogBuilderFactory = _dialogBuilderFactory;

        m_backButton.onClick.AddListener (OnBackButtonPressed);
        m_editCopyButton.onClick.AddListener (OnEditCopyButtonPressed);
        m_raceButton.onClick.AddListener (OnRaceButtonPressed);

        UIFader fader = GetComponent<UIFader> ();
        fader.RegisterStateCallbacks ((State)m_state);
        gameObject.SetActive (false);
    }

    private void OnBackButtonPressed()
    {
        m_state.Back ();
    }

    private void OnEditCopyButtonPressed()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Create a copy and edit?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .SetMessage ("Would you like to create a copy of this track and edit it?")
            .AddButton ("Yes", () => m_state.EditCopy ())
            .AddButton ("No")
            .Build ();
    }

    private void OnRaceButtonPressed()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Race?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .SetMessage ("Would you like to race the current status of the track?")
            .AddButton ("Yes", () => m_state.Race ())
            .AddButton ("No")
            .Build ();
    }
}
