/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;

/// <summary>
/// Usecase to be used in the observe dialog to check if a key exists in the cloud.
/// </summary>
public class ObserveDialogUseCase
{
    private TracksRepository m_repository;

    public ObserveDialogUseCase( TracksRepository _repository )
    {
        m_repository = _repository;
    }

    /// <summary>
    /// Checks if a key exists in the firebase.
    /// </summary>
    /// <returns>an Observable that emits true if key exists, or else if doesn't or problems occured</returns>
    public IObservable<bool> EvaluateKey( string _key )
    {
        return m_repository.EvaluateKey (_key);
    }
}