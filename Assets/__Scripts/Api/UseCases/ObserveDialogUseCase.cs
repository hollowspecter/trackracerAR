/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Usecase to 
/// </summary>
public class CheckKeyUseCase
{
    private TracksRepository m_repository;

    public CheckKeyUseCase(TracksRepository _repository )
    {
        m_repository = _repository;
    }

    public IObservable<bool> EvaluateKey( string _key )
    {
        return m_repository.EvaluateKey (_key);
    }
}
