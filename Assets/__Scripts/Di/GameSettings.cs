/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using Zenject;
using Baguio.Splines;

// TODO SUMMARY
[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettings")]
public class GameSettings : ScriptableObjectInstaller<GameSettings>
{
    public TrackPainterSettings TrackPainter;
    public UISettings UI;

    [System.Serializable]
    public class TrackPainterSettings
    {
        public PointRecorder.Settings PointRecorder;
        public Point3DFactory.Settings Point3DFactory;
    }

    [System.Serializable]
    public class UISettings
    {
        public BuildLoadViewModel.Settings LoadingSettings;
        public DialogBuilder.Settings DialogSettings;
        public DialogFactory.Settings DialogFactorySettings;
    }

    public override void InstallBindings()
    {
        Container.BindInstance ( TrackPainter.PointRecorder ).IfNotBound ();
        Container.BindInstance ( TrackPainter.Point3DFactory ).IfNotBound ();
        Container.BindInstance ( UI.LoadingSettings ).IfNotBound ();
        Container.BindInstance ( UI.DialogSettings).IfNotBound ();
        Container.BindInstance ( UI.DialogFactorySettings).IfNotBound ();
    }
}