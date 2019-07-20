/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using UnityEngine;

namespace Baguio.Splines
{
    public struct OrientedPoint
    {
        public Vector3 position;
        public Vector3 tangent;
        public Quaternion rotation;
        public Vector3 normal;

        public OrientedPoint ( Vector3 position, Quaternion rotation, Vector3 tangent, Vector3 normal )
        {
            this.position = position;
            this.rotation = rotation;
            this.tangent = tangent;
            this.normal = normal;
        }

        public Vector3 LocalToWorld ( Vector3 point )
        {
            return position + rotation * point;
        }

        public Vector3 WorldToLocal ( Vector3 point )
        {
            return Quaternion.Inverse ( rotation ) * ( point - position );
        }

        public Vector3 LocalToWorldDirection ( Vector3 dir )
        {
            return rotation * dir;
        }
    }
}
