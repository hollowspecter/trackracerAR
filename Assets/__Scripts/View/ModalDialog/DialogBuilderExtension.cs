/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogBuilderExtension
{
    public static DialogBuilder MakeGenericExceptionDialog(this DialogBuilder value, Exception _exception )
    {
        return value.SetTitle ("Error!")
            .SetIcon (DialogBuilder.Icon.ERROR)
            .SetMessage (_exception.Message)
            .AddButton ("OK", () => Debug.LogError (_exception));
    }
}
