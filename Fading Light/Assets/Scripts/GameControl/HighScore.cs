using UnityEngine;
using System.Collections;

/// <summary>
/// Used to store high score data that comes from the server.
/// </summary>
public class HighScore
{
    public string id { get; set; }
    public string name { get; set; }
    public string gold { get; set; }
    public string p1damagegiven { get; set; }
    public string p2damagegiven { get; set; }
    public string p1damagetaken { get; set; }
    public string p2damagetaken { get; set; }
    public string p1accuracy { get; set; }
    public string p2accuracy { get; set; }
}
