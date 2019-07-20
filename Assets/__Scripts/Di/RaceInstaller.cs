/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

public class RaceInstaller : Installer<RaceInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TouchInput> ().AsSingle ();
        Container.BindInterfacesAndSelfTo<VehicleManager> ().AsSingle ();
        Container.BindInterfacesAndSelfTo<RaceManager> ().AsSingle ();

        Container.BindFactory<VehicleFactory.Params, VehicleController, VehicleController.Factory> ()
            .FromFactory<VehicleFactory> ();
    }
}