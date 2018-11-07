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