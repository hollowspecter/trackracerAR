/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine.Assertions;
using UnityEngine;
using Zenject;

/// <summary>
/// Installed in the project context, do all app wide initialization here.
/// </summary>
public class MainInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        // init firebase api
        Container.BindInterfacesAndSelfTo<FirebaseApi> ().FromNew ().AsSingle ().NonLazy ();
        Container.BindInterfacesAndSelfTo<AuthApi> ().FromNew ().AsSingle ().NonLazy ();
    }
}