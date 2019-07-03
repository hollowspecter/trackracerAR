/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class DiscreteSlider : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_togglePrefab;
    [SerializeField]
    protected string [] m_labels;
    [SerializeField]
    protected float [] m_values;
    [SerializeField]
    protected int m_defaultValueIndex = 0;

    protected Transform m_transform;
    protected Toggle[] m_toggles;

    private int m_numberOfElements = 0;
    private DiscreteSliderModel m_model;

    private void Awake()
    {
        validateSettings ();
        m_transform = transform;
        m_numberOfElements = Mathf.Min ( m_labels.Length, m_values.Length );
        m_toggles = new Toggle [ m_numberOfElements ];
        m_defaultValueIndex = Mathf.Clamp ( m_defaultValueIndex, 0, m_numberOfElements - 1 );

        ToggleGroup toggleGroup = GetComponent<ToggleGroup> ();
        GameObject tmpGameObject;
        Toggle tmpToggle;
        TextMeshProUGUI tmpText;
        for (int i=0; i<m_numberOfElements; ++i )
        {
            // instantiate and get all of the objects
            tmpGameObject = Instantiate ( m_togglePrefab, m_transform );
            tmpToggle = tmpGameObject.GetComponent<Toggle> ();
            tmpToggle.ThrowIfNull ( nameof ( tmpToggle ) );
            tmpText = tmpGameObject.transform.GetChild ( 0 ).GetChild ( 1 ).GetComponent<TextMeshProUGUI> ();
            tmpText.ThrowIfNull ( nameof ( tmpText ) );
            tmpToggle.group = toggleGroup;
            m_toggles [ i ] = tmpToggle;

            // set stuff
            tmpText.text = m_labels [ i ];
        }
        m_toggles [ m_defaultValueIndex ].isOn = true;

        m_model = new DiscreteSliderModel ( m_values [ m_defaultValueIndex ] );
    }

    public ReactiveProperty<float> observeCurrentValue()
    {
        return m_model.value;
    }

    protected virtual void validateSettings()
    {
        if ( m_togglePrefab == null )
        {
            Debug.LogWarning ( "The Discrete Slider cannot initialize, if the prefab is null!", this );
        }
        m_labels.ThrowIfNull ( nameof ( m_labels ) );
        m_labels.ThrowIfNull ( nameof ( m_values ) );
        if (m_labels.Length != m_values.Length)
        {
            Debug.LogWarning ( "There should be an equal amount of labels and values!", this );
        }
    }
}
