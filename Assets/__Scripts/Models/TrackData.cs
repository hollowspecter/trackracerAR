/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Baguio.Splines;

[System.Serializable]
public class TrackData
{
    public Vector2 m_scale = new Vector2(0.01f, 0.01f);
    [Range ( 1, 10 )]
    public int m_precision = 5;
    public bool m_closed = true;
    public ShapeData m_shape;
    public Vector3[] m_featurePoints;
}
