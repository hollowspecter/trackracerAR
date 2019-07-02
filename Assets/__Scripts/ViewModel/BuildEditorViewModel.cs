/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using System.Collections;
using Zenject;
using Baguio.Splines;

public interface IBuildEditorViewModel { }

[RequireComponent ( typeof ( UIFader ) )]
public class BuildEditorViewModel : MonoBehaviour, IBuildEditorViewModel
{
    private IBuildEditorState m_state;
    private UIFader m_fader;
    private StreetView m_streetView;

    [Inject]
    private void Construct( IBuildEditorState _state,
                             [Inject ( Id = "TrackParent" )] ISplineManager _splineMgr,
                             [Inject ( Id = "TrackParent" )] StreetView _streetView )
    {
        m_state = _state;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ( ( State ) m_state );
        gameObject.SetActive ( false );
        _state.m_onShowPreview += _splineMgr.GenerateTrack;
        m_streetView = _streetView;
    }

    public void OnCancelButtonPressed()
    {
        m_state.OnCancel ();
    }

    public void OnSaveButtonPressed ()
    {
        m_state.OnSave ();
    }

    public void OnPreviewButtonPressed()
    {
        m_streetView.ToggleAppearance ( false, ()=>
            {
                m_state.OnShowPreview ();
                m_streetView.ToggleAppearance ( true, null );
            } );
    }
}
