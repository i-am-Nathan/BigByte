using UnityEngine;
using System.Linq;
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
        if (Earned)
        {
            return false;
        }
        _currentProgress = progress;
        if (progress >= TargetProgress)
        {
            Earned = true;
            return true;
        }
        return false;
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

    public Achievement[] Achievements;
    public GUIStyle GUIStyleAchievementEarned;
    public GUIStyle GUIStyleAchievementNotEarned;

    private int _currentRewardPoints = 0;
    private int _potentialRewardPoints = 0;
    private Vector2 _achievementScrollViewLocation = Vector2.zero;
	// Use this for initialization
	void Start () {
        ValidateAchievement();
        UpdateRewardPointTotals();
	}
	
    //Checkif the achievements are valid
    private void ValidateAchievements()
    {
        ArrayList usedNames = new ArrayList();
        foreach(Achievement achievement in Achievements)
        {
            if(achievement.RewardPoints < 0)
            {
                Debug.LogError("Achievement with negative RewardPoints " + achievement.Name);
            }
            if (usedNames.Contains(achievement.Name))
            {
                Debug.LogError("Duplicate achievement names " + achievement.Name);
            }
            usedNames.Add(achievement.Name);
        }
    }

    private void UpdateRewardPointTotals()
    {
        _currentRewardPoints = 0;
        _potentialRewardPoints = 0;

        foreach(Achievement achievement in Achievements)
        {
            if (achievement.Earned)
            {
                _currentRewardPoints += achievment.RewardPoints;
            }
            _potentialRewardPoints += achievement.RewardPoints;
        }
    }
    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    private void AchievementEarned()
    {
        UpdateRewardPointTotals();
        
    }
    //Incrementing progress to an achievement
    public void AddProgressToAchievement(string achievementName, float progressAmount)
    {
        Achievement achievement = GetAchevementByName(achievementName);
        if(achievement == null)
        {
            Debug.LogWarning("Trying to add progress to an achievement that doesn't exist " + achievementName);
            return;
        }
        if (achievement.AddProgress(progressAmount))
        {
            AchievementEarned();
        }
    }

    public void SetProgressToAchievement(string achievementName, float newProgress)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.LogWarning("Trying to add progress to an achievement that doesn't exist " + achievementName);
            return;
        }
        if (achievement.SetProgress(newProgress))
        {
            AchievementEarned();
        }
    }

    void OnGui()
    {
        float yValue = 5.0f;
        float achievementGUIWidth = 500.0f;

        GUI.Label(new Rect(200.0f, 5.0f, 200.0f, 25.0f), "--Achievements--");
        _achievementScrollViewLocation = GUI.BeginScrollView(new Rect(0.0f, 25.0f, achievementGUIWidth + 25.0f, 400.0f), _achievementScrollViewLocation,
                                                             new Rect(0.0f, 0.0f, achievementGUIWidth, Achievement.Count() * 80.0f));
        foreach(Achievement achievement in Achievements)
        {
            Rect position = new Rect(5.0f, yValue, achievementGUIWidth, 75.0f);
            achievement.OnGUI(position, GUIStyleAchievementEarned, GUIStyleAchievementNotEarned);
            yValue += 80.0f;
        }
        GUI.EndScrollView();
        GUI.Label(new Rect(10.0f, 440.0f, 200.0f, 25.0f), "Reward Points: [" + _currentRewardPoints + " out of " + _potentialRewardPoints + "]");
    }
}
