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

public class SettingsViewModel : MonoBehaviour, ISettingsViewModel
{
    [SerializeField] protected Text m_scaleXValueLabel;
    [SerializeField] protected Slider m_scaleXSlider;
    [SerializeField] protected Text m_scaleYValueLabel;
    [SerializeField] protected Slider m_scaleYSlider;
    [SerializeField] protected Text m_precisionValueLabel;
    [SerializeField] protected Slider m_precisionSlider;
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
        m_scaleXSlider.onValueChanged.AddListener ( val => { m_scaleXValueLabel.text = val.ToString ( "0.00" ); } );
        m_scaleYSlider.onValueChanged.AddListener ( val => { m_scaleYValueLabel.text = val.ToString ( "0.00" ); } );
        m_precisionSlider.onValueChanged.AddListener ( val => { m_precisionValueLabel.text = val.ToString ( ); } );
        m_discardChangesButton.onClick.AddListener ( Deactivate );
        m_saveChangesButton.onClick.AddListener ( SaveAndDeactivate );
    }

    #endregion

    #region Callbacks

    public void Activate()
    {
        // load
        m_scaleXSlider.value = m_session.CurrentTrackData.m_scale.x;
        m_scaleXSlider.onValueChanged.Invoke ( m_scaleXSlider.value );
        m_scaleYSlider.value = m_session.CurrentTrackData.m_scale.y;
        m_scaleYSlider.onValueChanged.Invoke ( m_scaleYSlider.value );
        m_precisionSlider.value = m_session.CurrentTrackData.m_precision;
        m_precisionSlider.onValueChanged.Invoke ( m_precisionSlider.value );
        m_closedToggle.isOn = m_session.CurrentTrackData.m_closed;

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
        m_session.CurrentTrackData.m_scale.x = m_scaleXSlider.value;
        m_session.CurrentTrackData.m_scale.y = m_scaleYSlider.value;
        m_session.CurrentTrackData.m_precision = Mathf.RoundToInt(m_precisionSlider.value);
        m_session.CurrentTrackData.m_closed = m_closedToggle.isOn;

        // deactivate
        Deactivate ();
    }

    #endregion
}
