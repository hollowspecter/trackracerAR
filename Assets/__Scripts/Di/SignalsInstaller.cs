/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install ( Container );

        Container.DeclareSignal<FeaturePointMovedSignal> ();
        Container.DeclareSignal<DestroyVehicleSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<LapSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<RespawnSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<SettingsChangedSignal> ().OptionalSubscriber ();
    }
}
