using UnityEngine;
using System.Collections;

// Linked List of TrackParts
public class TrackModel
{
    private TrackPart m_root;
    private TrackPart m_end;

    public TrackModel ( TrackPart _start )
    {
        if ( _start == null ) throw new System.NullReferenceException ( "Start Trackpart was null" );
        m_root = _start;
        m_end = _start;
    }

    public void AppendPart ( TrackPart _part )
    {
        if ( _part == null ) return;
        m_end.NextPart = _part;
        _part.PreviousPart = m_end;
        m_end = _part;
    }

    public void PrependPart ( TrackPart _part )
    {
        if ( _part == null ) return;
        _part.NextPart = m_root;
        m_root.PreviousPart = _part;
        m_root = _part;
    }

    public void GetPositioning(ArrowPosition _arrowPos, out Vector3 _position, out Quaternion _rotation)
    {
        switch (_arrowPos)
        {
            case ArrowPosition.IN: m_root.GetArrowPositioning(_arrowPos, out _position, out _rotation); break;
            case ArrowPosition.OUT: m_end.GetArrowPositioning(_arrowPos, out _position, out _rotation); break;
            default: throw new System.NotImplementedException(_arrowPos.ToString());
        }
    }

    public void GetArrowPositioning ( ArrowPosition _arrowPos, out Vector3 _position, out Quaternion _rotation )
    {
        switch (_arrowPos)
        {
            case ArrowPosition.IN: m_root.GetArrowPositioning(_arrowPos, out _position, out _rotation); break;
            case ArrowPosition.OUT: m_end.GetArrowPositioning(_arrowPos, out _position, out _rotation); break;
            default: throw new System.NotImplementedException(_arrowPos.ToString());
        }
    }
}
