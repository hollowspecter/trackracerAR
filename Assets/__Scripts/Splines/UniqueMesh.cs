/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;

namespace Baguio.Splines
{
    /// <summary>
    /// Derive from this class to ensure the GameObject has
    /// a unique mesh and a mesh filter.
    /// </summary>
    [RequireComponent ( typeof ( MeshFilter ) )]
    [RequireComponent ( typeof ( Mesh ) )]
    public class UniqueMesh : MonoBehaviour
    {
        [HideInInspector]
        private int m_ownerID; // to ensure they have a unique mesh
        private MeshFilter m_meshFilter;
        private Mesh m_mesh;

        #region Properties (public)

        MeshFilter mMeshFilter
        { // tries to find a mesh filter, adds one if it doesnt exist yet
            get
            {
                m_meshFilter = m_meshFilter ?? GetComponent<MeshFilter> ();
                m_meshFilter = m_meshFilter ?? gameObject.AddComponent<MeshFilter> ();
                return m_meshFilter;
            }
        }

        protected Mesh mMesh
        {
            get
            {
                bool isOwner = m_ownerID == gameObject.GetInstanceID ();
                if ( mMeshFilter.sharedMesh == null || !isOwner )
                {
                    mMeshFilter.sharedMesh = m_mesh = new Mesh ();
                    m_ownerID = gameObject.GetInstanceID ();
                    m_mesh.name = "Mesh [" + m_ownerID + "]";
                }
                return m_mesh;
            }
        }

        #endregion
    }
}
