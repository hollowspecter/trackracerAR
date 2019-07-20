/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using Zenject;

/// <summary>
/// Installs the neccessary signals (using Zenject's Signal Bus).
/// </summary>
public class SignalsInstaller : Installer<SignalsInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install ( Container );

        Container.DeclareSignal<FeaturePointChanged> ();
        Container.DeclareSignal<DestroyVehicleSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<LapSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<RespawnSignal> ().OptionalSubscriber ();
        Container.DeclareSignal<SettingsChangedSignal> ().OptionalSubscriber ();
    }
}
