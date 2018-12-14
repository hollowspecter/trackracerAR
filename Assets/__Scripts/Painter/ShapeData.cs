using UnityEngine;
using System.Collections;

[CreateAssetMenu ( menuName = "Custom/ShapeData" )]
public class ShapeData : ScriptableObject
{
    public Vector2 [] m_verts;
    public Vector2 [] m_normals;
    public float [] m_us;

    public float GetUSpan ()
    {
        float result = 0f;
        for ( int i = 0; i < m_verts.Length - 1; i++ )
        {
            Vector2 line = m_verts [ i + 1 ] - m_verts [ i ];
            result += line.magnitude;
        }
        return result;
    }

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
