using UnityEngine;

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
        m_defaultConfiguration = Resources.Load ( FILENAME_DEFAULT_CONFIG ) as Configuration;
        if ( m_defaultConfiguration == null )
        {
            Debug.LogError ( "Failed to load default configuration at Resources/" + FILENAME_DEFAULT_CONFIG );
        }
    }

    #endregion

    #region Fields

    [Header ( "Trackbuilder" )]
    [SerializeField]
    private GameObject m_startPrefab;
    [SerializeField]
    private GameObject [] m_trackParts;
    [SerializeField]
    private LayerMask m_planeLayer;
    [SerializeField]
    private LayerMask m_trackLayer;

    #endregion

    #region Properties

    public static GameObject StartPrefab
    {
        get
        {
            return DefaultConfig.m_startPrefab;
        }
    }

    public static LayerMask TrackLayer
    {
        get
        {
            return DefaultConfig.m_trackLayer;
        }
    }

    public static LayerMask PlaneLayer
    {
        get
        {
            return DefaultConfig.m_planeLayer;
        }
    }

    public static GameObject [] TrackParts
    {
        get
        {
            return DefaultConfig.m_trackParts;
        }
    }

    #endregion
}
