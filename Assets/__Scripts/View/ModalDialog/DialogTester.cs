/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Tester class to test out the functionality of the
/// <see cref="DialogBuilder"/>
/// </summary>
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
