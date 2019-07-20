/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */
using UnityEngine;
using Zenject;
using Baguio.Splines;

public class SplineTestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log ( "Spline Test Installer: InstallBindings" );

        Container.Bind<ISplineManager> ()
            .To<SplineManager>()
            .FromComponentInHierarchy ()
            .AsSingle ()
            .NonLazy ();

        RaceInstaller.Install (Container);
        RaceTestStateMachineInstaller.Install (Container);
    }
}