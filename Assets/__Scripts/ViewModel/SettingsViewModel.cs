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

    protected IBuildStateMachine m_session;

    #region Di

    [Inject]
    private void Construct(IBuildStateMachine _session)
    {
        m_session = _session;
        Deactivate ();
    }

    #endregion

    #region Unity lifecycle

    protected virtual void Awake()
    {
        //todo remove this if not needed
        //m_scaleXSlider.onValueChanged.AddListener ( val => { m_scaleXValueLabel.text = val.ToString ( "0.000" ); } );
        //m_scaleYSlider.onValueChanged.AddListener ( val => { m_scaleYValueLabel.text = val.ToString ( "0.000" ); } );
        //m_precisionSlider.onValueChanged.AddListener ( val => { m_precisionValueLabel.text = val.ToString ( ); } );
        m_discardChangesButton.onClick.AddListener ( Deactivate );
        m_saveChangesButton.onClick.AddListener ( SaveAndDeactivate );
    }

    #endregion

    #region Callbacks

    public void Activate()
    {
        // load
        m_scaleXSlider.setClosestValue ( m_session.CurrentTrackData.m_scale.x );
        m_scaleYSlider.setClosestValue ( m_session.CurrentTrackData.m_scale.y );
        m_precisionSlider.setClosestValue ( m_session.CurrentTrackData.m_precision );

        Debug.Log ("LOAD!");
        Debug.Log (m_session.CurrentTrackData.ToString ());

        // load 
        //m_scaleXSlider.value = m_session.CurrentTrackData.m_scale.x;
        //m_scaleXSlider.onValueChanged.Invoke ( m_scaleXSlider.value );
        //m_scaleYSlider.value = m_session.CurrentTrackData.m_scale.y;
        //m_scaleYSlider.onValueChanged.Invoke ( m_scaleYSlider.value );
        //m_precisionSlider.value = m_session.CurrentTrackData.m_precision;
        //m_precisionSlider.onValueChanged.Invoke ( m_precisionSlider.value );
        m_closedToggle.isOn = m_session.CurrentTrackData.m_closed;

        //m_scaleXValueLabel.text = m_scaleXSlider.value.ToString ( "0.00" );
        //m_scaleYValueLabel.text = m_scaleYSlider.value.ToString ( "0.00" );
        //m_precisionValueLabel.text = m_precisionSlider.value.ToString ();

        // activate
        gameObject.SetActive ( true );
    }

    protected void Deactivate()
    {
        gameObject.SetActive ( false );
    }

    protected void SaveAndDeactivate()
    {
        // save
        m_session.CurrentTrackData.m_scale.x = m_scaleXSlider.Value.Value;
        m_session.CurrentTrackData.m_scale.y = m_scaleYSlider.Value.Value;
        m_session.CurrentTrackData.m_precision = Mathf.RoundToInt(m_precisionSlider.Value.Value);
        m_session.CurrentTrackData.m_closed = m_closedToggle.isOn;
        Debug.Log ("Saved!");
        Debug.Log (m_session.CurrentTrackData.ToString ());

        // deactivate
        Deactivate ();
    }

    #endregion
}
