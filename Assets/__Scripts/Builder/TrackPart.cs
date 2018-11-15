using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

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

    [Inject]
    private void Construct( [Inject ( Id = "TrackRoot", Optional = false )] Transform _trackRoot )
    {
        transform.parent = _trackRoot;
        Debug.Log ( "Construct TrackPart" );
    }

    #region Unity Methods

    private void Awake ()
    {
        m_inTrans = transform.Find ( "in" );
        m_outTrans = transform.Find ( "out" );
        Assert.IsNotNull ( m_inTrans );
        Assert.IsNotNull ( m_outTrans );
    }

    #endregion

    #region Public Methods

    // if arrowpos is IN: put the OUT arrow on the given position
    // if arrowpos is OUT: put the IN arrow on the given position
    public void SetPositioning(ArrowPosition _arrowPos, Vector3 _position, Quaternion _rotation)
    {
        Vector3 difference;
        if ( _arrowPos == ArrowPosition.IN )
        {
            difference = _position - m_outTrans.position;
            transform.position += difference;
        }
        else if ( _arrowPos == ArrowPosition.OUT )
        {
            difference = _position - m_inTrans.position;
            transform.position += difference;
        }
        else
        {
            throw new System.NotImplementedException ( _arrowPos.ToString () );
        }
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

    #endregion

    #region Private Methods

    #endregion
}
