using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine
{
    // events
    event State.TouchHandler m_touchDetected;
}

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public event TouchHandler m_touchDetected;

    //public event TouchHandler m_touchPressed;
    //public event TouchHandler m_touchReleased;
    //public event TouchHandler m_touch;

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

#if !UNITY_EDITOR
        TouchInput();
#else
        EditorInput ();
#endif
    }

    private void TouchInput()
    {
        if ( Input.touchCount < 1)
        {
            return;
        }
        m_touchDetected?.Invoke ( Input.mousePosition.x, Input.mousePosition.y );
    }

    private void EditorInput()
    {
        if ( !Input.GetMouseButtonDown ( 0 ) )
        {
            return;
        }
        m_touchDetected?.Invoke ( Input.mousePosition.x, Input.mousePosition.y );
    }
}
