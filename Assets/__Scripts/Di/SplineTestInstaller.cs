/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
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
        SignalsInstaller.Install (Container);
    }
}