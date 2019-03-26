/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine.Assertions;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        //Container.Bind<Inputs> ().FromNewComponentSibling ().AsSingle ();
    }
}