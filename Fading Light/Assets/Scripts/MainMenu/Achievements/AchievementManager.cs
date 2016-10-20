// file:	assets\scripts\mainmenu\achievements\achievementmanager.cs
//
// summary:	Implements the achievementmanager class

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>   Manager for achievements. </summary>
///
/// <remarks>    . </remarks>

public class AchievementManager : MonoBehaviour
{
    /// <summary>   The achievement popup. </summary>
    public Canvas AchievementPopup;
    /// <summary>   The achievements. </summary>
    public Achievement[] Achievements;
    /// <summary>   The earned sound. </summary>
    public AudioClip EarnedSound;
    /// <summary>   The graphical user interface style achievement earned. </summary>
    public GUIStyle GUIStyleAchievementEarned;
    /// <summary>   The graphical user interface style achievement not earned. </summary>
    public GUIStyle GUIStyleAchievementNotEarned;


    /// <summary>   The current reward points. </summary>
    private int currentRewardPoints = 0;
    /// <summary>   The potential reward points. </summary>
    private int potentialRewardPoints = 0;
    /// <summary>   The achievement scrollview location. </summary>
    private Vector2 achievementScrollviewLocation = Vector2.zero;

    /// <summary>   The time to wait. </summary>
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 3.0f;

    /// <summary>   The achievement text. </summary>
    private Text achievementText;

    /// <summary>   The data. </summary>
    private GameData Data;

    /// <summary>
    /// Call it by using private AchievementManager achievementManager = AchievementManager.Instance;
    /// </summary>
    ///
 

    void Awake(){
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("AchievementManager");
		achievementText = GameObject.FindWithTag("Achievement").GetComponent<Text>();

		if(!(objects.Length>0)) {
			
			DontDestroyOnLoad (GameObject.FindWithTag("AchievementManager").gameObject);

			//Instance = this;
		}
	}

    /// <summary>   Starts this instance. </summary>
    ///
 

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        Data = (GameData)go.GetComponent(typeof(GameData));
        //GOT THE ACHIEVEMENT POPUP
        AchievementPopup.enabled = false;
   //     ValidateAchievements();
      //  UpdateRewardPointTotals();
    }

    /// <summary>   Gets achievement by name. </summary>
    ///
 
    ///
    /// <param name="achievementName">  Name of the achievement. </param>
    ///
    /// <returns>   The achievement by name. </returns>

    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    /// <summary>   When an achievement is earned. </summary>
    ///
 

    private void AchievementEarned()
    {
      //  UpdateRewardPointTotals();
       AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }

    /// <summary>   Achievement obtained. </summary>
    ///
 
    ///
    /// <param name="achievementName">  Name of the achievement. </param>

    public void AchievementObtained(string achievementName)
    {
       
        Achievement achievement = GetAchievementByName(achievementName);



        if (achievement == null)
        {
            Debug.LogWarning("AchievementManager::AddProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.Earned)
        {
            return;
        }

        achievement.Earned = true;


        achievement.Earned = true;
        AchievementEarned();
        AchievementPopup.enabled = true;
        executedTime = Time.time;
        achievementText.text = achievement.Name;
        //Debug.Log(Data.GameAchievements.Count);
        Data.AddAchievment(new GameAchievement(achievement.Name, achievement.Description));

    }

    /// <summary>
    /// Set the achievement if the progress needs to be reset, e.g. must finish before this time
    /// limit.
    /// </summary>
    ///
 
    ///
    /// <param name="achievementName">  Name of the achievement. </param>
    /// <param name="newProgress">      The new progress. </param>

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

    /// <summary>   Used to make the pop up stay for only 3 seconds. </summary>
    ///
 

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
