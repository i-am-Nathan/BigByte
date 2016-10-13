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

    public string id;
    public string name;
    public string gold;
    public string p1damagegiven;
    public string p2damagegiven;
    public string p1damagetaken;
    public string p2damagetaken;
    public string p1accuracy;
    public string p2accuracy;
}
