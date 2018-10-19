using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Inputs : MonoBehaviour
{
    public ReadOnlyReactiveProperty<bool> Escape { get; private set; }

    private void Awake ()
    {
        Escape = this.UpdateAsObservable ()
            .Select ( _ => Input.GetKey ( KeyCode.Escape ) )
            .ToReadOnlyReactiveProperty ();
    }
}
