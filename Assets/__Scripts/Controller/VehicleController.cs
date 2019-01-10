using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private const float WAYPOINT_RADIUS = 0.3f;

    public SplineManager m_splineManager;
    public float speed = 1f;

    private List<OrientedPoint> m_waypoints;
    private int current = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_waypoints = m_splineManager.m_waypoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_waypoints[current].position) <= WAYPOINT_RADIUS/2f)
        {
            current = ( current + 1 ) % m_waypoints.Count;
        }

        transform.position = Vector3.MoveTowards ( transform.position, m_waypoints [ current ].position, speed * Time.deltaTime );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere ( transform.position, WAYPOINT_RADIUS );
    }
}
