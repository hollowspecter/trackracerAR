///<summary>
///<copyright>(c) Vivien Baguio</copyright>
///http://www.vivienbaguio.com
///</summary>

using UnityEngine;
using System.Collections;

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
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
            _mf = _mf == null ? GetComponent<MeshFilter>() : _mf;
            _mf = _mf == null ? gameObject.AddComponent<MeshFilter>() : _mf;
            return _mf;
        }
    }

    protected Mesh mesh
    {
        get
        {
            bool isOwner = ownerID == gameObject.GetInstanceID();
            if (mf.sharedMesh == null || !isOwner) {
                mf.sharedMesh = _mesh = new Mesh();
                ownerID = gameObject.GetInstanceID();
                _mesh.name = "Mesh [" + ownerID + "]";
            }
            return _mesh;
        }
    }
    #endregion

    #region Unity event functions

    ///<summary>
    ///Use this for very first initialization
    ///</summary>
    void Awake()
    {

    }

    ///<summary>
    ///Use this for initialization
    ///</summary>
    void Start()
    {

    }

    ///<summary>
    ///Debugging information should be put here
    ///</summary>
    void OnDrawGizmos()
    {

    }

    #endregion

    #region Methods

    #endregion
}