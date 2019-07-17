/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
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
    }
}