/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using Zenject;

public class ARTrackPainterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Install State Machines
        StateMachineInstaller.Install (Container);

        // Install Signals
        SignalsInstaller.Install ( Container );

        Container.BindInterfacesTo<TrackBuilderManager> ().FromNew ().AsSingle ().NonLazy ();

        Container.BindFactory<PointRecorder, PointRecorder.Factory> ();

        Container.BindFactory<Point3DFactory.Params, Transform, Point3DFactory.Factory> ()
            .FromFactory< Point3DFactory> ();
    }
}