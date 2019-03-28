/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using Zenject;

public class ARTrackPainterInstaller : MonoInstaller
{
    [SerializeField]
    protected GameObject m_prefabPoint3D;

    public override void InstallBindings()
    {
        Container.BindFactory<PointRecorder, PointRecorder.Factory> ();

        Container.BindFactory<Point3DFactory.Params, Transform, Point3DFactory.Factory> ()
            .FromFactory< Point3DFactory> ();
    }

    private void CheckFields()
    {
        m_prefabPoint3D.ThrowIfNull ( nameof ( m_prefabPoint3D ) );
    }
}