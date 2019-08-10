/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using Baguio.Splines;

/// <summary>
/// Model for TrackData.
/// Contains feature points and settings for a track.
/// TrackData is saved and loaded using the <see cref="SaveExtension"/>
/// extension methods.
/// By utilizing the JSON serializer, the tracks will be saved in
/// JSON format and uploaded to the database as such as well.
/// </summary>
[System.Serializable]
public class TrackData
{
    /// <summary>
    /// The <see cref="ShapeData"/> object for this tracks shape
    /// </summary>
    public ShapeData Shape { get { return Configuration.ShapeDatas [m_shapeIndex]; } }

    /// <summary>
    /// The Material for this track
    /// </summary>
    public Material Material { get { return Configuration.StreetMaterials [m_materialIndex]; } }

    /// <summary>
    /// The scale of the shape data in x and y
    /// </summary>
    public Vector2 m_scale = new Vector2(0.01f, 0.01f);

    /// <summary>
    /// Determines the number of points (2 to the power of m_precision)
    /// that are being calculated on one segment of the catmull rom spline.
    /// </summary>
    [Range ( 1, 10 )]
    public int m_precision = 5;

    /// <summary>
    /// Determines if the track is closed or a sprint
    /// </summary>
    public bool m_closed = true;

    /// <summary>
    /// Determines the street shape from <see cref="Configuration.ShapeDatas"/>
    /// </summary>
    public int m_shapeIndex = 0;

    /// <summary>
    /// Determines the material from <see cref="Configuration.StreetMaterials"/>
    /// </summary>
    public int m_materialIndex = 0;

    /// <summary>
    /// The characteristic feature points from which the track will be
    /// generated from
    /// </summary>
    public Vector3[] m_featurePoints;

    /// <summary>
    /// The key of this track to identify in the database.
    /// If this key is null, this track has not been uploaded to the
    /// database yet.
    /// </summary>
    public string m_dbKey;

    /// <summary>
    /// Determines, if this track is being uploaded and updated
    /// to the cloud database
    /// </summary>
    public bool m_updateToCloud = false;

    /// <summary>
    /// Returns a valid <see cref="TrackData"/> object with various
    /// randomized settings
    /// </summary>
    /// <returns></returns>
    public static TrackData CreateRandomizedTrackData()
    {
        TrackData track = new TrackData ();
        track.m_shapeIndex = Random.Range (0, Configuration.ShapeDatas.Length);
        track.m_materialIndex = Random.Range (0, Configuration.StreetMaterials.Length);
        return track;
    }

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
