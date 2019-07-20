/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;
using Baguio.Splines;

/// <summary>
/// Test installer to install everything needed to setup a race
/// </summary>
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