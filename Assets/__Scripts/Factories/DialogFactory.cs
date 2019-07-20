/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Zenject;

/// <summary>
/// Prefab-instantiating factory to create Dialog-Canvases
/// Todo Improvement: Convert to MemoryPool
/// </summary>
public class DialogFactory : IFactory<DialogFactory.Params, Dialog>
{
    protected readonly DiContainer m_container;
    protected Settings m_settings;

    public class Params { }

    public class Factory : PlaceholderFactory<Params, Dialog> { }

    public DialogFactory(DiContainer _container, Settings _settings)
    {
        m_container = _container;
        m_settings = _settings;
    }

    public Dialog Create(Params _params)
    {
        return m_container.InstantiatePrefab (m_settings.Prefab).GetComponent<Dialog> ();
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject Prefab;
    }
}
