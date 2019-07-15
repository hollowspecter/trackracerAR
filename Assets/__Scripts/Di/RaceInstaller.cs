using UnityEngine;
using Zenject;

public class RaceInstaller : Installer<RaceInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TouchInput> ().AsSingle ();

        Container.BindFactory<VehicleFactory.Params, VehicleController, VehicleController.Factory> ()
            .FromFactory<VehicleFactory> ();
    }
}