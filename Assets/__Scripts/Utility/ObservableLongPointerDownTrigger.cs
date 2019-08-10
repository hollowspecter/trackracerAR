/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// UniRX Trigger that fires when a long tap on this gameobject happens.
/// </summary>
public class ObservableLongPointerDownTrigger : ObservableTriggerBase
{
    public float m_sqrDistanceThreshold = 0.001f;
    public float m_intervalSecond = .7f;

    private Subject<Unit> onLongPointerDown;
    private float? raiseTime;
    private Vector3 startPosition;

    void Update()
    {
        if ( raiseTime != null && raiseTime <= Time.realtimeSinceStartup ) {
            if ( onLongPointerDown != null ) onLongPointerDown.OnNext (Unit.Default);
            raiseTime = null;
        }
    }

    void OnMouseDown()
    {
        raiseTime = Time.realtimeSinceStartup + m_intervalSecond;
        startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if ( raiseTime != null &&
            Vector3.SqrMagnitude(startPosition - transform.position) >= m_sqrDistanceThreshold) {
            raiseTime = null;
        }
    }

    void OnMouseUp()
    {
        raiseTime = null;
    }

    public IObservable<Unit> OnLongPointerDownAsObservable()
    {
        return onLongPointerDown ?? (onLongPointerDown = new Subject<Unit> ());
    }

    protected override void RaiseOnCompletedOnDestroy()
    {
        if ( onLongPointerDown != null ) {
            onLongPointerDown.OnCompleted ();
        }
    }
}