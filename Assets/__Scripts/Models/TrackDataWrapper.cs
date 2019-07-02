﻿/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://www.gnu.org/licenses/gpl.txt
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Baguio.Splines
{
    [CreateAssetMenu ( menuName = "Custom/TrackData" )]
    public class TrackDataWrapper : ScriptableObject
    {
        private const string FOLDER = "/Tracks/";
        private const string FILE_ENDING = ".track";

        public TrackData m_track;

        public static string GetPath ()
        {
#if UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets" + FOLDER;
#else
        return Application.persistentDataPath + FOLDER;
#endif
        }

        public static void SaveTrack ( TrackData _track, string _name )
        {
            string json = JsonUtility.ToJson ( _track );
            File.WriteAllText ( GetPath () + _name + FILE_ENDING, json );
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
#endif
        }

        public static bool LoadTrack ( string _name, out TrackData _track )
        {
            // try load the track
            string filename = GetPath () + _name + FILE_ENDING;
            if ( File.Exists ( filename ) )
            {
                try
                {
                    string json = File.ReadAllText ( filename );
                    _track = JsonUtility.FromJson<TrackData> ( json );
                    return true;
                }
                catch ( Exception )
                {
                    Debug.LogWarning ( "There went something wrong while loading the Track" );
                }
            }

            // fallback
            _track = null;
            return false;
        }
    }

}