/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

using System.Linq;
using UnityEngine;

public class RandomStringUtil
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static Random random = new Random ();

    /// <summary>
    /// Creates an alphanumeric random String with a given length.
    /// </summary>
    public static string RandomString(int length)
    {
        return new string (Enumerable.Repeat (chars, length)
            .Select (s => s [Random.Range (0, s.Length)]).ToArray ());
    }
}
