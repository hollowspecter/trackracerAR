using UnityEngine;

public interface IBuildEditorState
{
    void OnShowPreview();
    void OnCancel();
}

public class BuildEditorState : State, IBuildEditorState
{
    private IBuildStateMachine m_buildSM;

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
    }

    public override void ExitState ()
    {
        base.ExitState ();

        m_buildSM.m_touchDetected -= OnTouchDetected;
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
        m_stateMachine.TransitionToState ( StateName.BUILD_STREET_STATE );
    }

    public void OnCancel()
    {
        if ( !m_active ) return;
        Debug.Log ( "BuildEditorState: OnCancel" );
        m_stateMachine.TransitionToState ( StateName.BUILD_DIALOG_STATE );
    }

    #endregion
}
