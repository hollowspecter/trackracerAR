/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Baguio.Splines;
using UnityEngine.UI;

public interface ISettingsUI
{
    void Activate();
}

//todo remove comments if not needed, add summary
public class SettingsUI : MonoBehaviour, ISettingsUI
{
    [SerializeField] protected DiscreteSlider m_scaleXSlider;
    [SerializeField] protected DiscreteSlider m_scaleYSlider;
    [SerializeField] protected DiscreteSlider m_precisionSlider;
    [SerializeField] protected Toggle m_closedToggle;
    [SerializeField] protected Button m_discardChangesButton;
    [SerializeField] protected Button m_saveChangesButton;
    [SerializeField] protected ValueToggle [] m_materialToggles;
    [SerializeField] protected ValueToggle [] m_shapeToggles;

    protected IBuildStateMachine m_session;
    protected DialogBuilder.Factory m_dialogBuilderFactory;
    protected SignalBus m_signalBus;

    #region Di

    [Inject]
    private void Construct( IBuildStateMachine _session,
                           DialogBuilder.Factory _dialogBuilderFactory,
                           SignalBus _signalBus )
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
        m_discardChangesButton.onClick.AddListener (OnDiscard);
        m_saveChangesButton.onClick.AddListener (SaveAndDeactivate);
    }

    #endregion

    #region Callbacks

    public void Activate()
    {
        // activate
        gameObject.SetActive (true);

        // load
        m_scaleXSlider.setClosestValue (m_session.CurrentTrackData.m_scale.x);
        m_scaleYSlider.setClosestValue (m_session.CurrentTrackData.m_scale.y);
        m_precisionSlider.setClosestValue (m_session.CurrentTrackData.m_precision);
        m_closedToggle.isOn = m_session.CurrentTrackData.m_closed;

        for ( int i = 0; i < m_materialToggles.Length; ++i ) {
            m_materialToggles [i].isOn = m_materialToggles [i].Value == m_session.CurrentTrackData.m_materialIndex;
        }
        for ( int i = 0; i < m_shapeToggles.Length; ++i ) {
            m_shapeToggles [i].isOn = m_shapeToggles [i].Value == m_session.CurrentTrackData.m_shapeIndex;
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
        m_session.CurrentTrackData.m_precision = Mathf.RoundToInt (m_precisionSlider.Value.Value);
        m_session.CurrentTrackData.m_closed = m_closedToggle.isOn;
        for ( int i = 0; i < m_materialToggles.Length; ++i ) {
            if (m_materialToggles[i].isOn) {
                m_session.CurrentTrackData.m_materialIndex = m_materialToggles [i].Value;
                break;
            }
        }
        for ( int i = 0; i < m_shapeToggles.Length; ++i ) {
            if ( m_shapeToggles [i].isOn ) {
                m_session.CurrentTrackData.m_shapeIndex = m_shapeToggles [i].Value;
                break;
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
