/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using Zenject;

/// <summary>
/// Installed in the project context (hence, globally), do all app wide initialization here.
/// </summary>
public class ProjectMainInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        // init firebase api
        Container.BindInterfacesAndSelfTo<FirebaseApi> ().FromNew ().AsSingle ().NonLazy ();
        Container.BindInterfacesAndSelfTo<AuthApi> ().FromNew ().AsSingle ().NonLazy ();
        DialogInstaller.Install (Container);

        // Install Signals
        SignalsInstaller.Install (Container);

    }
}