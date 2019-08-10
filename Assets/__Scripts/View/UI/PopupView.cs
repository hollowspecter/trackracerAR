/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using DG.Tweening;

/// <summary>
/// View that can be activated and deactivated using
/// a popup overlay transition.
/// (Used to display the impress)
/// </summary>
public class PopupView : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform> ();
    }

    public void Activate()
    {
        gameObject.SetActive (true);
        rect.DOScaleY (0f, 0.3f).From ();
    }

    public void Deactivate()
    {
        rect.DOScaleY (0f, 0.3f).OnComplete (() => {
            rect.localScale = Vector3.one;
            gameObject.SetActive (false);
        });
    }
}
