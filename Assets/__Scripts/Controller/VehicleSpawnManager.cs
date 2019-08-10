/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;

/// <summary>
/// Manages the spawning of the vehicle for the race setup.
/// </summary>
public class VehicleSpawnManager
{
    private VehicleController.Factory m_vehicleFactory;
    private ISplineManager m_splineManager;

    [Inject]
    protected void Init( [Inject(Id = "TrackParent")]ISplineManager _splineManager,
                     VehicleController.Factory _factory)
    {
        m_vehicleFactory = _factory;
        m_splineManager = _splineManager;
    }

    /// <summary>
    /// Spawns a vehicle on the tracks first waypoint.
    /// </summary>
    /// <returns>The vehicles gameobject.</returns>
    public GameObject SpawnVehicleAtStart()
    {
        List<OrientedPoint> waypoints = m_splineManager.GetWaypoints ();
        VehicleFactory.Params param = new VehicleFactory.Params (waypoints [0].position, waypoints [0].rotation);
        VehicleController controller = m_vehicleFactory.Create (param);
        controller.SetWaypoint (0);
        return controller.gameObject;
    }
}
