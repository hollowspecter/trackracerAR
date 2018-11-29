///<summary>
///<copyright>(c) Vivien Baguio</copyright>
///http://www.vivienbaguio.com
///</summary>

using UnityEngine;
using System.Collections;

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
public class BezierHandle : MonoBehaviour
{
    #region variables (private)
    private Vector3 scale;
    #endregion

    #region Properties (public)
    public Vector3 Scale
    {
        get { return scale; }
        set { scale = value; }
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
    ///Gets updated every frame
    ///</summary>
    void Update()
    {

    }

    ///<summary>
    ///Debugging information should be put here
    ///</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,1f,1,0.7f);
        Gizmos.DrawLine(transform.position, transform.localScale);
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawSphere(transform.localScale, 0.3f);
    }

    #endregion

    #region Methods

    #endregion
}