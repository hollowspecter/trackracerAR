/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// TODO SUMMARY
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
