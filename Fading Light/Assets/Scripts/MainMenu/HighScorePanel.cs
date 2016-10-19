using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// To be used in future builds for controlling items players can obtain from a shop
/// </summary>
public class HighScorePanel : MonoBehaviour
{

    public Text Name, Time, Gold, Accuracy, Deaths;
    public HighScore Score;

    public void Populate()
    {
        Name.text = Score.name;
        Time.text = Score.TimeString();
        Gold.text = Score.gold;
        Accuracy.text = string.Format("{0}", (float.Parse(Score.p1accuracy ) + float.Parse(Score.p2accuracy)) / 2);
        Deaths.text = Score.timeskilled;
    }

    

}
