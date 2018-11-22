using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SaveExtension
{
    private static string m_path = Path.Combine ( Application.streamingAssetsPath, "Tracks/JSON/" );

    public static void SaveAsJson ( this TrackModel _track, string _fileName )
    {
        TrackDataStructure trackData = new TrackDataStructure ();
        List<int> indexList = new List<int> ();
        TrackPart root = _track.GetRoot ();
        TrackPart currentTrack = root;

        // traverse the tree until root or null are hit
        while ( currentTrack.NextPart != null &&
               currentTrack.NextPart != root )
        {
            indexList.Add ( currentTrack.Index );
            currentTrack = currentTrack.NextPart;
        }
        trackData.m_trackIndices = indexList.ToArray ();

        // save as json
        string json = JsonUtility.ToJson ( trackData );
        File.WriteAllText ( Path.Combine ( m_path, _fileName ), json );
    }
}
