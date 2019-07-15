﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TODO SUMMARY
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI m_title;
    public Image m_icon;
    public TextMeshProUGUI m_message;
    public Transform m_buttonPanel;

    private GameObject m_buttonPrefab;

    public void SetButtonPrefab(GameObject _buttonPrefab)
    {
        m_buttonPrefab = _buttonPrefab;
    }

    public void AddButton(DialogBuilder.ButtonModel _buttonModel)
    {
        DialogButton button = Instantiate (m_buttonPrefab, m_buttonPanel).GetComponent<DialogButton> ();
        button.m_text.text = _buttonModel.ButtonText;
        button.m_button.onClick.AddListener (() => {
            _buttonModel.ButtonCallback?.Invoke ();
            Deactivate ();
        });
    }

    private void Deactivate()
    {
        Destroy (gameObject);
    }
}