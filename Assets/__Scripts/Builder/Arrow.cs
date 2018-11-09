using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum ArrowPosition
{
    IN, OUT
}

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private ArrowPosition m_arrowPosition;

    [Inject]
    private void Construct ( IBuildEditorState _state )
    {
        switch ( m_arrowPosition )
        {
            case ArrowPosition.IN: _state.m_inArrowPositionChanged += Reposition; break;
            case ArrowPosition.OUT: _state.m_outArrowPositionChanged += Reposition; break;
            default: throw new System.NotImplementedException ( m_arrowPosition.ToString () );
        }

        ( ( State ) _state ).m_enteredState += Activate;
        ( ( State ) _state ).m_exitedState += Deactivate;
    }

    private void Activate ()
    {
        Debug.Log ( "Arrow: Activate" );
        gameObject.SetActive ( true );
    }

    private void Deactivate ()
    {
        Debug.Log ( "Arrow: Deactivate" );
        gameObject.SetActive ( false );
    }

    private void Reposition ( Vector3 _position, Quaternion _rotation )
    {
        Debug.Log ( "Arrow: Reposition" );
        transform.position = _position;
        transform.rotation = _rotation;
    }
}
