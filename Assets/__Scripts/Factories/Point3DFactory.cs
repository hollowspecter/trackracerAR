/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        return m_container.InstantiatePrefabForComponent<Transform> ( m_settings.Prefab );
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject Prefab;
    }
}
