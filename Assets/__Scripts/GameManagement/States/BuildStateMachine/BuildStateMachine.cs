using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildStateMachine
{
    event State.TouchHandler m_touchDetected;
}

public class BuildStateMachine : StateMachine, IBuildStateMachine
{
    public event TouchHandler m_touchDetected;

    public override void EnterState ()
    {
        base.EnterState ();
    }

    public override void UpdateActive ( double _deltaTime )
    {
        base.UpdateActive ( _deltaTime );

#if !UNITY_EDITOR
        Touch touch;
        if ( Input.touchCount < 1 || ( touch = Input.GetTouch ( 0 ) ).phase != TouchPhase.Began )
        {
            return;
        }
        if ( m_touchDetected != null ) m_touchDetected ( touch.position.x, touch.position.y );
#else
        if ( !Input.GetMouseButtonDown ( 0 ) )
        {
            return;
        }
        if ( m_touchDetected != null ) m_touchDetected ( Input.mousePosition.x, Input.mousePosition.y );
#endif
    }
}
