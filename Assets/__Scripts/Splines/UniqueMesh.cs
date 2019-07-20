/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using UnityEngine;
using System.Collections;

namespace Baguio.Splines
{
    [RequireComponent ( typeof ( MeshFilter ) )]
    [RequireComponent ( typeof ( Mesh ) )]
    public class UniqueMesh : MonoBehaviour
    {
        #region variables (private)
        [HideInInspector]
        int ownerID; // to ensure they have a unique mesh
        MeshFilter _mf;
        Mesh _mesh;
        #endregion

        #region Properties (public)
        MeshFilter mf
        { // tries to find a mesh filter, adds one if it doesnt exist yet
            get
            {
                _mf = _mf == null ? GetComponent<MeshFilter> () : _mf;
                _mf = _mf == null ? gameObject.AddComponent<MeshFilter> () : _mf;
                return _mf;
            }
        }

        protected Mesh mesh
        {
            get
            {
                bool isOwner = ownerID == gameObject.GetInstanceID ();
                if ( mf.sharedMesh == null || !isOwner )
                {
                    mf.sharedMesh = _mesh = new Mesh ();
                    ownerID = gameObject.GetInstanceID ();
                    _mesh.name = "Mesh [" + ownerID + "]";
                }
                return _mesh;
            }
        }
        #endregion
    }
}
