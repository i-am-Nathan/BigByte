using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListController : MonoBehaviour {
    private Achievement[] _achievements;
    public GameObject MenuAchievement;
    public GameObject Grid;

	// Use this for initialization
	void Start () {
        //Getting the achievement list from achievement manager
        GameObject go = GameObject.FindGameObjectWithTag("AchievementManager");
        AchievementManager am = (AchievementManager)go.GetComponent(typeof(AchievementManager));
        _achievements = am.Achievements;

        Debug.Log("INSIDE LIST CONTROLLER START");
        foreach(Achievement achievement in am.Achievements)
        {
            Debug.Log(achievement.Name);
            GameObject newAchievement = Instantiate(MenuAchievement) as GameObject;
            ListItemController controller = (ListItemController)newAchievement.GetComponent(typeof(ListItemController));
            controller.Icon.sprite = achievement.IconComplete;
            controller.Name.text = achievement.Name;
            controller.Description.text = achievement.Description;
            controller.Reward.text = achievement.Reward;
            newAchievement.transform.parent = Grid.transform;
            newAchievement.transform.localScale = Vector3.one;
        }
    }
	
}
