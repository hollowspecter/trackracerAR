/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

namespace Baguio.Splines
{
    /// <summary>
    /// Scriptable object containing the data of a track shape
    /// ready to extrude.
    /// </summary>
    [CreateAssetMenu ( menuName = "Custom/ShapeData" )]
    public class ShapeData : ScriptableObject
    {
        /// <summary>
        /// The vertices. Every two vertices from one line.
        /// Therefore the number of vertices must be even.
        /// </summary>
        public Vector2 [] m_verts;

        /// <summary>
        /// the normals, must be in the same order and number as the vertices
        /// </summary>
        public Vector2 [] m_normals;

        /// <summary>
        /// the u texture coordinates. there is no v, because it
        /// will be calculated during extrusion.
        /// Must be in the same order and number as vertices
        /// </summary>
        public float [] m_us;

        /// <summary>
        /// Returns the default street shape.
        /// </summary>
        /// <returns></returns>
        public static ShapeData GetDefaultShape()
        {
            ShapeData result = Resources.Load<ShapeData> ( "DefaultStreetShape" );
            if ( result == null ) throw new System.NullReferenceException ( nameof ( result ) );
            return result;
        }

        /// <summary>
        /// Debug method to draw lines and normals in scene editor.
        /// </summary>
        /// <param name="_pivot"></param>
        public void DrawGizmos ( Vector3 _pivot )
        {
            // normals
            Gizmos.color = Color.green;
            if ( m_verts.Length == m_normals.Length )
            {
                for ( int i = 0; i < m_normals.Length; ++i )
                {
                    Vector3 tmp = _pivot + new Vector3 ( m_verts [ i ].x, m_verts [ i ].y, 0 );
                    Gizmos.DrawLine ( tmp, tmp + new Vector3 ( m_normals [ i ].x, m_normals [ i ].y, 0 ) );
                }
            }

            // line
            Gizmos.color = Color.red;
            if ( m_verts.Length > 0 )
            {
                for ( int i = 0; i < m_verts.Length - 1; ++i )
                {
                    Gizmos.DrawLine ( _pivot + new Vector3 ( m_verts [ i ].x, m_verts [ i ].y, 0 ), _pivot + new Vector3 ( m_verts [ i + 1 ].x, m_verts [ i + 1 ].y, 0 ) );
                }
            }
        }
    }

}
