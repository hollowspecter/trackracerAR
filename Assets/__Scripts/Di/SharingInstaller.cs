/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Installs the neccessary classes for the sharing feature,
/// most importantly the database architecture.
/// </summary>
public class SharingInstaller : Installer<SharingInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DatabaseApi> ().FromNew ().AsSingle ().NonLazy ();
        Container.BindInterfacesAndSelfTo<TracksRepository> ().FromNew ().AsSingle ().NonLazy ();
        Container.BindInterfacesAndSelfTo<ObserveDialogUseCase> ().FromNew ().AsSingle ();
        Container.BindInterfacesAndSelfTo<ObserveUseCase> ().FromNew ().AsSingle ();
        Container.BindInterfacesAndSelfTo<UpdateUseCase> ().FromNew ().AsSingle ();
    }
}