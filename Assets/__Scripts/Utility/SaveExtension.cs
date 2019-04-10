/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SaveExtension
{
    public static string m_path = Path.Combine ( Application.streamingAssetsPath, "Tracks/JSON/" );

    public static string ConvertToJsonFileName ( this string _name )
    {
        return _name.RemoveWhitespace () + ".json";
    }

    public static TrackData LoadTrackData ( string _fileName )
    {
        string json = File.ReadAllText ( Path.Combine ( m_path, _fileName ) );
        return JsonUtility.FromJson<TrackData> ( json );
    }

    public static void SaveAsJson ( this TrackData _track, string _fileName )
    {
        // save as json
        string json = JsonUtility.ToJson ( _track );
        File.WriteAllText ( Path.Combine ( m_path, _fileName ), json );
    }
}
