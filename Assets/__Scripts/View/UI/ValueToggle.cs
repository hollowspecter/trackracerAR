/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Extends the <see cref="Toggle"/> with an
/// inherit int value.
/// </summary>
public class ValueToggle : Toggle
{
    public int Value { get => m_value; }

#pragma warning disable CS0649 // unused
#pragma warning disable IDE0044 // Readonly-Modifizierer hinzufügen

    [Header ("Value")]
    [SerializeField]
    private int m_value;

#pragma warning restore CS0649 // unused
#pragma warning restore IDE0044 // Readonly-Modifizierer hinzufügen
}
