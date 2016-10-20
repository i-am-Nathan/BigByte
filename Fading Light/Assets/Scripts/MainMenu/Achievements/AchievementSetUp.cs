// file:	Assets\Scripts\MainMenu\Achievements\AchievementSetUp.cs
//
// summary:	Implements the achievement set up class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>   An achievement set up. </summary>
///
/// <remarks>    . </remarks>

public class AchievementSetUp : MonoBehaviour {
    /// <summary>   The achievements. </summary>
    private List<GameAchievement> _achievements;
    /// <summary>   The menu achievement. </summary>
    public GameObject MenuAchievement;
    /// <summary>   The grid. </summary>
    public GameObject Grid;

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
        //Getting the achievement list from achievement manager
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        GameData am = go.GetComponent<GameData>();
        _achievements = am.GetGameAchievements();

        Debug.Log("GOLD: " + am.GetTotalTime());
        Debug.Log("Achivements" + _achievements.Count);

        foreach(var achievement in _achievements)
        {
            Debug.Log("LALALALALALALA");
            var listItem = Instantiate(MenuAchievement) as GameObject;
            var panel = (HighScorePanel)listItem.GetComponent(typeof(HighScorePanel));

            if (panel != null && achievement != null)
            {
                panel.Name.text = achievement.Name;
                panel.Deaths.text = achievement.Description;
                panel.transform.parent = Grid.transform;
                panel.transform.localScale = Vector3.one;

            }
        }
    }
	
}
