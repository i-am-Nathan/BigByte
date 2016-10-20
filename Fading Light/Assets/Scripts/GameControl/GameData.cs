using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// Achievement class used to track achievements throughout the game
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
/// Game data which persists through levels and stores all relevant information
/// </summary>
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

    private float _playerOneAccuracy = 0f;
    private float _playerTwoAccuracy = 0f;

    private float _playerOneNumHitsMissed = 0f;
    private float _playerOneNumHitsAchieved = 0f;
    private float _playerTwoNumHitsMissed = 0f;
    private float _playerTwoNumHitsAchieved = 0f;

    private InGameUiManager _inGameUiManager;
    private bool _firstLevel = true;

    private Dictionary<string, int> _player1ItemQuantityDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> _player2ItemQuantityDictionary = new Dictionary<string, int>();
    private List<GameAchievement> GameAchievements = new List<GameAchievement>();

    //Achievements
    public Achievement[] Achievements;
    public Canvas AchievementPopup;
    private Text achievementText;
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 3.0f;

    public bool isMainMenu = false;

	/// <summary>
	/// Adds a new achievement
	/// </summary>
    public void AddAchievment(GameAchievement ac)
    {
        GameAchievements.Add(ac);
    }

	/// <summary>
	/// Returns achievements
	/// </summary>
    public List<GameAchievement> GetGameAchievements()
    {
        return GameAchievements;
    }

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
            // Used to initialise this object with intial game values and assigning a tag
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

	/// <summary>
	/// Start method to initialise appropriate ui manager
	/// </summary>
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

	/// <summary>
	/// Returns an achievement
	/// </summary>
     private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

	/// <summary>
	/// Used to play sounds when an achievement is earned
	/// </summary>
    private void AchievementEarned()
    {
        //  UpdateRewardPointTotals();
        //AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }

	/// <summary>
	/// Closes the pop up for the achievement
	/// </summary>
    void ClosePopUpTimer()
    {
        currentTime = Time.time;
        if (currentTime - executedTime > timeToWait)
        {
            executedTime = 0.0f;
            AchievementPopup.enabled = false;
        }
    }

	/// <summary>
	/// Updating the achievement to check whether it has been met yet
	/// </summary>
    public void AddProgressToAchievement(string achievementName, float progressAmount)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
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

	/// <summary>
	/// Setting player 1 accuracy
	/// </summary>
	public void SetPlayer1Accuracy(float acc) {
		_playerOneAccuracy = acc;
	}

	/// <summary>
	/// Getting player 1 accuracy
	/// </summary>
	public float GetPlayer1Accuracy() {
		float totalSwings = _playerOneNumHitsAchieved + _playerOneNumHitsMissed;
		if (totalSwings != 0) {
			_playerOneAccuracy = _playerOneNumHitsAchieved / totalSwings;
		}
		return _playerOneAccuracy;
	}

	/// <summary>
	/// Setting player 2 accuracy
	/// </summary>
	public void SetPlayer2Accuracy(float acc) {
		_playerTwoAccuracy = acc;
	}

	/// <summary>
	/// Getting player 2 accuracy
	/// </summary>
	public float GetPlayer2Accuracy() {
		float totalSwings = _playerTwoNumHitsAchieved + _playerTwoNumHitsMissed;

		if (totalSwings != 0) {
			_playerTwoAccuracy = _playerTwoNumHitsAchieved / totalSwings;
		}
		return _playerTwoAccuracy;
	}

	/// <summary>
	/// Updating the number of chests missed (chests missed = total chests - chests missed)
	/// </summary>
	public void UpdateChestsMissed() {
		_chestsMissed++;
	}

	/// <summary>
	/// Getting the number of chests missed
	/// </summary>
	public float GetChestsMissed() {
		return _chestsMissed;
	}

	/// <summary>
	/// Updating the number of times the players have been killed
	/// </summary>
	public void UpdateTimesKilled() {
		_timesKilled++;
	}

	/// <summary>
	/// Getting the number of times the players have been killed
	/// </summary>
	public float GetTimesKilled() {
		return _timesKilled;
	}

	/// <summary>
	/// Updating the monsters killed
	/// </summary>
	public void UpdateMonstersKilled() {
		_monstersKilled++;
	}

	/// <summary>
	/// Getting the number of monsters killed
	/// </summary>
	public float GetMonstersKilled() {
		return _monstersKilled;
	}

	/// <summary>
	/// Getting the dictionary of items
	/// </summary>
	public Dictionary<string,int> GetPlayer1ItemQuantityDictionary() {
		return _player1ItemQuantityDictionary;
	}

	/// <summary>
	/// Getting the dictionary of items
	/// </summary>
	public Dictionary<string,int> GetPlayer2ItemQuantityDictionary() {
		return _player2ItemQuantityDictionary;
	}

	/// <summary>
	/// Setting the dictionary of items
	/// </summary>
	public void SetPlayer1ItemQuantityDictionary(string item, int num) {
		_player1ItemQuantityDictionary [item] = num;
	}

	/// <summary>
	/// Setting the dictionary of items
	/// </summary>
	public void SetPlayer2ItemQuantityDictionary(string item, int num) {
		_player2ItemQuantityDictionary [item] = num;
	}

	/// <summary>
	/// Updating the number of times the player has not hit an enemy
	/// </summary>
	public void UpdatePlayerNumHitsMissed (bool isPlayer1) {
		if (isPlayer1) {
			_playerOneNumHitsMissed++;
		} else {
			_playerTwoNumHitsMissed++;
		}
	}

	/// <summary>
	/// Updating the number of times the player has hit an enemy
	/// </summary>
	public void UpdatePlayerNumHitsAchieved (bool isPlayer1) {
		if (isPlayer1) {
			_playerOneNumHitsAchieved++;
		} else {
			_playerTwoNumHitsAchieved++;
		}
	}
}
