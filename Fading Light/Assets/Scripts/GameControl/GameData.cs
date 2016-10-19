using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

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

public class GameData : MonoBehaviour {

    // Shared attributes between scenes
    private int _numberOfLivesLeft;

    private float _totalTime;
    private int _sharedGold;

    private int _torchFuel;
    private Slider _torchFuelSlider;

    private float _timesKilled;
	private float _monstersKilled;
	private float _chestsMissed;
    //private float _playerTwoTotalDamageTaken;

    private float _playerOneAccuracy;
    private float _playerTwoAccuracy;

	private InGameUiManager _inGameUiManager;
	private bool _firstLevel = true;

	private Dictionary<string,int> _player1ItemQuantityDictionary = new Dictionary<string,int>();
	private Dictionary<string,int> _player2ItemQuantityDictionary = new Dictionary<string,int>();

    //Achievements
    public Achievement[] Achievements;
    public Canvas AchievementPopup;
    //public AudioClip EarnedSound;
    private Text achievementText;
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 3.0f;

    public bool isMainMenu = false;


    /// <summary>
    /// Called before any Start methods called and is used for initialisation
    /// </summary>
    void Awake()
    {
        // Finding existing instances of this game object
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Game Data");     

        // Checking if this game data object already exists
		if (!(objects.Length > 0))
        {
            achievementText = GameObject.FindWithTag("Achievement").GetComponent<Text>();

            // Used to initialise this object with 3 lives and a time of 0
            // Assigning a tag and instantiating number of lives
            _numberOfLivesLeft = 3;
            this.gameObject.tag = "Game Data";
            _totalTime = 0f;
            _sharedGold = 0;
			_timesKilled = 0;
			_monstersKilled = 0;
			_chestsMissed = 0;
			_playerOneAccuracy = 0;
			_playerTwoAccuracy = 0;
			_player1ItemQuantityDictionary.Add ("Health Potion", 0);
			_player1ItemQuantityDictionary.Add ("Attack Potion", 0);
			_player1ItemQuantityDictionary.Add ("Defense Potion",0);
			_player2ItemQuantityDictionary.Add ("Health Potion", 0);
			_player2ItemQuantityDictionary.Add ("Attack Potion", 0);
			_player2ItemQuantityDictionary.Add ("Defense Potion", 0);
            DontDestroyOnLoad(GameObject.FindWithTag("Game Data").gameObject);
        }
    }

	void Start() {
        if (!isMainMenu)
        {
            // Getting the game data object which shows the total lives left
            GameObject go = GameObject.Find("InGameUiManager");
            _inGameUiManager = (InGameUiManager)go.GetComponent(typeof(InGameUiManager));
        }
	}

    /// <summary>
    /// Called every frame to set the total time
    /// </summary>
    void Update()
    {
		// Getting current time since users started playing 
		_totalTime += Time.deltaTime;
    }

    /// <summary>
    /// Used to get the number of shared lives
    /// </summary>
    public int GetNumberOfLives()
    {
        return _numberOfLivesLeft;
    }

    /// <summary>
    /// Used to set the number of shared lives
    /// </summary>
    public void SetNumberOfLives(int lives)
    {
        _numberOfLivesLeft = lives;
    }

	public void UpdateNumberOfLives() {
		// Cannot get more than 3 lives
		if (_numberOfLivesLeft != 3) {
			_numberOfLivesLeft++;
		}
	}

    /// <summary>
    /// Used to get the amount of shared gold
    /// </summary>
    public int GetAmountOfGold()
    {
        return _sharedGold;
    }

	/// <summary>
	/// Used to get the total time
	/// </summary>
	public float GetTotalTime()
	{
		if (_firstLevel) {
			_totalTime = 0f;
			_firstLevel = false;
		}
		return _totalTime;
	}

    /// <summary>
    /// Used to set the amount of shared gold
    /// </summary>
    public void SetAmountOfGold(int gold)
    {
        _sharedGold = gold;
    }


     /// <summary>
    /// Updates the gold.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateGold(int amount)
    {
        _sharedGold += amount;
		_inGameUiManager.UpdateGold (_sharedGold);
    }
     private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);

    }
    private void AchievementEarned()
    {
        //  UpdateRewardPointTotals();
        //AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }
    void ClosePopUpTimer()
    {
        currentTime = Time.time;
        if (currentTime - executedTime > timeToWait)
        {
            executedTime = 0.0f;
            AchievementPopup.enabled = false;
        }
    }

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

	public void SetPlayer1Accuracy(float acc) {
		_playerOneAccuracy = acc;
	}

	public float GetPlayer1Accuracy() {
		return _playerOneAccuracy;
	}

	public void SetPlayer2Accuracy(float acc) {
		_playerTwoAccuracy = acc;
	}

	public float GetPlayer2Accuracy() {
		return _playerTwoAccuracy;
	}

	public void UpdateChestsMissed() {
		_chestsMissed++;
	}

	public float GetChestsMissed() {
		return _chestsMissed;
	}

	public void UpdateTimesKilled() {
		_timesKilled++;
	}

	public float GetTimesKilled() {
		return _timesKilled;
	}

	public void UpdateMonstersKilled() {
		_monstersKilled++;
	}

	public float GetMonstersKilled() {
		return _monstersKilled;
	}

	public Dictionary<string,int> GetPlayer1ItemQuantityDictionary() {
		return _player1ItemQuantityDictionary;
	}

	public Dictionary<string,int> GetPlayer2ItemQuantityDictionary() {
		return _player2ItemQuantityDictionary;
	}

	public void SetPlayer1ItemQuantityDictionary(string item, int num) {
		_player1ItemQuantityDictionary [item] = num;
	}

	public void SetPlayer2ItemQuantityDictionary(string item, int num) {
		_player2ItemQuantityDictionary [item] = num;
	}
}
