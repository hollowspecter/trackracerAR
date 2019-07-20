/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using Zenject;

/// <summary>
/// Installs the neccessary classes for the race feature.
/// </summary>
public class RaceInstaller : Installer<RaceInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TouchInput> ().AsSingle ();
        Container.BindInterfacesAndSelfTo<VehicleSpawnManager> ().AsSingle ();
        Container.BindInterfacesAndSelfTo<RaceManager> ().AsSingle ();

        Container.BindFactory<VehicleFactory.Params, VehicleController, VehicleController.Factory> ()
            .FromFactory<VehicleFactory> ();
    }
}