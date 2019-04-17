/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using Zenject;

public interface IBuildEditorState
{
    event State.InputActionHandler m_onShowPreview;
    void OnShowPreview();
    void OnCancel();
    void OnSave();
}

public class BuildEditorState : State, IBuildEditorState
{
    public event InputActionHandler m_onShowPreview;

    private IBuildStateMachine m_buildSM;
    private ITrackBuilderManager m_trackBuilder;

    #region DI

    [Inject]
    private void Construct(ITrackBuilderManager _trackBuilder )
    {
        m_trackBuilder = _trackBuilder;
    }

    #endregion

    #region State Methods

    protected override void Initialise ()
    {
        base.Initialise ();
        m_buildSM = m_stateMachine as IBuildStateMachine;
    }

    public override void EnterState ()
    {
        base.EnterState ();

        m_buildSM.m_touchDetected += OnTouchDetected;

        // Instantiate the Feature Points
        m_trackBuilder.InstantiateFeaturePoints ( ref m_buildSM.CurrentTrackData.m_featurePoints );
    }

    public override void ExitState ()
    {
        base.ExitState ();

        m_buildSM.m_touchDetected -= OnTouchDetected;

        m_buildSM.CurrentTrackData.m_featurePoints = m_trackBuilder.GetFeaturePoints ();
    }

    #endregion

    #region Building Functions

    private void OnTouchDetected ( float x, float y )
    {

    }

    #endregion

    #region UI Callbacks

    public void OnShowPreview()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildEditorState: OnShowPreview" );
        m_onShowPreview?.Invoke ();
    }

    public void OnCancel()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildEditorState: OnCancel" );
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    public void OnSave()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildEditorState: OnSave" );
        m_stateMachine.TransitionToState ( StateName.BUILD_SAVE_STATE );
    }

    #endregion
}
