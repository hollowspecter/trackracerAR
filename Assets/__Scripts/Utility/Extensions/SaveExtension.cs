/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SaveExtension
{
#if UNITY_EDITOR
    public static string m_path = Path.Combine ( Application.streamingAssetsPath, "Tracks/" );
#else
    public static string m_path = Path.Combine ( Application.persistentDataPath, "Tracks/" );
#endif

    public static string ConvertToJsonFileName ( this string _name )
    {
        return _name.RemoveWhitespace () + ".json";
    }

    public static TrackData LoadTrackData ( string _fileName )
    {
        string json = File.ReadAllText ( Path.Combine ( m_path, _fileName.ConvertToJsonFileName() ) );
        TrackData track = JsonUtility.FromJson<TrackData> ( json );
        Debug.Log ( track.ToString () );
        return track;
    }

    public static void DeleteTrackData (string _fileName)
    {
        try {
            File.Delete (Path.Combine (m_path, _fileName.ConvertToJsonFileName ()));
        } catch (System.Exception e) {
            Debug.LogError (e);
        }
    }

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
