/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

public class Point3DFactory : IFactory<Point3DFactory.Params, Transform>
{
    protected readonly DiContainer m_container;
    protected Settings m_settings;

    public class Params
    {
        
    }

    public class Factory : PlaceholderFactory<Params, Transform> { }

    public Point3DFactory(DiContainer _container, Settings _settings)
    {
        m_container = _container;
        m_settings = _settings;
    }

    public Transform Create( Params _param )
    {
        return m_container.InstantiatePrefab ( m_settings.Prefab ).transform;
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject Prefab;
    }
}
