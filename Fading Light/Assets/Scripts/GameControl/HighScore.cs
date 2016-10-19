using UnityEngine;
using System.Collections;

/// <summary>
/// Used to store high score data that comes from the server.
/// </summary>
[System.Serializable]
public class HighScore
{
    public static HighScore CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HighScore>(jsonString);
    }

    public string id = string.Empty;
    public string name = string.Empty;
    public string gold = string.Empty;
    public string minutes = string.Empty;
    public string seconds = string.Empty;
    public string monsterskilled = string.Empty;
    public string timeskilled = string.Empty;
    public string chestsmissed = string.Empty;
    public string p1accuracy = string.Empty;
    public string p2accuracy = string.Empty;

    public string ExpandedString()
    {
        return string.Format("{0} completed in the game in {1} minutes and {2} seconds.  They killed {3} monsters and were killed {4} times.  They missed {5} chests and collected {6} gold coins.",
            name, minutes, seconds, monsterskilled, timeskilled, chestsmissed, gold);

    }

    public string TimeString()
    {
        return minutes + " min, " + seconds + " sec";
    }

    public int GetTotalSeconds()
    {
        return int.Parse(minutes) * 60 + int.Parse(seconds);
    }
}
