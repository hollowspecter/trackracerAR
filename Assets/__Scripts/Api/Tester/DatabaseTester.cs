﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using Zenject;
using UniRx;
using Baguio.Splines;

/// <summary>
/// Use this component to test the functionality of the
/// database interface.
/// </summary>
public class DatabaseTester : MonoBehaviour, IBuildStateMachine
{
    public TrackData CurrentTrackData { get => m_trackData; set => value = m_trackData; }
    public Vector3 CurrentFeaturePointOffset { get; set; }
    public bool ReturnToPreviousStateFlag { get; set; }

#pragma warning disable CS0067 // unused
    public event State.TouchHandler m_touchDetected;
#pragma warning restore CS0067 // unused

    public TrackData m_trackData;
    public string m_keyToLoad;
    
    private TracksRepository m_repository;
    private ISplineManager m_splineManager;
    private IDisposable m_subscription;

    [Inject]
    private void Construct( TracksRepository _repository,
                           [Inject (Id = "TrackParent")] ISplineManager _splineManager)
    {
        m_repository = _repository;
        m_splineManager = _splineManager;
    }

    public void OnPushToDB()
    {
        Debug.Log ("Start pushing to DB!");
        m_repository.PushTrack (m_trackData)
            .Subscribe (key => Debug.Log ("Successful! " + key))
            .AddTo (this);
    }

    public void OnGetTrackOnce()
    {
        Debug.Log ("Start Get Track once!");
        m_repository.GetTrackOnce (m_keyToLoad)
            .SubscribeOn (Scheduler.ThreadPool)
            .ObserveOnMainThread ()
            .Subscribe (UpdateTrack, Debug.LogError, () => { Debug.Log ("Successful!"); })
            .AddTo (this);
    }

    public void OnObserveTrack()
    {
        Debug.Log ("Start observing track changes");
        m_subscription = m_repository.ObserveTrack (m_keyToLoad)
            .SubscribeOn (Scheduler.ThreadPool)
            .ObserveOnMainThread ()
            .Subscribe (UpdateTrack, Debug.LogError, () => { Debug.Log ("Successful!"); })
            .AddTo (this);
    }

    public void OnStopObservingTrackChanges()
    {
        Debug.Log ("Stop observing track changes");
        m_subscription?.Dispose ();
    }

    public void OnEvaluateKey()
    {
        Debug.Log ("Start checking if key exists");
        m_repository.EvaluateKey (m_keyToLoad)
            .Subscribe (keyExists => {
                if ( keyExists ) {
                    Debug.Log ("Key Exists!");
                } else {
                    Debug.Log ("Key does not exist!");
                }
            }, Debug.LogError, () => { })
            .AddTo (this);
    }

    private void UpdateTrack(TrackData _track )
    {
        Debug.Log ("Update came through!");
        m_trackData = _track;
        m_splineManager.GenerateTrackFromTrackData ();
    }
}
