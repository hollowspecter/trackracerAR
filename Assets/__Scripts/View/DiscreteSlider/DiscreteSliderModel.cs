/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UniRx;
using UnityEngine;

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
        _initialIndex = Mathf.Clamp ( _initialIndex, 0, _values.Length );
        Index = new ReactiveProperty<int> ( _initialIndex );
        Value = Index
            .Select ( index => values [ Mathf.Clamp ( index, 0, _values.Length )] )
            .ToReactiveProperty ();
    }
}
