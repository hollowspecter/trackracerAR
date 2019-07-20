/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Baguio.Splines;
using Zenject;

/// <summary>
/// The vehicles follow the tracks waypoints.
/// 
/// There are two different acceleration modes:
/// 1. movement by translation of the transform (though deprecated)
/// 2. movement by translation of the rigidbody
/// Due to the fact that the respawn mechanism is triggered
/// using Unitys physics engine, the latter movement mode is
/// required.
/// 
/// There are also two different respawn modes implemented:
/// 1. respawning by restore the vehicle to its original status
/// 2. respawning by destroying the old vehicle and spawning a new one
/// Because the first mode was difficult to implement due to
/// unpredictable physics interactions, the latter is used here as well.
/// </summary>
[RequireComponent ( typeof ( Rigidbody ) )]
[RequireComponent ( typeof ( HingeJoint ) )]
public class VehicleController : MonoBehaviour
{
    private static readonly bool RESPAWN_USING_PREFAB = true;

    private float m_speedPercentage = 0f;
    private List<OrientedPoint> m_waypoints;
    private int m_currWaypointIndex = 0;
    private Rigidbody m_rigidBody;
    private Transform m_connectedMesh;
    private bool m_respawning = false;
    private Transform m_meshAnchor;
    private Factory m_factory;
    private TouchInput m_input;
    private Settings m_settings;
    private SignalBus m_signalBus;
    private bool m_trackIsClosed;

    #region Di

    [Inject]
    protected void Init( [Inject(Id = "TrackParent")]ISplineManager _splineManager,
                         Factory _factory,
                         TouchInput _input,
                         Settings _settings,
                         SignalBus _signalBus)
    {
        m_factory = _factory;
        m_waypoints = _splineManager.GetWaypoints ();
        m_input = _input;
        m_settings = _settings;
        m_signalBus = _signalBus;
        m_trackIsClosed = _splineManager.ClosedTrack;
    }

    public class Factory : PlaceholderFactory<VehicleFactory.Params, VehicleController> { }

    #endregion

    #region Unity Functions

    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody> ();
        m_connectedMesh = GetComponent<HingeJoint> ().connectedBody.transform;
        m_meshAnchor = transform.GetChild ( 0 );

        transform.position = m_waypoints [ m_currWaypointIndex ].position;
        transform.rotation = m_waypoints [ m_currWaypointIndex ].rotation;
    }

    void Update ()
    {
        Speed ();
        UpdateWaypoint ();
    }

    private void FixedUpdate ()
    {
        if ( !m_respawning )
            MoveWithPhysics ();
    }

    private void OnJointBreak ( float _breakForce )
    {
        Debug.Log ( "Joint Broken! Respawn..." );
        m_respawning = true;
        m_signalBus.Fire<RespawnSignal> ();
        if ( !RESPAWN_USING_PREFAB )
            Invoke ( "Respawn", m_settings.RespawnTime );
        else
            Invoke ( "RespawnWithPrefab", m_settings.RespawnTime );
    }

    private void OnEnable()
    {
        m_signalBus.Subscribe<DestroyVehicleSignal> (Destroy);
    }

    private void OnDisable()
    {
        m_signalBus.Unsubscribe<DestroyVehicleSignal> (Destroy);
    }

    #endregion

    #region Public Functions

    public void SetWaypoint ( int waypointIndex )
    {
        m_currWaypointIndex = waypointIndex;
    }

    #endregion

    #region Private Functions

    /* Movement Functions */

    private void Speed ()
    {
        m_speedPercentage = m_input.Value;
        if ( m_respawning ) m_speedPercentage = 0f;
    }

    [System.Obsolete ( "Move() is deprecated, use MoveWithPhysics() in FixedUpdate instead." )]
    public void Move ()
    {
        transform.position = Vector3.MoveTowards ( transform.position, m_waypoints [ m_currWaypointIndex ].position, m_settings.MaxSpeed * m_speedPercentage * Time.deltaTime );
        transform.rotation = Quaternion.RotateTowards ( transform.rotation, m_waypoints [ m_currWaypointIndex ].rotation, m_speedPercentage * m_settings.MaxDegrees );
    }

    private void MoveWithPhysics ()
    {
        m_rigidBody.MovePosition ( Vector3.MoveTowards ( transform.position, m_waypoints [ m_currWaypointIndex ].position, m_settings.MaxSpeed * m_speedPercentage * Time.deltaTime ) );
        m_rigidBody.MoveRotation ( Quaternion.RotateTowards ( transform.rotation, m_waypoints [ m_currWaypointIndex ].rotation, m_speedPercentage * m_settings.MaxDegrees ) );
    }

    private void UpdateWaypoint ()
    {
        if ( Vector3.Distance ( transform.position, m_waypoints [ m_currWaypointIndex ].position ) <= m_settings.WaypointDetectionRadius / 2f )
        {
            m_currWaypointIndex++;

            if (m_currWaypointIndex == m_waypoints.Count) {
                m_signalBus.Fire<LapSignal> ();
                if ( m_trackIsClosed ) {
                    m_currWaypointIndex = 0;
                } else {
                    m_currWaypointIndex = m_waypoints.Count - 1;
                }
            }
        }
    }

    /* Respawn Functions */
    [System.Obsolete ("Respawn() is deprecated, use RespawnWithPrefab() instead.")]
    private void Respawn ()
    {
        Debug.Log ( "Respawn" );
        GameObject prefab = Configuration.Vehicles [ 0 ];
        Rigidbody meshRigidbody = prefab.transform.GetChild ( 1 ).GetComponent<Rigidbody> ();
        HingeJoint joint = prefab.transform.GetChild ( 0 ).GetComponent<HingeJoint> ();

        meshRigidbody.velocity = Vector3.zero;
        meshRigidbody.angularVelocity = Vector3.zero;
        m_connectedMesh.position = m_meshAnchor.position;
        m_connectedMesh.rotation = m_meshAnchor.rotation;

        m_respawning = false;
        HingeJoint newJoint = gameObject.AddComponent<HingeJoint> ();
        newJoint.connectedBody = m_connectedMesh.GetComponent<Rigidbody> ();

        Assert.IsNotNull ( newJoint );
        Assert.IsNotNull ( joint );

        newJoint.anchor = joint.anchor;
        newJoint.axis = joint.axis;
        newJoint.autoConfigureConnectedAnchor = joint.autoConfigureConnectedAnchor;
        newJoint.connectedAnchor = joint.connectedAnchor;
        newJoint.useSpring = joint.useSpring;
        newJoint.spring = joint.spring;
        newJoint.useMotor = joint.useMotor;
        newJoint.motor = joint.motor;
        newJoint.useLimits = joint.useLimits;
        newJoint.limits = joint.limits;
        newJoint.breakForce = joint.breakForce;
        newJoint.breakTorque = joint.breakTorque;
        newJoint.enableCollision = joint.enableCollision;
        newJoint.enablePreprocessing = joint.enablePreprocessing;
        newJoint.massScale = joint.massScale;
        newJoint.connectedMassScale = joint.connectedMassScale;
    }

    private void RespawnWithPrefab ()
    {
        VehicleFactory.Params param = new VehicleFactory.Params ( m_waypoints [ m_currWaypointIndex ].position, m_waypoints [ m_currWaypointIndex ].rotation );
        VehicleController controller = m_factory.Create (param );
        controller.SetWaypoint ( m_currWaypointIndex );
        Destroy ();
    }

    private void Destroy()
    {
        Destroy (transform.parent.gameObject);
    }

    #endregion

    #region Debug Functions

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere ( transform.position, m_settings.WaypointDetectionRadius );
    }

    #endregion

    #region Settings

    [System.Serializable]
    public class Settings
    {
        public float MaxSpeed = 20f;
        public float MaxDegrees = 30f;
        public float WaypointDetectionRadius = 0.06f;
        public float RespawnTime = 1f;
    }

    #endregion
}
