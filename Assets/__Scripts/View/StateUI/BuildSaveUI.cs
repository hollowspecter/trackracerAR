/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;

public interface IBuildSaveUI
{
}

[RequireComponent ( typeof ( UIFader ) )]
public class BuildSaveUI : MonoBehaviour, IBuildSaveUI
{
    public Toggle m_updateToCloudToggle;

    private IBuildStateMachine m_session;
    private IBuildSaveState m_state;
    private UIFader m_fader;

    private Text m_validateOutputText;
    private Button m_saveButton;
    private InputField m_inputNameField;
    private DialogBuilder.Factory m_dialogBuilderFactory;
    private CompositeDisposable m_subscriptions;
    private UpdateUseCase m_useCase;

    [Inject]
    private void Construct ( IBuildSaveState _state,
                            [Inject ( Id = "TextValidateOutput" )] Text _validateOutputText,
                            [Inject ( Id = "InputName" )] InputField _inputNameField,
                            [Inject ( Id = "ButtonSaveTrack" )] Button _saveButton,
                            [Inject ( Id = "ButtonCancel" )] Button _cancelButton,
                            [Inject ( Id = "ButtonShare" )] Button _shareButton,
                            DialogBuilder.Factory _dialogBuilderFactory,
                            IBuildStateMachine _buildSM,
                            UpdateUseCase _useCase)
    {
        m_state = _state;
        m_session = _buildSM;
        m_useCase = _useCase;
        m_fader = GetComponent<UIFader> ();
        m_fader.RegisterStateCallbacks ( ( State ) m_state );

        m_validateOutputText = _validateOutputText;
        m_validateOutputText.text = "Please enter a name for the Track";
        m_inputNameField = _inputNameField;
        m_inputNameField.onValueChanged.AddListener ( OnValueChanged );
        m_inputNameField.text = "";
        m_saveButton = _saveButton;
        m_saveButton.onClick.AddListener ( OnSaveButtonPressed );
        m_saveButton.interactable = false;
        _cancelButton.onClick.AddListener ( OnCancelButtonPressed );
        m_dialogBuilderFactory = _dialogBuilderFactory;
        _shareButton.onClick.AddListener (OnShareButtonPressed);

        gameObject.SetActive ( false );
    }

    protected void OnValueChanged ( string _value )
    {
        if ( string.IsNullOrEmpty ( _value ) )
        {
            m_validateOutputText.text = "Please enter a name for the Track";
            m_saveButton.interactable = false;
        }
        else
        {
            m_saveButton.interactable = true;

            if ( System.IO.File.Exists ( System.IO.Path.Combine ( SaveExtension.m_path, _value.ConvertToJsonFileName () ) ) )
            {
                Debug.Log ( "File already exists!" );
                m_validateOutputText.text = "Careful, this file already exists! It will be overwritten!";
            }
            else
            {
                m_validateOutputText.text = "Press save if you wanna";
            }
        }
    }

    protected void OnSaveButtonPressed ()
    {
        DialogBuilder builder = m_dialogBuilderFactory.Create ();

        // if save is successful
        if (m_state.OnSave ( m_inputNameField.text )) {
            builder.SetTitle ("Save successful!")
                .SetMessage ("Would you like to keep editing this track or start a new one?\n" +
                             "Or you can race it immediately!")
                .SetIcon(DialogBuilder.Icon.STAR)
                .AddButton ("Keep Editing", m_state.OnCancel)
                .AddButton ("New Track", m_state.OnNewTrack)
                .AddButton ("Race!", m_state.OnDone);
        } else {
            builder.SetTitle ("Save unsuccessful!")
                .SetMessage ("Please check the storage of your device or contact the developer.")
                .SetIcon(DialogBuilder.Icon.WARNING)
                .AddButton ("Back")
                .AddButton ("Try Again", () => m_state.OnSave(m_inputNameField.text));
        }

        builder.Build ();
    }

    protected void OnCancelButtonPressed ()
    {
        m_state.OnCancel ();
    }

    // todo refactor this bc it was copied from editorviewmodel
    protected void OnShareButtonPressed()
    {
        string key = m_state.OnShare ();
        if ( string.IsNullOrWhiteSpace (key) ) {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Not uploaded yet!")
                .SetIcon (DialogBuilder.Icon.INFO)
                .SetMessage ("If you would like to share this track, go to the save menu and upload this track to the cloud.")
                .Build ();
        } else {
            m_dialogBuilderFactory.Create ()
                .SetTitle ("Success!")
                .SetMessage (string.Format ("The key to the track\n{0}\nwas copied into your clipboard. Share it with your friends now!", key))
                .Build ();
        }
    }

    private void OnEnable()
    {
        m_subscriptions = new CompositeDisposable ();
        m_updateToCloudToggle.isOn = m_session.CurrentTrackData.m_updateToCloud;
        m_subscriptions.Add(m_updateToCloudToggle.OnValueChangedAsObservable ()
            .Subscribe (isOn => {
                // update session
                m_session.CurrentTrackData.m_updateToCloud = isOn;
                // update to cloud if turned on
                if (isOn) {
                    m_subscriptions.Add (m_useCase.UpdateTrackToCloud (m_session.CurrentTrackData)
                        .Subscribe(_=> { }));
                }
            }));
    }

    private void OnDisable()
    {
        m_subscriptions?.Dispose ();
    }
}
