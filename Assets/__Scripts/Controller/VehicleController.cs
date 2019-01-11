using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    private const float WAYPOINT_RADIUS = 0.3f;
    private const bool USE_PHSYICS = true;

    // TODO use DI
    public SplineManager m_splineManager;
    public Slider m_slider;

    [SerializeField]
    private float m_maxSpeed = 20f;
    [SerializeField]
    private float m_speedPercentage = 0f;
    [SerializeField]
    private float m_maxDegrees = 30;

    private List<OrientedPoint> m_waypoints;
    private int m_currWaypointIndex = 1;
    private Rigidbody m_rigidBody;

    #region Unity Functions

    void Start()
    {
        m_waypoints = m_splineManager.m_waypoints;
        m_rigidBody = GetComponent<Rigidbody> ();

        // set him to the first waypoint
        transform.position = m_waypoints [ 0 ].position;
        transform.rotation = m_waypoints [ 0 ].rotation;
    }

    void Update()
    {
        Speed ();
        UpdateWaypoint ();
        if (!USE_PHSYICS) Move ();
    }

    private void FixedUpdate()
    {
        if ( USE_PHSYICS ) MoveWithPhysics ();
    }

    #endregion

    #region Private Functions

    private void Speed()
    {
        m_speedPercentage = m_slider.value = Input.GetAxis ( "Vertical" );
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards ( transform.position, m_waypoints [ m_currWaypointIndex ].position, m_maxSpeed * m_speedPercentage * Time.deltaTime );
        transform.rotation = Quaternion.RotateTowards ( transform.rotation, m_waypoints [ m_currWaypointIndex ].rotation, m_speedPercentage*m_maxDegrees);
    }

    private void MoveWithPhysics()
    {
        m_rigidBody.MovePosition ( Vector3.MoveTowards ( transform.position, m_waypoints [ m_currWaypointIndex ].position, m_maxSpeed * m_speedPercentage * Time.deltaTime ) );
        m_rigidBody.MoveRotation ( Quaternion.RotateTowards ( transform.rotation, m_waypoints [ m_currWaypointIndex ].rotation, m_speedPercentage * m_maxDegrees ) );
    }

    private void UpdateWaypoint()
    {
        if ( Vector3.Distance ( transform.position, m_waypoints [ m_currWaypointIndex ].position ) <= WAYPOINT_RADIUS / 2f )
        {
            m_currWaypointIndex = ( m_currWaypointIndex + 1 ) % m_waypoints.Count;
        }
    }

    #endregion

    #region Debug Functions

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere ( transform.position, WAYPOINT_RADIUS );
    }

    #endregion
}
