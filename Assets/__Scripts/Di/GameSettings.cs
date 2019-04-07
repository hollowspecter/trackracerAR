using UnityEngine;
using Zenject;
using Baguio.Splines;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettings")]
public class GameSettings : ScriptableObjectInstaller<GameSettings>
{
    public PointRecorder.Settings PointRecorder;
    public Point3DFactory.Settings Point3DFactory;

    public override void InstallBindings()
    {
        Container.BindInstances ( PointRecorder, Point3DFactory );
    }
}