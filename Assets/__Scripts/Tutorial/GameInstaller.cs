using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        Container.BindInterfacesTo<GameRunner> ().AsSingle ();
    }
}