using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// AchievementManager contains Achievements, which players are able to earn through performing various actions
// in the game. Each Achievement specifies 

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
    private float currentProgress = 0.0f;

    // Returns true if this progress added results in the Achievement being earned.
    public bool AddProgress(float progress)
    {
        if (Earned)
        {
            return false;
        }

        currentProgress += progress;
        if (currentProgress >= TargetProgress)
        {
            Earned = true;
            return true;
        }

        return false;
    }

    // Returns true if this progress set results in the Achievement being earned.
    public bool SetProgress(float progress)
    {
        if (Earned)
        {
            return false;
        }

        currentProgress = progress;
        if (progress >= TargetProgress)
        {
            Earned = true;
            return true;
        }

        return false;
    }
		
}

public class AchievementManager : MonoBehaviour
{
    public Canvas AchievementPopup;
    public Achievement[] Achievements;
    public AudioClip EarnedSound;
    public GUIStyle GUIStyleAchievementEarned;
    public GUIStyle GUIStyleAchievementNotEarned;


    private int currentRewardPoints = 0;
    private int potentialRewardPoints = 0;
    private Vector2 achievementScrollviewLocation = Vector2.zero;

    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 3.0f;

    private Text achievementText;

	//Making this class into global
	public static AchievementManager Instance {
		get;
		set;
	}
	//Call it by using
	//private AchievementManager achievementManager = AchievementManager.Instance;
	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
		Instance = this;
	}
    void Start()
    {
        //GOT THE ACHIEVEMENT POPUP
        achievementText = GameObject.FindWithTag("Achievement").GetComponent<Text>();
        AchievementPopup.enabled = false;

        ValidateAchievements();
        UpdateRewardPointTotals();
    }

    // Make sure some assumptions about achievement data setup are followed.
    private void ValidateAchievements()
    {
        ArrayList usedNames = new ArrayList();
        foreach (Achievement achievement in Achievements)
        {
            if (achievement.RewardPoints < 0)
            {
                Debug.LogError("AchievementManager::ValidateAchievements() - Achievement with negative RewardPoints! " + achievement.Name + " gives " + achievement.RewardPoints + " points!");
            }

            if (usedNames.Contains(achievement.Name))
            {
                Debug.LogError("AchievementManager::ValidateAchievements() - Duplicate achievement names! " + achievement.Name + " found more than once!");
            }
            usedNames.Add(achievement.Name);
        }
    }

    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    //Updating the reward points
    private void UpdateRewardPointTotals()
    {
        currentRewardPoints = 0;
        potentialRewardPoints = 0;

        foreach (Achievement achievement in Achievements)
        {
            if (achievement.Earned)
            {
                currentRewardPoints += achievement.RewardPoints;
            }

            potentialRewardPoints += achievement.RewardPoints;
        }
    }

    //When an achievement is earned
    private void AchievementEarned()
    {
        UpdateRewardPointTotals();
        AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }

    //This is the one that will be called by the playerController
    public void AddProgressToAchievement(string achievementName, float progressAmount)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.LogWarning("AchievementManager::AddProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

		//If the achievement has been earned than create a pop up for the achievement for 3 seconds
        if (achievement.AddProgress(progressAmount))
        {
            AchievementEarned();
            AchievementPopup.enabled = true;
            executedTime = Time.time;
            achievementText.text = achievement.Name;

        }
    }

	//Set the achievement if the progress needs to be reset, e.g. must finish before this time limit
    public void SetProgressToAchievement(string achievementName, float newProgress)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.LogWarning("AchievementManager::SetProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.SetProgress(newProgress))
        {
            AchievementEarned();
        }
    }

 	//Used to make the pop up stay for only 3 seconds
    void Update()
    {
        currentTime = Time.time;
        if (currentTime - executedTime > timeToWait)
        {
            executedTime = 0.0f;
            AchievementPopup.enabled = false;
        }
    }

}
