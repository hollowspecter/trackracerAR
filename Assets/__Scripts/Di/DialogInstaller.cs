/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using Zenject;

// TODO SUMMARY
public class DialogInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<DialogBuilder, DialogBuilder.Factory> ();
        Container.BindFactory<DialogFactory.Params, Dialog, DialogFactory.Factory> ()
            .FromFactory<DialogFactory> ();
    }
}
