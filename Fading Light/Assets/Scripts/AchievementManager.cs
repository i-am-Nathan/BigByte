using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// AchievementManager contains Achievements, which players are able to earn through performing various actions
/// in the game. Each Achievement specifies 
/// </summary>
[System.Serializable]
public class Achievement
{
    public string Name;
    public string Description;
    public Texture2D IconIncomplete;
    public Sprite IconComplete;
    public string Reward;
    public float TargetProgress;
    public bool Secret;

    [HideInInspector]
    public bool Earned = false;
    private float currentProgress = 0.0f;

    /// <summary>
    /// Returns true if this progress added results in the Achievement being earned.
    /// </summary>
    /// <param name="progress">The progress.</param>
    /// <returns></returns>
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


	

    /// <summary>
    ///  Returns true if this progress set results in the Achievement being earned.
    /// </summary>
    /// <param name="progress">The progress.</param>
    /// <returns></returns>
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

/// <summary>
/// 
/// </summary>
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


    /// <summary>
    ///Making this class into global
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
   /* public static AchievementManager Instance {
		get;
		set;
	}*/

    /// <summary>
    ///Call it by using
    ///private AchievementManager achievementManager = AchievementManager.Instance;
    /// </summary>
    void Awake(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("AchievementManager");
		achievementText = GameObject.FindWithTag("Achievement").GetComponent<Text>();

		if(!(objects.Length>0)) {
			
			DontDestroyOnLoad (GameObject.FindWithTag("AchievementManager").gameObject);

			//Instance = this;
		}
	}

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        //GOT THE ACHIEVEMENT POPUP
		AchievementPopup.enabled = false;
   //     ValidateAchievements();
      //  UpdateRewardPointTotals();
    }


    /// <summary>
    /// Make sure some assumptions about achievement data setup are followed.
    /// </summary>
 /*   private void ValidateAchievements()
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
    }*/

    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

   
    /// <summary>
    /// Updating the reward points
    /// </summary>
   /* private void UpdateRewardPointTotals()
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
    }*/


    /// <summary>
    ///When an achievement is earned
    /// </summary>
    private void AchievementEarned()
    {
      //  UpdateRewardPointTotals();
        AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }


    /// <summary>
    ///This is the one that will be called by the playerController
    /// </summary>
    /// <param name="achievementName">Name of the achievement.</param>
    /// <param name="progressAmount">The progress amount.</param>
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
	    Debug.Log("POPUP ACHIEVEMENT");	
            AchievementPopup.enabled = true;
            executedTime = Time.time;
            achievementText.text = achievement.Name;

        }
    }


    /// <summary>
    ///Set the achievement if the progress needs to be reset, e.g. must finish before this time limit
    /// </summary>
    /// <param name="achievementName">Name of the achievement.</param>
    /// <param name="newProgress">The new progress.</param>
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

   
    /// <summary>
    ///Used to make the pop up stay for only 3 seconds
    /// </summary>
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
