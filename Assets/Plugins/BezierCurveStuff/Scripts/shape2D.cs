///<summary>
///<copyright>(c) Vivien Baguio</copyright>
///http://www.vivienbaguio.com
///</summary>

using UnityEngine;
using System.Collections;

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
public class shape2D : UniqueMesh
{
    #region variables (private)
    public bool m_debugDrawMesh = false;
    public Vector2 [] m_verts;
    public Vector2 [] m_normals;
    public float [] m_us;
    public Vector3 [] m_bezierPositions = new Vector3 [ 4 ];
    public int m_numberOfSegments = 6;
    public float m_height = 0.5f;

    private Bezier m_bezier;
    private int [] lines;
    #endregion

    #region Properties (public)

    #endregion

    #region Unity event functions

    ///<summary>
    ///Use this for very first initialization
    ///</summary>
    void Awake ()
    {
        m_bezier = new Bezier ();

        lines = new int [ m_verts.Length ];
        for ( int i = 0; i < lines.Length; ++i )
        {
            lines [ i ] = i;
            m_verts [ i ].y *= m_height;
        }
    }

    ///<summary>
    ///Use this for initialization
    ///</summary>
    void Start ()
    {
        m_bezier.m_points = m_bezierPositions;

        Extrude ( mesh, this, m_bezier.GetBezierPath ( m_numberOfSegments ) );

        Debug.Log ( GetUSpan () );
    }

    ///<summary>
    ///Debugging information should be put here
    ///</summary>
    void OnDrawGizmos ()
    {
        // the mesh
        if ( mesh.vertexCount != 0 && m_debugDrawMesh )
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh ( mesh );
        }

        if ( Application.isPlaying )
            return;

        // line
        Gizmos.color = Color.yellow;
        if ( m_verts.Length > 0 )
        {
            for ( int i = 0; i < m_verts.Length - 1; ++i )
            {
                Gizmos.DrawLine ( transform.position + new Vector3 ( m_verts [ i ].x, m_verts [ i ].y, 0 ), transform.position + new Vector3 ( m_verts [ i + 1 ].x, m_verts [ i + 1 ].y, 0 ) );
            }
        }

        // normals
        Gizmos.color = Color.green;
        if ( m_verts.Length == m_normals.Length )
        {
            for ( int i = 0; i < m_normals.Length; ++i )
            {
                Vector3 tmp = transform.position + new Vector3 ( m_verts [ i ].x, m_verts [ i ].y, 0 );
                Gizmos.DrawLine ( tmp, tmp + new Vector3 ( m_normals [ i ].x, m_normals [ i ].y, 0 ) );
            }
        }

        // bezier path
        m_bezier = new Bezier ();
        m_bezier.m_points = m_bezierPositions;
        foreach ( OrientedPoint p in m_bezier.GetBezierPath ( m_numberOfSegments ) )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube ( p.position, Vector3.one * 0.1f );
        }
        for ( int i = 0; i < m_bezierPositions.Length; i++ )
        {
            Gizmos.color = Color.red;
            if ( i != m_bezierPositions.Length - 1 )
                Gizmos.DrawLine ( m_bezierPositions [ i ], m_bezierPositions [ ( i + 1 ) % 4 ] );
        }

    }

    #endregion

    #region Methods

    float GetUSpan ()
    {
        float result = 0f;
        for ( int i = 0; i < m_verts.Length - 1; i++ )
        {
            Vector2 line = m_verts [ i + 1 ] - m_verts [ i ];
            result += line.magnitude;
        }
        return result;
    }

    Vector2 GetNormal ( Vector2 tangent )
    {
        return new Vector2 ( -tangent.y, tangent.x );
    }

    public void Extrude ( Mesh mesh, shape2D shape, OrientedPoint [] path )
    {
        // initialisation
        int vertsInShape = shape.m_verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments;
        int triIndexCount = triCount * 3;

        int [] triangleIndices = new int [ triIndexCount ];
        Vector3 [] vertices = new Vector3 [ vertCount ];
        Vector3 [] normals = new Vector3 [ vertCount ];
        Vector2 [] uvs = new Vector2 [ vertCount ];

        /* Mesh Generation */

        // create the vertices
        float uSpan = GetUSpan ();
        float totalLength = Bezier.GetLength ( path );
        float [] lengthTable = new float [ path.Length ];
        CalcLengthTableInto ( lengthTable, m_bezier );
        for ( int i = 0; i < path.Length; i++ )
        {
            int offset = i * vertsInShape;
            for ( int j = 0; j < vertsInShape; j++ )
            {
                int id = offset + j;
                vertices [ id ] = path [ i ].LocalToWorld ( shape.m_verts [ j ] );
                normals [ id ] = path [ i ].LocalToWorldDirection ( shape.m_normals [ j ] );

                float vCoord = lengthTable.Sample ( i / ( ( float ) edgeLoops ) );
                vCoord /= uSpan;

                uvs [ id ] = new Vector2 ( shape.m_us [ j ], vCoord );
            }
        }

        // create the segments
        int ti = 0;
        for ( int i = 0; i < segments; i++ )
        {
            int offset = i * vertsInShape;
            for ( int l = 0; l < lines.Length; l += 2 )
            {
                int a = offset + lines [ l ] + vertsInShape;
                int b = offset + lines [ l ];
                int c = offset + lines [ l + 1 ];
                int d = offset + lines [ l + 1 ] + vertsInShape;
                triangleIndices [ ti ] = c; ti++;
                triangleIndices [ ti ] = b; ti++;
                triangleIndices [ ti ] = a; ti++;
                triangleIndices [ ti ] = a; ti++;
                triangleIndices [ ti ] = d; ti++;
                triangleIndices [ ti ] = c; ti++;
            }
        }

        // apply
        mesh.Clear ();
        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.normals = normals;
        mesh.uv = uvs;
    }

    void CalcLengthTableInto ( float [] _array, Bezier _bezier )
    {
        _array [ 0 ] = 0f;
        float totalLength = 0f;
        Vector3 prev = _bezier.p0;
        for ( int i = 1; i < _array.Length; ++i )
        {
            float t = ( ( float ) i ) / ( _array.Length - 1 );
            Vector3 point = _bezier.GetPoint ( t );
            float diff = ( prev - point ).magnitude;
            totalLength += diff;
            _array [ i ] = totalLength;
            prev = point;
        }
    }

    #endregion
}