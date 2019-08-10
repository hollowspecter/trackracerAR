/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Installs the modal dialog functionality
/// </summary>
public class DialogInstaller : Installer<DialogInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactory<DialogBuilder, DialogBuilder.Factory> ();
        Container.BindFactory<DialogFactory.Params, Dialog, DialogFactory.Factory> ()
            .FromFactory<DialogFactory> ();
    }
}
