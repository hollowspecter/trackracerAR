using UnityEngine.Assertions;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public GameObject m_snackbar;

    public override void InstallBindings ()
    {
        Debug.Log ( "MainInstaller InstallBindings called" );

        Container.Bind<Inputs> ().FromNewComponentSibling ().AsSingle ();
        Container.Bind<RectTransform> ().WithId ( "snackbar" ).FromComponentOn ( m_snackbar ).AsSingle ();
    }
}