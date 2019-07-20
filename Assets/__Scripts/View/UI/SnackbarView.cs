/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SnackbarView : MonoBehaviour
{
    private const string PLAYER_PRES_DEFAULT_STRING = "SNACKBAR_";
    private const int SHOW_STATUS = 0;
    private const int HIDE_STATUS = 1;

    public Button m_hideButton;
    public Button m_showButton;
    public Image m_showButtonImage;
    public string m_playerPrefsKey = PLAYER_PRES_DEFAULT_STRING;

    private RectTransform m_rectTransform;
    private float m_originalY;
    private Tweener m_tweener;

    private void Awake()
    {
        if (m_playerPrefsKey == PLAYER_PRES_DEFAULT_STRING ) {
            Debug.LogError ("Please change the player prefs string for this snackbar!", this);
        }

        // init
        m_rectTransform = GetComponent<RectTransform> ();
        m_hideButton.onClick.AddListener (()=>Hide(.5f));
        m_showButton.onClick.AddListener (()=>Show(.5f));
        m_showButton.gameObject.SetActive (false);
        m_showButtonImage.DOFade (0f, 0f);
        m_originalY = m_rectTransform.position.y;

        // check for playerprefs
        if ( PlayerPrefs.HasKey (m_playerPrefsKey) &&
            PlayerPrefs.GetInt (m_playerPrefsKey) == HIDE_STATUS ) {
            Hide (0f);
        }
    }

    public void Hide(float _duration)
    {
        if (m_tweener != null && m_tweener.IsPlaying()) {
            return;
        }
        m_tweener = m_rectTransform.DOMoveY (0f, _duration, true).SetAutoKill (false).SetEase (Ease.InOutQuint);
        m_hideButton.interactable = false;
        m_showButton.gameObject.SetActive (true);
        m_showButtonImage.DOFade (1f, _duration);
        PlayerPrefs.SetInt (m_playerPrefsKey, HIDE_STATUS);
    }

    public void Show(float _duration)
    {
        if ( m_tweener != null && m_tweener.IsPlaying () ) {
            return;
        }
        m_tweener = m_rectTransform.DOMoveY (m_originalY, _duration, true).SetAutoKill (false).SetEase (Ease.InOutQuint);
        m_hideButton.interactable = true;
        m_showButtonImage.DOFade (0f, _duration).OnComplete (() => m_showButton.gameObject.SetActive (false));
        PlayerPrefs.SetInt (m_playerPrefsKey, SHOW_STATUS);
    }
}
