/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;
using UnityEngine.UI;

public interface ISettingsViewModel
{
    void Activate();
}

//todo remove comments if not needed, add summary
public class SettingsViewModel : MonoBehaviour, ISettingsViewModel
{
    [SerializeField] protected DiscreteSlider m_scaleXSlider;
    [SerializeField] protected DiscreteSlider m_scaleYSlider;
    [SerializeField] protected DiscreteSlider m_precisionSlider;
    [SerializeField] protected Toggle m_closedToggle;
    [SerializeField] protected Button m_discardChangesButton;
    [SerializeField] protected Button m_saveChangesButton;
    [SerializeField] protected ToggleGroup m_materialGroup;
    [SerializeField] protected ToggleGroup m_shapeGroup;

    protected IBuildStateMachine m_session;
    protected DialogBuilder.Factory m_dialogBuilderFactory;
    protected SignalBus m_signalBus;

    #region Di

    [Inject]
    private void Construct(IBuildStateMachine _session,
                           DialogBuilder.Factory _dialogBuilderFactory,
                           SignalBus _signalBus)
    {
        m_session = _session;
        m_dialogBuilderFactory = _dialogBuilderFactory;
        m_signalBus = _signalBus;
        Deactivate ();
    }

    #endregion

    #region Unity lifecycle

    protected virtual void Awake()
    {
        m_discardChangesButton.onClick.AddListener ( OnDiscard );
        m_saveChangesButton.onClick.AddListener ( SaveAndDeactivate );
    }

    #endregion

    #region Callbacks

    public void Activate()
    {
        // activate
        gameObject.SetActive ( true );

        // load
        m_scaleXSlider.setClosestValue ( m_session.CurrentTrackData.m_scale.x );
        m_scaleYSlider.setClosestValue ( m_session.CurrentTrackData.m_scale.y );
        m_precisionSlider.setClosestValue ( m_session.CurrentTrackData.m_precision );
        m_closedToggle.isOn = m_session.CurrentTrackData.m_closed;
        foreach (Toggle t in m_materialGroup.ActiveToggles ()) { //todo refactor this
            ValueToggle valueToggle = (ValueToggle)t;
            if (valueToggle.Value == m_session.CurrentTrackData.m_materialIndex) {
                valueToggle.Select ();
                break;
            }
        }
        foreach ( Toggle t in m_shapeGroup.ActiveToggles () ) {
            ValueToggle valueToggle = (ValueToggle)t;
            if ( valueToggle.Value == m_session.CurrentTrackData.m_shapeIndex ) {
                valueToggle.Select ();
                break;
            }
        }
    }

    protected void OnDiscard()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Discard changes?")
            .SetMessage ("Would you like to discard the changes done to the tracks´ settings?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .AddButton ("YES", Deactivate)
            .AddButton ("NO")
            .Build ();
    }

    protected void Deactivate()
    {
        gameObject.SetActive (false);
    }

    protected void SaveAndDeactivate()
    {
        // save
        m_session.CurrentTrackData.m_scale.x = m_scaleXSlider.Value.Value;
        m_session.CurrentTrackData.m_scale.y = m_scaleYSlider.Value.Value;
        m_session.CurrentTrackData.m_precision = Mathf.RoundToInt(m_precisionSlider.Value.Value);
        m_session.CurrentTrackData.m_closed = m_closedToggle.isOn;
        foreach ( Toggle t in m_materialGroup.ActiveToggles () ) { //todo refactor this
            ValueToggle valueToggle = (ValueToggle)t;
            if (valueToggle.isOn) {
                m_session.CurrentTrackData.m_materialIndex = valueToggle.Value;
            }
        }
        foreach ( Toggle t in m_shapeGroup.ActiveToggles () ) {
            ValueToggle valueToggle = (ValueToggle)t;
            if ( valueToggle.isOn) {
                m_session.CurrentTrackData.m_shapeIndex = valueToggle.Value;
            }
        }

        Debug.Log ("Saved!");
        Debug.Log (m_session.CurrentTrackData.ToString ());
        m_signalBus.Fire<SettingsChangedSignal> ();

        // deactivate
        Deactivate ();
    }

    #endregion
}
