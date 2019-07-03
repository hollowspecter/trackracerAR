/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class DiscreteSliderModel
{
    public ReactiveProperty<float> value { get; protected set; }

    public DiscreteSliderModel(float initialValue)
    {
        value = new ReactiveProperty<float> ( initialValue );
    }
}
