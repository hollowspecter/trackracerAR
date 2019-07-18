/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObserveDialogUseCase
{
    private TracksRepository m_repository;

    public ObserveDialogUseCase(TracksRepository _repository )
    {
        m_repository = _repository;
    }

    public IObservable<bool> EvaluateKey( string _key )
    {
        return m_repository.EvaluateKey (_key);
    }
}
