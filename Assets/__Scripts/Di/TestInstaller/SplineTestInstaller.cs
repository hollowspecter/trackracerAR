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

    public class MockBuildStateMachine : IBuildStateMachine
    {
        public bool ReturnToPreviousStateFlag { get => throw new System.NotImplementedException (); set => throw new System.NotImplementedException (); }
        public TrackData CurrentTrackData { get => null; set => throw new System.NotImplementedException (); }
        public Vector3 CurrentFeaturePointOffset { get => throw new System.NotImplementedException (); set => throw new System.NotImplementedException (); }

        public event State.TouchHandler m_touchDetected;
    }
}