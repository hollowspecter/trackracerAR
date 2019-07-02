using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrackDataTest
    {
        /// <summary>
        /// Create a Trackdata
        /// Save it
        /// Load it
        /// Check if it is equal
        /// </summary>
        [Test]
        public void TrackDataSaveAndLoad()
        {
            // Arrange
            TrackData data = new TrackData ();
            TrackData loadedData;
            data.m_scale = Vector2.one;
            data.m_featurePoints = new Vector3 [] { Vector3.zero };
            string filename = "TestTrack";

            // Act
            Debug.Log ( "Before:" );
            Debug.Log ( data.ToString () );
            data.SaveAsJson ( filename );
            loadedData = SaveExtension.LoadTrackData ( filename );

            // Assert
            Debug.Log ( "After:" );
            Debug.Log ( data.ToString () );

            Assert.That ( data.m_scale.Equals ( loadedData.m_scale ) );
            Assert.That ( data.m_precision.Equals ( loadedData.m_precision ) );
            Assert.That ( data.m_closed.Equals ( loadedData.m_closed ) );
            Assert.That ( data.m_featurePoints[0].Equals ( loadedData.m_featurePoints [ 0 ] ) );
        }
    }
}
