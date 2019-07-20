/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

// TODO SUMMARY
public class ARTrackPainterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Install State Machines
        StateMachineInstaller.Install (Container);

        Container.BindInterfacesTo<TrackBuilderManager> ().FromNew ().AsSingle ().NonLazy ();

        Container.BindFactory<PointRecorder, PointRecorder.Factory> ();

        Container.BindFactory<Point3DFactory.Params, Transform, Point3DFactory.Factory> ()
            .FromFactory< Point3DFactory> ();

        RaceInstaller.Install (Container);

        SharingInstaller.Install (Container);
    }
}