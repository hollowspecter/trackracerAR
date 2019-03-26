/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Point3DFactory : IFactory<Point3DFactory.Params, Object, Transform>
{
    protected readonly DiContainer m_container;

    public class Params
    {
        
    }

    public class Factory : PlaceholderFactory<Point3DFactory.Params, Object, Transform> { }

    public Point3DFactory(DiContainer _container )
    {
        m_container = _container;
    }

    public Transform Create( Params _param, Object _prefab )
    {
        return m_container.InstantiatePrefabForComponent<Transform> (_prefab);
    }
}
