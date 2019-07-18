﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImpressView : MonoBehaviour
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
