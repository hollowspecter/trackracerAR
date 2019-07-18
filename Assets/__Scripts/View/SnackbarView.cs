/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SnackbarView : MonoBehaviour
{
    public Button m_hideButton;
    public Button m_showButton;

    private RectTransform m_rectTransform;
    private Image m_showButtonImage;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform> ();
        m_hideButton.onClick.AddListener (Hide);
        m_showButton.onClick.AddListener (Show);
        m_showButton.gameObject.SetActive (false);
        m_showButtonImage = m_showButton.GetComponent<Image> ();
        m_showButtonImage.DOFade (0f, 0f);
    }

    public void Hide()
    {
        m_rectTransform.DOMoveY (0f, .5f, true).SetAutoKill (false).SetEase (Ease.InOutQuint);
        m_hideButton.interactable = false;
        m_showButton.gameObject.SetActive (true);
        m_showButtonImage.DOFade (1f, 0.5f);
    }

    public void Show()
    {
        m_rectTransform.DOPlayBackwards ();
        m_hideButton.interactable = true;
        m_showButtonImage.DOFade (0f, 0.5f).OnComplete (() => m_showButton.gameObject.SetActive (false));
    }
}
