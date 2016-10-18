using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementSetUp : MonoBehaviour {
    private Achievement[] _achievements;
    public GameObject MenuAchievement;
    public GameObject Grid;

	// Use this for initialization
	void Start () {
        //Getting the achievement list from achievement manager
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        GameData am = (GameData)go.GetComponent(typeof(GameData));
        _achievements = am.Achievements;

        Debug.Log("INSIDE LIST CONTROLLER START");
        foreach(Achievement achievement in am.Achievements)
        {
            Debug.Log(achievement.Name);
            GameObject newAchievement = Instantiate(MenuAchievement) as GameObject;
            AchievementPanel controller = (AchievementPanel)newAchievement.GetComponent(typeof(AchievementPanel));
            if (controller != null)
            {
                controller.Icon.sprite = achievement.IconComplete;
                controller.Name.text = achievement.Name;
                controller.Description.text = achievement.Description;
                controller.Reward.text = achievement.Reward;
                newAchievement.transform.parent = Grid.transform;
                newAchievement.transform.localScale = Vector3.one;
            }
        }
    }
	
}
