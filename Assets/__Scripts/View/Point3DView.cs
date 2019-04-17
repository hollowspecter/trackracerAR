using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Point3DView : MonoBehaviour
{
    [SerializeField]
    protected Material m_material;
    [SerializeField]
    protected Color m_unselectedColor = Color.white;
    [SerializeField]
    protected Color m_selectedColor = Color.magenta;

    private Vector3 m_screenPoint;
    private Vector3 m_offset;
    private SignalBus m_signalBus;

    [Inject]
    private void Construct(SignalBus _signalBus )
    {
        m_signalBus = _signalBus;
    }

    private void Awake()
    {
        m_material = GetComponent<MeshRenderer> ().material;
        m_material.color = m_unselectedColor;
    }

    private void OnMouseDown()
    {
        Select ();
        m_screenPoint = Camera.main.WorldToScreenPoint ( gameObject.transform.position );
        m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint ( new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z ) );
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z );
        Vector3 curPosition = Camera.main.ScreenToWorldPoint ( curScreenPoint ) + m_offset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        Deselect ();
        m_signalBus.Fire ( new FeaturePointMovedSignal () );
    }

    public void Select()
    {
        m_material.color = m_selectedColor;
    }

    public void Deselect()
    {
        m_material.color = m_unselectedColor;
    }
}
