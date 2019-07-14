﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// TODO SUMMARY
public class DialogTester : MonoBehaviour
{
    public string Title = "Debug_Title";
    public string Message = "Debug_Message";
    public DialogBuilder.Icon Icon = DialogBuilder.Icon.NONE;
    public DialogBuilder.ButtonModel [] Buttons;

    private DialogBuilder.Factory m_builderFactory;

    [Inject]
    public void Construct( DialogBuilder.Factory _builderFactory )
    {
        m_builderFactory = _builderFactory;
    }

    public void CreateDialog()
    {
        DialogBuilder builder = m_builderFactory.Create ();
        builder.SetTitle (Title)
            .SetMessage (Message)
            .SetIcon (Icon);

        for (int i=0; i<Buttons.Length; ++i ) {
            builder.AddButton (Buttons [i].ButtonText, DummyCallback);
        }

        builder.Build ();
    }

    private void DummyCallback()
    {
        Debug.Log ("DebugTester: Dummy Callback Called!");
    }
}
