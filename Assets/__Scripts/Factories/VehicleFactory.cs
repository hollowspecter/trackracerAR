/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Prefab-Instantiating Factory that spawns vehicles at a given
/// position with a given rotation.
/// Can be altered so that different vehicles get spawned.
/// As of now, only one vehicle type exists (set in <see cref="Configuration"/>).
/// 
/// Todo Improvement: Convert to memory pool
/// </summary>
public class VehicleFactory : IFactory<VehicleFactory.Params, VehicleController>
{
    protected readonly DiContainer m_container;

    public class Params
    {
        public Params(Vector3 _position, Quaternion _rotation )
        {
            _position.ThrowIfNull ( nameof ( _position ) );
            _rotation.ThrowIfNull ( nameof ( _rotation ) );
            Position = _position;
            Rotation = _rotation;
        }

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
    }

    public VehicleFactory(DiContainer _container )
    {
        m_container = _container;
    }

    public VehicleController Create( Params _params )
    {
        return m_container.InstantiatePrefabForComponent<VehicleController> ( Configuration.Vehicles[0], _params.Position, _params.Rotation, null );
    }

    public VehicleController Create(UnityEngine.Object _prefab, Vector3 _position, Quaternion _rotation )
    {
        return m_container.InstantiatePrefabForComponent<VehicleController> ( _prefab, _position, _rotation, null );
    }
}
