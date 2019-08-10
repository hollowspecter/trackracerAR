/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using UnityEngine;
using System.IO;

/// <summary>
/// Extension and Utility class for saving, loading and
/// deleting <see cref="TrackData"/>-Objects to the device
/// memory in JSON format.
/// </summary>
public static class SaveExtension
{
#if UNITY_EDITOR
    public static string m_path = Path.Combine ( Application.streamingAssetsPath, "Tracks/" );
#else
    public static string m_path = Path.Combine ( Application.persistentDataPath, "Tracks/" );
#endif

    /// <summary>
    /// Converts a string to a json filename.
    /// </summary>
    public static string ConvertToJsonFileName ( this string _name )
    {
        return _name.RemoveWhitespace () + ".json";
    }

    /// <summary>
    /// Tries to load a TrackData object from device memory with the
    /// given filename.
    /// </summary>
    public static TrackData LoadTrackData ( string _fileName )
    {
        string json = File.ReadAllText ( Path.Combine ( m_path, _fileName.ConvertToJsonFileName() ) );
        TrackData track = JsonUtility.FromJson<TrackData> ( json );
        Debug.Log ( track.ToString () );
        return track;
    }

    /// <summary>
    /// Deletes a TrackData object from device memory
    /// </summary>
    /// <param name="_fileName"></param>
    public static void DeleteTrackData (string _fileName)
    {
        try {
            File.Delete (Path.Combine (m_path, _fileName.ConvertToJsonFileName ()));
        } catch (System.Exception e) {
            Debug.LogError (e);
        }
    }

    /// <summary>
    /// Tries to save a <see cref="TrackData"/> object as JSON to device memory
    /// </summary>
    public static bool SaveAsJson ( this TrackData _track, string _fileName )
    {
        try {
            // save as json
            string json = JsonUtility.ToJson ( _track );
            string completePath = Path.Combine ( m_path, _fileName.ConvertToJsonFileName () );
            ( new FileInfo ( completePath ) ).Directory.Create ();
            File.WriteAllText ( completePath, json );
            Debug.Log ( "Written Track Data to " + completePath );
            Debug.Log ( _track.ToString () );
            return true;
        }
        catch (System.Exception e ) {
            Debug.LogError ( e );
            return false;
        }
    }
}
