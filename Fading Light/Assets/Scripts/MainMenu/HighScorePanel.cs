// file:	Assets\Scripts\MainMenu\HighScorePanel.cs
//
// summary:	Implements the high score panel class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// To be used in future builds for controlling items players can obtain from a shop.
/// </summary>
///
/// <remarks>    . </remarks>

public class HighScorePanel : MonoBehaviour
{
    /// <summary>   Gets the deaths. </summary>
    ///
    /// <value> The deaths. </value>

    public Text Name, Time, Gold, Accuracy, Deaths;
    /// <summary>   The score. </summary>
    public HighScore Score;

    /// <summary>   Populates this object. </summary>
    ///
 

    public void Populate()
    {
        Name.text = Score.name;
        Time.text = Score.TimeString();
        Gold.text = Score.gold;
        Accuracy.text = string.Format("{0}", (float.Parse(Score.p1accuracy ) + float.Parse(Score.p2accuracy)) / 2);
        Deaths.text = Score.timeskilled;
    }

    

}
