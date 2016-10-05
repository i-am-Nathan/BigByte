using UnityEngine;
using System.Linq
using System.Collections;

[System.Serializable]
public class Achievement
{
    public string Name;
    public string Description;
    public Texture2D IconIncomplete;
    public Texture2D IconComplete;
    public int RewardPoints;
    public float TargetProgress;
    public bool Secret;

    [HideInInspector]
    public bool Earned = false;
    private float _currentProgress = 0.0f;

    //Returns true if the progress has been achieved.
    public bool AddProgress(float progress)
    {
        //Achievement is already earned
        if (Earned)
        {
            return false;
        }

        //Adding progress to the achievement
        _currentProgress += progress;
        if(_currentProgress >= TargetProgress)
        {
            Earned = true;
            return true;
        }
        return false;
    }

    public bool SetProgress(float progress)
    {

    }

    //Basic GUI for displaying an achievement.
    public void OnGUI(Rect position, GUIStyle GUIStyleAchievementEarned, GUIStyle GUIStyleAchievementNotEarned)
    {
        GUIStyle style = GUIStyleAchievementNotEarned;
        //ACHIEVEMENT UNLOCKED!!!!
        if (Earned)
        {
            style = GUIStyleAchievementEarned;
        }
        GUIStyleAchievementEarned.BeginGroup(position);
        GUIStyleAchievementEarned.Box(new Rect(0.0f, 0.0f, position.width, position.height), "");

        if (Earned)
        {
            GUIStyleAchievementEarned.Box(new Rect(0.0f, 0.0f, position.height, position.height), IconComplete);
        }
        else
        {
            GUIStyleAchievementEarned.Box(new Rect(0.0f, 0.0f, position.height, position.height), IconIncomplete);
        }
        GUI.Label(new Rect(80.0f, 5.0f, position.width - 80.0f - 50.0f, 25.0f), Name, style);
        
        //Hidden achievements
        if(Secret && !Earned)
        {
            GUI.Label(new Rect(80.0f, 25.0f, position.width - 80.0f, 25.0f), "Decription Hidden!", style);
            GUI.Label(new Rect(position.width - 50.0f, 5.0f, 25.0f, 25.0f), "???", style);
            GUI.Label(new Rect(position.width - 250.0f, 50.0f, 250.0f, 25.0f), "Progress Hidden!", style);
        }
        else
        {
            GUI.Label(new Rect(80.0f, 25.0f, position.width - 08.f, 25.0f), Description, style);
            GUI.Label(new Rect(position.width - 50.0f, 5.0f, 25.0f, 25.0f), RewardPoints.ToString(), style);
            GUI.Label(new Rect(position.width - 250.0f, 50.0f, 250.0f, 25.0f), "Progress: [" + _currentProgress.ToString("0.#") + "out of " + TargetProgress.Totring("0.#") + "]", style);
        }
        GUI.EndGroup();
    }
}

public class AchievementManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
