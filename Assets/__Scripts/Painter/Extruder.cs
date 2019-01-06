using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extruder
{
    public static void Extrude ( Mesh mesh, ShapeData shape, OrientedPoint [] path, Vector2 scale )
    {
        // scale
        Vector3 [] verts = new Vector3 [ shape.m_verts.Length ];
        for ( int i = 0; i < verts.Length; ++i )
        {
            verts [ i ] = shape.m_verts [ i ] * scale;
        }

        // initialisation
        int vertsInShape = verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = vertsInShape * segments;
        int triIndexCount = triCount * 3;

        int [] triangleIndices = new int [ triIndexCount ];
        Vector3 [] vertices = new Vector3 [ vertCount ];
        Vector3 [] normals = new Vector3 [ vertCount ];
        Vector2 [] uvs = new Vector2 [ vertCount ];

        /* Mesh Generation */

        // create the vertices
        float uSpan = GetUSpan ( ref verts );
        float [] lengthTable;
        CalcLengthTable ( out lengthTable, ref path );
        for ( int i = 0; i < path.Length; i++ )
        {
            int offset = i * vertsInShape;
            for ( int j = 0; j < vertsInShape; j++ )
            {
                int id = offset + j;
                vertices [ id ] = path [ i ].LocalToWorld ( verts [ j ] );
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
            for ( int l = 0; l < vertsInShape; l += 2 )
            {
                int a = offset + l + vertsInShape;
                int b = offset + l;
                int c = offset + l + 1;
                int d = offset + l + 1 + vertsInShape;
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

    private static float GetUSpan(ref Vector3[] _verts)
    {
        float result = 0f;
        for ( int i = 0; i < _verts.Length - 1; i++ )
        {
            Vector2 line = _verts [ i + 1 ] - _verts [ i ];
            result += line.magnitude;
        }
        return result;
    }

    private static void CalcLengthTable( out float [] _lengthTable, ref OrientedPoint [] _path )
    {
        _lengthTable = new float [ _path.Length ];
        _lengthTable [ 0 ] = 0f;
        float totalLength = 0f;
        OrientedPoint prev = _path [ 0 ];
        for (int i=1; i<_path.Length;++i )
        {
            OrientedPoint point = _path[i];
            float diff = ( prev.position - point.position ).magnitude;
            totalLength += diff;
            _lengthTable [ i ] = totalLength;
            prev = point;
        }
    }
}

