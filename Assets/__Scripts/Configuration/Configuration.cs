/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
 
 using UnityEngine;
using System.Collections.Generic;
using Baguio.Splines;

/// <summary>
/// Configuration file as singleton.
/// Use this to setup different build configurations
/// i.e. for CI.
/// </summary>
[CreateAssetMenu ( menuName = "Custom/Configuration" )]
public class Configuration : ScriptableObject
{
    #region Configuration Singleton Code

    private static Configuration m_defaultConfiguration = null;
    private const string FILENAME_DEFAULT_CONFIG = "DefaultConfiguration";

    private static Configuration DefaultConfig
    {
        get
        {
            if ( m_defaultConfiguration == null )
                LoadDefaultConfiguration ();
            return m_defaultConfiguration;
        }
    }

    private static void LoadDefaultConfiguration ()
    {
        // Load
        m_defaultConfiguration = Resources.Load ( FILENAME_DEFAULT_CONFIG ) as Configuration;
        if ( m_defaultConfiguration == null )
        {
            Debug.LogError ( "Failed to load default configuration at Resources/" + FILENAME_DEFAULT_CONFIG );
        }
    }

    #endregion

    #region Fields

    [Header ( "Debug" )]
    [SerializeField]
    private bool m_showTangents;
    [SerializeField]
    private bool m_showNormals;

    [Header ( "Trackpainter" )]
    [SerializeField]
    private float m_distanceThreshold = 0.05f;
    [SerializeField]
    private float m_angleThreshold = 20f;
    [SerializeField]
    private ShapeData[] m_shapeDatas;
    [SerializeField]
    private Material [] m_streetMaterials;

    [ Header ( "Vehicles" )]
    [SerializeField]
    private GameObject[] m_vehicles;

    [Header ( "Trackbuilder (Deprecated)" )]
    [SerializeField]
    private List<GameObject> m_trackParts;
    [SerializeField]
    private LayerMask m_planeLayer;
    [SerializeField]
    private LayerMask m_trackLayer;
    [SerializeField]
    private LayerMask m_arrowLayer;
    [SerializeField]
    private GameObject m_loadItemList;

    #endregion

    #region Properties

    public static bool ShowTangents { get { return DefaultConfig.m_showTangents; } }
    public static bool ShowNormals { get { return DefaultConfig.m_showNormals; } }
    public static GameObject[] Vehicles { get { return DefaultConfig.m_vehicles; } }
    public static ShapeData[] ShapeDatas { get { return DefaultConfig.m_shapeDatas; } }
    public static Material[] StreetMaterials { get { return DefaultConfig.m_streetMaterials; } }
    public static float DistanceThreshold { get { return DefaultConfig.m_distanceThreshold; } }
    public static float AngleThreshold { get { return DefaultConfig.m_angleThreshold; } }

    #endregion
}
