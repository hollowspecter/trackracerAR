/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;

public class VehicleManager
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

    public GameObject SpawnVehicleAtStart()
    {
        List<OrientedPoint> waypoints = m_splineManager.GetWaypoints ();
        VehicleFactory.Params param = new VehicleFactory.Params (waypoints [0].position, waypoints [0].rotation);
        VehicleController controller = m_vehicleFactory.Create (param);
        controller.SetWaypoint (0);
        return controller.gameObject;
    }
}
