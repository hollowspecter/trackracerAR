/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;

public class VehicleManager
{
    private VehicleController.Factory m_vehicleFactory;
    private List<OrientedPoint> m_waypoints;

    [Inject]
    protected void Init( [Inject(Id = "TrackParent")]ISplineManager _splineManager,
                     VehicleController.Factory _factory)
    {
        m_vehicleFactory = _factory;
        m_waypoints = _splineManager.GetWaypoints ();
    }

    public GameObject SpawnVehicleAtStart()
    {
        VehicleFactory.Params param = new VehicleFactory.Params (m_waypoints [0].position, m_waypoints [0].rotation);
        VehicleController controller = m_vehicleFactory.Create (param);
        controller.SetWaypoint (0);
        return controller.gameObject;
    }
}
