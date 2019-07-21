/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UniRx;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Model class for <see cref="DiscreteSlider"/>
/// </summary>
public class DiscreteSliderModel
{
    /// <summary>
    /// The current value of the discrete slider
    /// </summary>
    public IReadOnlyReactiveProperty<float> Value { get; }

    /// <summary>
    /// The currently selected index
    /// </summary>
    public ReactiveProperty<int> Index { get; protected set; }

    private float [] values;

    public DiscreteSliderModel(float[] _values, int _initialIndex)
    {
        _values.ThrowIfNull ( nameof ( _values ) );
        values = _values;
        _initialIndex = Mathf.Clamp ( _initialIndex, 0, values.Length );
        Index = new ReactiveProperty<int> ( _initialIndex );
        Value = Index
            .Select ( index => values [index] )
            .ToReactiveProperty ();
    }

    /// <summary>
    /// Sets the closest discrete value
    /// </summary>
    /// <param name="newValue"></param>
    public void setClosestValue( float newValue )
    {
        Index.Value = Array.IndexOf ( values, values.OrderBy ( val => Math.Abs ( newValue - val ) ).First () );
    }
}
