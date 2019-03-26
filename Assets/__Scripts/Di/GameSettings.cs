using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettings")]
public class GameSettings : ScriptableObjectInstaller<GameSettings>
{
    public PointRecorder.Settings PointRecorder;

    public override void InstallBindings()
    {
        Container.BindInstances ( PointRecorder );
    }
}