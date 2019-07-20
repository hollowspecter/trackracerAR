/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baguio.Splines
{
    public class Extruder
    {
        public static void Extrude ( Mesh _mesh, ShapeData _shape, OrientedPoint [] _path, Vector2 _scale )
        {
            // check for traps
            _mesh.ThrowIfNull ( nameof ( _mesh ) );
            _shape.ThrowIfNull ( nameof ( _shape ) );
            _path.ThrowIfNull ( nameof ( _path ) );
            _scale.ThrowIfNull ( nameof ( _scale ) );

            // scale
            Vector3 [] verts = new Vector3 [ _shape.m_verts.Length ];
            for ( int i = 0; i < verts.Length; ++i )
            {
                verts [ i ] = _shape.m_verts [ i ] * _scale;
            }

            // initialisation
            int vertsInShape = verts.Length;
            int segments = _path.Length - 1;
            int edgeLoops = _path.Length;
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
            CalcLengthTable ( out lengthTable, ref _path );
            for ( int i = 0; i < _path.Length; i++ )
            {
                int offset = i * vertsInShape;
                for ( int j = 0; j < vertsInShape; j++ )
                {
                    int id = offset + j;
                    vertices [ id ] = _path [ i ].LocalToWorld ( verts [ j ] );
                    normals [ id ] = _path [ i ].LocalToWorldDirection ( _shape.m_normals [ j ] );

                    float vCoord = lengthTable.Sample ( i / ( ( float ) edgeLoops ) );
                    vCoord /= uSpan;

                    uvs [ id ] = new Vector2 ( _shape.m_us [ j ], vCoord );
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
            _mesh.Clear ();
            _mesh.vertices = vertices;
            _mesh.triangles = triangleIndices;
            _mesh.normals = normals;
            _mesh.uv = uvs;
        }

        private static float GetUSpan ( ref Vector3 [] _verts )
        {
            float result = 0f;
            for ( int i = 0; i < _verts.Length - 1; i++ )
            {
                Vector2 line = _verts [ i + 1 ] - _verts [ i ];
                result += line.magnitude;
            }
            return result;
        }

        private static void CalcLengthTable ( out float [] _lengthTable, ref OrientedPoint [] _path )
        {
            _lengthTable = new float [ _path.Length ];
            _lengthTable [ 0 ] = 0f;
            float totalLength = 0f;
            OrientedPoint prev = _path [ 0 ];
            for ( int i = 1; i < _path.Length; ++i )
            {
                OrientedPoint point = _path [ i ];
                float diff = ( prev.position - point.position ).magnitude;
                totalLength += diff;
                _lengthTable [ i ] = totalLength;
                prev = point;
            }
        }
    }


}
