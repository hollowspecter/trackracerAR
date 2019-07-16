using UnityEngine;
using Zenject;

public class TrackSharingTestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SharingInstaller.Install (Container);

        Container.BindFactory<Point3DFactory.Params, Transform, Point3DFactory.Factory> ()
            .FromFactory<Point3DFactory> ();

    }
}