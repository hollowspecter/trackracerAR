/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

// TODO SUMMARY
public class DialogBuilder
{
    public enum Icon
    {
        ALERT = 0,
        ADD = 1,
        CREATE = 2,
        ERROR = 3,
        QUESTION = 4,
        ALERT_2 = 5,
        STAR = 6,
        WARNING = 7,
        INFO = 8,
        NONE
    };

    private DialogFactory.Factory m_factory;
    private Settings m_settings;

    private string m_title = null;
    private string m_message = null;
    private Icon m_icon = Icon.NONE;
    private List<ButtonModel> m_buttons;

    DialogBuilder(DialogFactory.Factory _dialogFactory,
                         Settings _settings)
    {
        m_factory = _dialogFactory;
        m_settings = _settings;
        m_buttons = new List<ButtonModel> ();
    }

    public DialogBuilder SetTitle(string _title)
    {
        m_title = _title;
        return this;
    }

    public DialogBuilder SetMessage(string _message )
    {
        m_message = _message;
        return this;
    }

    public DialogBuilder SetIcon(Icon _icon)
    {
        m_icon = _icon;
        return this;
    }

    public DialogBuilder AddButton(string _buttonText, UnityAction _callback = null)
    {
        m_buttons.Add (new ButtonModel (_buttonText, _callback));
        return this;
    }

    public Dialog Build()
    {
        Dialog dialog = m_factory.Create (new DialogFactory.Params ());
        dialog.SetButtonPrefab (m_settings.ButtonPrefab);
        dialog.m_title.text = m_title;
        dialog.m_message.text = m_message;
        if (m_icon != Icon.NONE) {
            dialog.m_icon.sprite = m_settings.Icons [(int)m_icon];
            dialog.m_icon.gameObject.SetActive (true);
        }
        if (m_buttons.Count == 0) {
            m_buttons.Add (new ButtonModel (m_settings.DefaultButtonText, null));
        }
        foreach(ButtonModel b in m_buttons) {
            dialog.AddButton (b);
        }
        return dialog;
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject ButtonPrefab;
        [Header("Match the DialogBuilder.Icon Enum!")]
        public Sprite[] Icons;
        public string DefaultButtonText;
    }

    [System.Serializable]
    public class ButtonModel
    {
        public string ButtonText;
        public UnityAction ButtonCallback;

        public ButtonModel( string _text, UnityAction _callback )
        {
            ButtonText = _text;
            ButtonCallback = _callback;
        }
    }

    public class Factory : PlaceholderFactory<DialogBuilder> { }
}
