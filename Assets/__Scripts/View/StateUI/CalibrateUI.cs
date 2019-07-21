﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

/// <summary>
/// Interface for <see cref="ICalibrateState"/>
/// </summary>
public interface ICalibrateUI { }

/// <summary>
/// Manages the UI for the <see cref="CalibrateState"/>.
/// This state UI manages the automatic transition to the
/// next state, if the ARSession has found sufficient
/// trackables to have a stable experience.
/// </summary>
[RequireComponent(typeof(UIFader))]
public class CalibrateUI : MonoBehaviour
{
    public Slider m_progressSlider;
    public Button m_resetButton;
    public Button m_skipButton;

    private ReactiveProperty<int> m_planeSatisfaction;
    private ReactiveProperty<int> m_pointSatisfaction;

    private ICalibrateState m_state;
    private ARPlaneManager m_planeManager;
    private ARPointCloudManager m_pointCloudManager;
    private Settings m_settings;
    private IDisposable m_subscription;
    private DialogBuilder.Factory m_dialogBuilderFactory;

    #region DI

    [Inject]
    private void Construct( ICalibrateState _state,
                           ARPlaneManager _planeManager,
                           ARPointCloudManager _pointCloudManager,
                           Settings _settings,
                           DialogBuilder.Factory _dialogBuilderFactory)
    {
        m_state = _state;
        m_settings = _settings;
        m_planeManager = _planeManager;
        m_pointCloudManager = _pointCloudManager;
        m_dialogBuilderFactory = _dialogBuilderFactory;

        UIFader fader = GetComponent<UIFader> ();
        fader.RegisterStateCallbacks ((State)_state);
        ((State)_state).m_enteredState += Reset;

        m_planeSatisfaction = new ReactiveProperty<int> ();
        m_pointSatisfaction = new ReactiveProperty<int> ();

        m_resetButton.onClick.AddListener (Reset);
        m_skipButton.onClick.AddListener (OnSkipButtonPressed);

        gameObject.SetActive (false);
    }

    #endregion

    #region Unity Lifecycle

    private void OnEnable()
    {
        m_planeManager.planeAdded += OnPlaneAdded;
        m_pointCloudManager.pointCloudUpdated += OnPointCloudUpdated;

        // subscribe to the progress of the plane and point satisfaction and check 
        // if the latest combination suffice
        m_subscription = m_planeSatisfaction
            .CombineLatest (m_pointSatisfaction, ( planeSatisfaction, pointSatisfaction ) => {
                return (m_settings.MinNumberOfPlanes - planeSatisfaction) * (m_settings.PlaneWeight / m_settings.MinNumberOfPlanes) +
                (m_settings.MinNumberOfPoints - pointSatisfaction) * (m_settings.PointWeight / m_settings.MinNumberOfPoints);
            })
            .Subscribe (OnCheckSatisfaction);
    }

    private void OnDisable()
    {
        m_planeManager.planeAdded -= OnPlaneAdded;
        m_subscription?.Dispose ();
    }

    #endregion

    #region Button Callbacks

    private void OnRestart()
    {
        m_state.Restart ();
    }

    private void OnSkipButtonPressed()
    {
        m_dialogBuilderFactory.Create ()
            .SetTitle ("Are you sure you want to skip?")
            .SetIcon (DialogBuilder.Icon.QUESTION)
            .SetMessage ("Skipping this step may result in an instable experience. Make sure you are in a well lit room and have at least one uncluttered visible surface.")
            .AddButton ("Skip", OnStart)
            .AddButton ("Stay")
            .Build ();
    }

    #endregion

    #region Private Methods

    private void Reset()
    {
        m_planeSatisfaction.Value = m_settings.MinNumberOfPlanes;
        m_pointSatisfaction.Value = m_settings.MinNumberOfPoints;
    }

    private void OnStart()
    {
        m_state.DoStart ();
    }

    private void OnCheckSatisfaction( float _satisfactionPercentage )
    {
        m_progressSlider.value = _satisfactionPercentage;

        if ( _satisfactionPercentage >= 1.0f - Mathf.Epsilon ) {
            OnStart ();
        }
    }

    private void OnPlaneAdded(ARPlaneAddedEventArgs args )
    {
        m_planeSatisfaction.Value = Mathf.Max (0, m_planeSatisfaction.Value - 1);
    }

    private void OnPointCloudUpdated( ARPointCloudUpdatedEventArgs obj )
    {
        m_pointSatisfaction.Value = Mathf.Max (0, m_pointSatisfaction.Value - 1);
    }

    #endregion

    #region Settings

    [System.Serializable]
    public class Settings
    {
        public float PlaneWeight = 0.4f;
        public float PointWeight = 0.6f;
        public int MinNumberOfPlanes = 2;
        public int MinNumberOfPoints = 300;
    }

    #endregion
}
