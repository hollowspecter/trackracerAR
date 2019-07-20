/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueToggle : Toggle
{
    public int Value { get => m_value; }

    [Header("Value")]
    [SerializeField]
    private int m_value;
}
