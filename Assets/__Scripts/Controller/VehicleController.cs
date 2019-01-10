using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{
    private const float WAYPOINT_RADIUS = 0.3f;

    public float m_maxSpeed = 3f;
    public SplineManager m_splineManager;
    public float m_speedPercentage = 0f;
    public Slider m_slider;
    public float m_maxDegrees = 30;

    private List<OrientedPoint> m_waypoints;
    private int current = 1;

    void Start()
    {
        m_waypoints = m_splineManager.m_waypoints;
    }

    void Update()
    {
        Speed ();
        Move ();
    }

    private void Speed()
    {
        m_speedPercentage = m_slider.value = Input.GetAxis ( "Vertical" );
    }

    private void Move()
    {
        if ( Vector3.Distance ( transform.position, m_waypoints [ current ].position ) <= WAYPOINT_RADIUS / 2f )
        {
            current = ( current + 1 ) % m_waypoints.Count;
        }
        transform.rotation = Quaternion.RotateTowards ( transform.rotation, m_waypoints [ current ].rotation, m_speedPercentage*m_maxDegrees);
        transform.position = Vector3.MoveTowards ( transform.position, m_waypoints [ current ].position, m_maxSpeed * m_speedPercentage * Time.deltaTime );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere ( transform.position, WAYPOINT_RADIUS );
    }
}
