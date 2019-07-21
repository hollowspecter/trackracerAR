/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Interface for <see cref="BuildDialogUI"/>
/// </summary>
public interface IBuildDialogUI { }

/// <summary>
/// Manages the UI screen for the <see cref="BuildDialogState"/>
/// </summary>
[RequireComponent ( typeof ( UIFader ) )]
public class BuildDialogUI : MonoBehaviour, IBuildDialogUI
{
    public PopupView m_impress;
    private IBuildDialogState m_state;
    private UIFader m_fader;

    [Inject]
    private void Construct ( IBuildDialogState _state )
    {
        m_state = _state;

        // Listen for state events
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ( ( State ) m_state );

        // turn off this gameobject in case it is active
        gameObject.SetActive ( false );
        m_impress.gameObject.SetActive (false);
    }

    #region Button Callbacks

    public void OnNewTrackButtonPressed ()
    {
        m_state.StartNewTrack ();
    }

    public void OnLoadTrackButtonPressed ()
    {
        m_state.LoadTrack ();
    }

    public void OnDownloadTrackPressed()
    {
        m_state.ObserveTrack ();
    }

    public void OnRecalibratePressed()
    {
        m_state.Recalibrate ();
    }

    public void OnImpressButtonPressed()
    {
        m_impress.Activate ();
    }

    #endregion
}
