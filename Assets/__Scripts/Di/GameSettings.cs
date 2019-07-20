/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;
using Baguio.Splines;

// TODO SUMMARY
[CreateAssetMenu (fileName = "GameSettings", menuName = "Installers/GameSettings")]
public class GameSettings : ScriptableObjectInstaller<GameSettings>
{
    public TrackPainterSettings TrackPainter;
    public UISettings UI;
    public RaceSettings Race;
    public FirebaseSettings Firebase;

    [System.Serializable]
    public class TrackPainterSettings
    {
        public CalibrateUI.Settings CalibrateSettings;
        public PointRecorder.Settings PointRecorder;
        public Point3DFactory.Settings Point3DFactory;
        public CenterTrackTool.Settings CenterToolSettings;
    }

    [System.Serializable]
    public class UISettings
    {
        public BuildLoadUI.Settings LoadingSettings;
        public DialogBuilder.Settings DialogSettings;
        public DialogFactory.Settings DialogFactorySettings;
    }

    [System.Serializable]
    public class RaceSettings
    {
        public TouchInput.Settings InputSettings;
        public VehicleController.Settings VehicleSettings;
        public RaceSetupUI.Settings SetupSettings;
    }

    [System.Serializable]
    public class FirebaseSettings
    {
        public DatabaseApi.Settings DbSettings;
    }

    public override void InstallBindings()
    {
        Container.BindInstance (TrackPainter.PointRecorder).IfNotBound ();
        Container.BindInstance (TrackPainter.Point3DFactory).IfNotBound ();
        Container.BindInstance (TrackPainter.CalibrateSettings).IfNotBound ();
        Container.BindInstance (TrackPainter.CenterToolSettings).IfNotBound ();

        Container.BindInstance (UI.LoadingSettings).IfNotBound ();
        Container.BindInstance (UI.DialogSettings).IfNotBound ();
        Container.BindInstance (UI.DialogFactorySettings).IfNotBound ();

        Container.BindInstance (Race.InputSettings).IfNotBound ();
        Container.BindInstance (Race.VehicleSettings).IfNotBound ();
        Container.BindInstance (Race.SetupSettings).IfNotBound ();

        Container.BindInstance (Firebase.DbSettings).IfNotBound ();
    }
}