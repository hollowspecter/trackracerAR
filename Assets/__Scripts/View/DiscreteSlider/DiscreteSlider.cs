/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Zenject;

/// <summary>
/// TODO 
/// * write summary
/// * test if this actually works
/// </summary>
public class DiscreteSlider : MonoBehaviour
{
    public IReadOnlyReactiveProperty<float> Value { get { return m_model.Value; } }

    [SerializeField]
    protected GameObject m_togglePrefab;
    [SerializeField]
    protected string [] m_labels;
    [SerializeField]
    protected float [] m_values;
    [SerializeField]
    protected int m_defaultValueIndex = 0;

    protected Transform m_transform;
    protected Toggle [] m_toggles;

    private int m_numberOfElements = 0;
    private DiscreteSliderModel m_model;
    private CompositeDisposable m_disposables;

    #region Unity Lifecycle

    private void Awake()
    {
        validateSettings ();
        m_numberOfElements = Mathf.Min (m_labels.Length, m_values.Length);
        m_defaultValueIndex = Mathf.Clamp (m_defaultValueIndex, 0, m_numberOfElements - 1);
        m_model = new DiscreteSliderModel (m_values, m_defaultValueIndex);

        m_transform = transform;
        m_toggles = new Toggle [m_numberOfElements];
        ToggleGroup toggleGroup = GetComponent<ToggleGroup> ();
        GameObject tmpGameObject;
        Toggle tmpToggle;
        TextMeshProUGUI tmpText;
        for ( int i = 0; i < m_numberOfElements; ++i ) {
            // instantiate and get all of the objects
            tmpGameObject = Instantiate (m_togglePrefab, m_transform);
            tmpToggle = tmpGameObject.GetComponent<Toggle> ();
            tmpToggle.ThrowIfNull (nameof (tmpToggle));
            tmpText = tmpGameObject.transform.GetChild (0).GetChild (1).GetComponent<TextMeshProUGUI> ();
            tmpText.ThrowIfNull (nameof (tmpText));
            tmpToggle.group = toggleGroup;
            m_toggles [i] = tmpToggle;

            // set stuff
            tmpText.text = m_labels [i];
        }
        m_toggles [m_model.Index.Value].isOn = true;
    }

    protected virtual void OnEnable()
    {
        m_disposables = new CompositeDisposable ();
        for ( int i = 0; i < m_numberOfElements; ++i ) {
            m_toggles [i]
                .OnValueChangedAsObservable ()
                .Where (isOn => isOn == true)
                .Subscribe (isOn => {
                    setIndex ();
                })
                .AddTo (m_disposables);
        }

        m_model.Index.Subscribe (i => Debug.Log ("Index changed to " + i)).AddTo (m_disposables);
    }

    protected virtual void OnDisable()
    {
        m_disposables.Dispose ();
        for ( int i = 0; i < m_numberOfElements; ++i ) {
            m_toggles [i].isOn = false;
        }
    }

    #endregion
    
    public void setClosestValue( float value )
    {
        m_model.setClosestValue (value);

        if ( m_toggles != null ) {
            if (m_toggles[m_model.Index.Value] != null) {
                m_toggles[m_model.Index.Value].isOn = true;
            }
        }
    }

    private void setIndex()
    {
        for ( int i = 0; i < m_toggles.Length; ++i ) {
            if ( m_toggles [i].isOn ) {
                m_model.Index.Value = i;
                break;
            }
        }
    }

    protected virtual void validateSettings()
    {
        if ( m_togglePrefab == null ) {
            Debug.LogWarning ("The Discrete Slider cannot initialize, if the prefab is null!", this);
        }
        m_labels.ThrowIfNull (nameof (m_labels));
        m_labels.ThrowIfNull (nameof (m_values));
        if ( m_labels.Length != m_values.Length ) {
            Debug.LogWarning ("There should be an equal amount of labels and values!", this);
        }
    }
}
