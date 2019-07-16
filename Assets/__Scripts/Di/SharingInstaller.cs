using UnityEngine;
using Zenject;

public class SharingInstaller : Installer<SharingInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DatabaseApi> ().FromNew ().AsSingle ().NonLazy ();
        Container.BindInterfacesAndSelfTo<TracksRepository> ().FromNew ().AsSingle ().NonLazy ();
    }
}