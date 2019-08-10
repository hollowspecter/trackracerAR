/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

/// <summary>
/// Controller for the tool that centers the whole track
/// in front of the camera.
/// </summary>
public class CenterTrackTool : MonoBehaviour
{
    /// <summary>
    /// The data for the tracks feature points exist in two places:
    /// First in the child transforms of the track parent <see cref="Point3DView"/>.
    /// Second in the <see cref="BuildStateMachine.CurrentTrackData"/>. This
    /// toggles, which source of points shall be moved.
    /// </summary>
    public bool m_useTransforms = true;

    public Button m_toolButton;

    private Transform m_cameraTransform;
    private Transform m_trackTransform;
    private Settings m_settings;
    private SignalBus m_signalBus;
    private IBuildStateMachine m_buildSM;

    [Inject]
    private void Construct( [Inject (Id = "TrackParent")] Transform _trackTransform,
                            [Inject (Id = "ARCamera")] Transform _cameraTransform,
                            Settings _settings,
                            SignalBus _signalBus,
                            IBuildStateMachine _buildSM)
    {
        m_cameraTransform = _cameraTransform;
        m_trackTransform = _trackTransform;
        m_settings = _settings;
        m_signalBus = _signalBus;
        m_buildSM = _buildSM;

        if (m_useTransforms) {
            m_toolButton.onClick.AddListener (Center);
        } else {
            m_toolButton.onClick.AddListener (OffsetFeaturePoints);
        }
    }

    private void Center()
    {
        Vector3 centerOfTrack = Vector3.zero;
        Transform [] transforms = m_trackTransform.GetComponentsInChildren<Transform> (); // adds parent as first element
        for ( int i = 1; i < transforms.Length; ++i ) { // start at 1 to ignore the parents influence
            centerOfTrack += transforms [i].position;
        }
        centerOfTrack /= transforms.Length - 1;
        Vector3 translate = CalculateTranslate (centerOfTrack);
        for (int i=1; i<transforms.Length; ++i) {
            transforms [i].DOMove (transforms [i].position + translate, .7f).SetEase (Ease.OutQuint);
        }
        Invoke (nameof(SendSignal), .7f);
    }

    private void OffsetFeaturePoints()
    {
        Vector3 centerOfTrack = Vector3.zero;
        int numFeaturePoints = m_buildSM.CurrentTrackData.m_featurePoints.Length;
        for (int i=0; i<numFeaturePoints; ++i ) {
            centerOfTrack += m_buildSM.CurrentTrackData.m_featurePoints [i];
        }
        centerOfTrack /= numFeaturePoints;
        m_buildSM.CurrentFeaturePointOffset = CalculateTranslate(centerOfTrack);

        SendSignal ();
    }

    private Vector3 CalculateTranslate(in Vector3 center)
    {
        Vector3 newTrackCenter = m_cameraTransform.position + m_cameraTransform.forward * m_settings.Offset;
        return newTrackCenter - center;
    }

    private void SendSignal()
    {
        m_signalBus.Fire<FeaturePointChanged> ();
    }

    [Serializable]
    public class Settings
    {
        public float Offset = 1f;
    }
}
