using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TrackPart : MonoBehaviour
{
    private TrackPart m_previousPart;
    private TrackPart m_nextPart;

    private Transform m_inTrans;
    private Transform m_outTrans;

    #region Properties

    public TrackPart PreviousPart
    {
        get
        {
            return m_previousPart;
        }

        set
        {
            m_previousPart = value;
        }
    }

    public TrackPart NextPart
    {
        get
        {
            return m_nextPart;
        }

        set
        {
            m_nextPart = value;
        }
    }

    #endregion

    private void Awake ()
    {
        m_inTrans = transform.Find ( "in" );
        m_outTrans = transform.Find ( "out" );
        Assert.IsNotNull ( m_inTrans );
        Assert.IsNotNull ( m_outTrans );
    }

    public void GetArrowPositioning ( ArrowPosition _arrowPos, out Vector3 _position, out Quaternion _rotation )
    {
        Transform trans = null;
        switch ( _arrowPos )
        {
            case ArrowPosition.IN: trans = m_inTrans; break;
            case ArrowPosition.OUT: trans = m_outTrans; break;
            default: throw new System.NotImplementedException ( _arrowPos.ToString () );
        }

        _position = trans.position;
        Vector3 direction = trans.TransformDirection ( trans.forward );
        _rotation = Quaternion.LookRotation ( direction );
    }
}
