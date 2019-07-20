/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class ObservableLongPointerDownTrigger : ObservableTriggerBase
{
    public float SqrDistanceThreshold = 0.001f;
    public float IntervalSecond = .7f;

    Subject<Unit> onLongPointerDown;

    float? raiseTime;

    Vector3 startPosition;

    void Update()
    {
        if ( raiseTime != null && raiseTime <= Time.realtimeSinceStartup ) {
            if ( onLongPointerDown != null ) onLongPointerDown.OnNext (Unit.Default);
            raiseTime = null;
        }
    }

    void OnMouseDown()
    {
        raiseTime = Time.realtimeSinceStartup + IntervalSecond;
        startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if ( raiseTime != null &&
            Vector3.SqrMagnitude(startPosition - transform.position) >= SqrDistanceThreshold) {
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