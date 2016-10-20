// file:	Assets\Scripts\GameControl\HighScore.cs
//
// summary:	Implements the high score class

using UnityEngine;
using System.Collections;

/// <summary>   Used to store high score data that comes from the server. </summary>
///
/// <remarks>    . </remarks>

[System.Serializable]
public class HighScore
{
    /// <summary>   Creates from JSON. </summary>
    ///
 
    ///
    /// <param name="jsonString">   The JSON string. </param>
    ///
    /// <returns>   The new from JSON. </returns>

    public static HighScore CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HighScore>(jsonString);
    }

	// Attributes which are stored in the database
    /// <summary>   The identifier. </summary>
    public string id = string.Empty;
    /// <summary>   The name. </summary>
    public string name = string.Empty;
    /// <summary>   The gold. </summary>
    public string gold = string.Empty;
    /// <summary>   The minutes. </summary>
    public string minutes = string.Empty;
    /// <summary>   The seconds. </summary>
    public string seconds = string.Empty;
    /// <summary>   The monsterskilled. </summary>
    public string monsterskilled = string.Empty;
    /// <summary>   The timeskilled. </summary>
    public string timeskilled = string.Empty;
    /// <summary>   The chestsmissed. </summary>
    public string chestsmissed = string.Empty;
    /// <summary>   The 1accuracy. </summary>
    public string p1accuracy = string.Empty;
    /// <summary>   The 2accuracy. </summary>
    public string p2accuracy = string.Empty;

	// String shown on the highscore menu

    /// <summary>   Expanded string. </summary>
    ///
 
    ///
    /// <returns>   A string. </returns>

    public string ExpandedString()
    {
        return string.Format("{0} completed in the game in {1} minutes and {2} seconds.  They killed {3} monsters and were killed {4} times.  They missed {5} chests and collected {6} gold coins.",
            name, minutes, seconds, monsterskilled, timeskilled, chestsmissed, gold);

    }

    /// <summary>   Time string. </summary>
    ///
 
    ///
    /// <returns>   A string. </returns>

    public string TimeString()
    {
        return minutes + " min, " + seconds + " sec";
    }

    /// <summary>   Gets total seconds. </summary>
    ///
 
    ///
    /// <returns>   The total seconds. </returns>

    public int GetTotalSeconds()
    {
        return int.Parse(minutes) * 60 + int.Parse(seconds);
    }
}
