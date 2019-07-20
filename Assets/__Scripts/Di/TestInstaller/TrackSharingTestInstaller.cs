/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Test installer to setup a test environment for the sharing feature
/// </summary>
public class TrackSharingTestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SharingInstaller.Install (Container);

        Container.BindFactory<Point3DFactory.Params, Transform, Point3DFactory.Factory> ()
            .FromFactory<Point3DFactory> ();

    }
}