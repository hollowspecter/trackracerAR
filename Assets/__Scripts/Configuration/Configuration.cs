using UnityEngine;
using System.Collections.Generic;
using System;

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

        // Setup Trackparts
        for ( int i = 0; i < m_defaultConfiguration.m_trackParts.Count; ++i )
        {
            // Every Trackpart knows it's own Index in the Array
            m_defaultConfiguration.m_trackParts [ i ].GetComponent<TrackPart> ().Index = i;
        }
    }

    #endregion

    #region Fields

    [Header ( "Debug" )]
    [SerializeField]
    private bool m_showTangents;
    [SerializeField]
    private bool m_showNormals;
    [Header ( "Trackbuilder" )]
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
    public static LayerMask TrackLayer { get { return DefaultConfig.m_trackLayer; } }
    public static LayerMask PlaneLayer { get { return DefaultConfig.m_planeLayer; } }
    public static List<GameObject> TrackParts { get { return DefaultConfig.m_trackParts; } }
    public static LayerMask ArrowLayer { get { return DefaultConfig.m_arrowLayer; } }
    public static GameObject LoadItemList { get { return DefaultConfig.m_loadItemList; } }

    #endregion
}
