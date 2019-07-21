/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;

/// <summary>
/// Extension class for dialogs to create generic dialogs
/// that are often used.
/// </summary>
public static class DialogBuilderExtension
{
    /// <summary>
    /// Takes an exception and creates a generic dialog builder ready
    /// to build to display the exception.
    /// </summary>
    public static DialogBuilder MakeGenericExceptionDialog(this DialogBuilder value, Exception _exception )
    {
        return value.SetTitle ("Error!")
            .SetIcon (DialogBuilder.Icon.ERROR)
            .SetMessage (_exception.Message)
            .AddButton ("OK", () => Debug.LogError (_exception));
    }
}
