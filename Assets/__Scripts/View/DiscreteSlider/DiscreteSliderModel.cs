/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */
using UniRx;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// TODO
/// </summary>
public class DiscreteSliderModel
{
    public IReadOnlyReactiveProperty<float> Value { get; }
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

    // TODO write tests?
    public void setClosestValue( float newValue )
    {
        Index.Value = Array.IndexOf ( values, values.OrderBy ( val => Math.Abs ( newValue - val ) ).First () );
    }
}
