/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

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