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
    public ShapeData Shape { get { return Configuration.ShapeDatas [m_shapeIndex]; } }
    public Material Material { get { return Configuration.StreetMaterials [m_materialIndex]; } }

    public Vector2 m_scale = new Vector2(0.01f, 0.01f);
    [Range ( 1, 10 )]
    public int m_precision = 5;
    public bool m_closed = true;
    public int m_shapeIndex = 0;
    public int m_materialIndex = 0;
    public Vector3[] m_featurePoints;
    public string m_dbKey;
    public bool m_updateToCloud = false;

    public override string ToString()
    {
        return string.Format ( "Scale: {0}\n" +
                             "Precision: {1}\n" +
                             "Closed: {2}\n" +
                             "Shape: {3}\n" +
                             "Number of Featurepoints: {4}\n" +
                             "DB Key: {5}\n" +
                             "Update to Cloud: {6}\n",
                             "Material Index: {7}\n",
                             m_scale,
                             m_precision,
                             m_closed,
                             Shape,
                             m_featurePoints.Length,
                             m_dbKey,
                             m_updateToCloud,
                             m_materialIndex);
    }
}
