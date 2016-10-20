using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementSetUp : MonoBehaviour {
    private List<GameAchievement> _achievements;
    public GameObject MenuAchievement;
    public GameObject Grid;

	// Use this for initialization
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
