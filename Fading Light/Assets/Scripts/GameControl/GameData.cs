// file:	Assets\Scripts\GameControl\GameData.cs
//
// summary:	Implements the game data class

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>   Achievement class used to track achievements throughout the game. </summary>
///
/// <remarks>    . </remarks>

[System.Serializable]
public class Achievement
{
    /// <summary>   The name. </summary>
    public string Name;
    /// <summary>   The description. </summary>
    public string Description;
    /// <summary>   The icon incomplete. </summary>
    public Texture2D IconIncomplete;
    /// <summary>   The icon complete. </summary>
    public Sprite IconComplete;
    /// <summary>   The reward. </summary>
    public string Reward;
    /// <summary>   Target progress. </summary>
    public float TargetProgress;
    /// <summary>   True to secret. </summary>
    public bool Secret;

    /// <summary>   True if earned. </summary>
    [HideInInspector]
    public bool Earned = false;
    /// <summary>   The current progress. </summary>
    private float currentProgress = 0.0f;

    /// <summary>
    /// Returns true if this progress added results in the Achievement being earned.
    /// </summary>
    ///
 
    ///
    /// <param name="progress"> The progress. </param>
    ///
    /// <returns>   True if it succeeds, false if it fails. </returns>

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

    /// <summary>   Returns true if this progress set results in the Achievement being earned. </summary>
    ///
 
    ///
    /// <param name="progress"> The progress. </param>
    ///
    /// <returns>   True if it succeeds, false if it fails. </returns>

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
/// Game data which persists through levels and stores all relevant information.
/// </summary>
///
/// <remarks>    . </remarks>

public class GameData : MonoBehaviour {

    // Shared attributes between scenes
    /// <summary>   Number of lives lefts. </summary>
    private int _numberOfLivesLeft;

    /// <summary>   The total time. </summary>
    private float _totalTime;
    /// <summary>   The shared gold. </summary>
    private int _sharedGold;

    /// <summary>   The torch fuel. </summary>
    private int _torchFuel;
    /// <summary>   The torch fuel slider. </summary>
    private Slider _torchFuelSlider;

    /// <summary>   The times killed. </summary>
    private float _timesKilled;
    /// <summary>   The monsters killed. </summary>
    private float _monstersKilled;
    /// <summary>   The chests missed. </summary>
    private float _chestsMissed;

    /// <summary>   The player one accuracy. </summary>
    private float _playerOneAccuracy = 0f;
    /// <summary>   The player two accuracy. </summary>
    private float _playerTwoAccuracy = 0f;

    /// <summary>   The player one number hits missed. </summary>
    private float _playerOneNumHitsMissed = 0f;
    /// <summary>   The player one number hits achieved. </summary>
    private float _playerOneNumHitsAchieved = 0f;
    /// <summary>   The player two number hits missed. </summary>
    private float _playerTwoNumHitsMissed = 0f;
    /// <summary>   The player two number hits achieved. </summary>
    private float _playerTwoNumHitsAchieved = 0f;

    /// <summary>   Manager for in game user interface. </summary>
    private InGameUiManager _inGameUiManager;
    /// <summary>   True to first level. </summary>
    private bool _firstLevel = true;

    /// <summary>   Dictionary of player 1 item quantities. </summary>
    private Dictionary<string, int> _player1ItemQuantityDictionary = new Dictionary<string, int>();
    /// <summary>   Dictionary of player 2 item quantities. </summary>
    private Dictionary<string, int> _player2ItemQuantityDictionary = new Dictionary<string, int>();
    /// <summary>   The game achievements. </summary>
    private List<GameAchievement> GameAchievements = new List<GameAchievement>();

    //Achievements
    /// <summary>   The achievements. </summary>
    public Achievement[] Achievements;
    /// <summary>   The achievement popup. </summary>
    public Canvas AchievementPopup;
    /// <summary>   The achievement text. </summary>
    private Text achievementText;
    /// <summary>   The time to wait. </summary>
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 3.0f;

    /// <summary>   True if this object is main menu. </summary>
    public bool isMainMenu = false;

    /// <summary>   Adds a new achievement. </summary>
    ///
 
    ///
    /// <param name="ac">   The AC. </param>

    public void AddAchievment(GameAchievement ac)
    {
        GameAchievements.Add(ac);
    }

    /// <summary>   Returns achievements. </summary>
    ///
 
    ///
    /// <returns>   The game achievements. </returns>

    public List<GameAchievement> GetGameAchievements()
    {
        return GameAchievements;
    }

    /// <summary>   Called before any Start methods called and is used for initialisation. </summary>
    ///
 

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

    /// <summary>   Start method to initialise appropriate ui manager. </summary>
    ///
 

	void Start() {
        if (!isMainMenu)
        {
            // Getting the game data object which shows the total lives left
            GameObject go = GameObject.Find("InGameUiManager");
            _inGameUiManager = (InGameUiManager)go.GetComponent(typeof(InGameUiManager));
        }
	}

    /// <summary>   Called every frame to set the total time. </summary>
    ///
 

    void Update()
    {
		// Getting current time since users started playing 
		_totalTime += Time.deltaTime;
    }

    /// <summary>   Used to get the number of shared lives. </summary>
    ///
 
    ///
    /// <returns>   The number of lives. </returns>

    public int GetNumberOfLives()
    {
        return _numberOfLivesLeft;
    }

    /// <summary>   Used to set the number of shared lives. </summary>
    ///
 
    ///
    /// <param name="lives">    The lives. </param>

    public void SetNumberOfLives(int lives)
    {
        _numberOfLivesLeft = lives;
    }

    /// <summary>   Updates the number of lives. </summary>
    ///
 

	public void UpdateNumberOfLives() {
		// Cannot get more than 3 lives
		if (_numberOfLivesLeft != 3) {
			_numberOfLivesLeft++;
		}
	}

    /// <summary>   Used to get the amount of shared gold. </summary>
    ///
 
    ///
    /// <returns>   The amount of gold. </returns>

    public int GetAmountOfGold()
    {
        return _sharedGold;
    }

    /// <summary>   Used to get the total time. </summary>
    ///
 
    ///
    /// <returns>   The total time. </returns>

	public float GetTotalTime()
	{
		if (_firstLevel) {
			_totalTime = 0f;
			_firstLevel = false;
		}
		return _totalTime;
	}

    /// <summary>   Used to set the amount of shared gold. </summary>
    ///
 
    ///
    /// <param name="gold"> The gold. </param>

    public void SetAmountOfGold(int gold)
    {
        _sharedGold = gold;
    }

    /// <summary>   Updates the gold. </summary>
    ///
 
    ///
    /// <param name="amount">   The amount. </param>

    public void UpdateGold(int amount)
    {
        _sharedGold += amount;
		_inGameUiManager.UpdateGold (_sharedGold);
    }

     /// <summary>  Returns an achievement. </summary>
     ///
     /// <remarks>   . </remarks>
     ///
     /// <param name="achievementName"> Name of the achievement. </param>
     ///
     /// <returns>  The achievement by name. </returns>

     private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    /// <summary>   Used to play sounds when an achievement is earned. </summary>
    ///
 

    private void AchievementEarned()
    {
        //  UpdateRewardPointTotals();
        //AudioSource.PlayClipAtPoint(EarnedSound, Camera.main.transform.position);
    }

    /// <summary>   Closes the pop up for the achievement. </summary>
    ///
 

    void ClosePopUpTimer()
    {
        currentTime = Time.time;
        if (currentTime - executedTime > timeToWait)
        {
            executedTime = 0.0f;
            AchievementPopup.enabled = false;
        }
    }

    /// <summary>   Updating the achievement to check whether it has been met yet. </summary>
    ///
 
    ///
    /// <param name="achievementName">  Name of the achievement. </param>
    /// <param name="progressAmount">   The progress amount. </param>

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

    /// <summary>   Setting player 1 accuracy. </summary>
    ///
 
    ///
    /// <param name="acc">  The accumulate. </param>

	public void SetPlayer1Accuracy(float acc) {
		_playerOneAccuracy = acc;
	}

    /// <summary>   Getting player 1 accuracy. </summary>
    ///
 
    ///
    /// <returns>   The player 1 accuracy. </returns>

	public float GetPlayer1Accuracy() {
		float totalSwings = _playerOneNumHitsAchieved + _playerOneNumHitsMissed;
		if (totalSwings != 0) {
			_playerOneAccuracy = _playerOneNumHitsAchieved / totalSwings;
		}
		return _playerOneAccuracy;
	}

    /// <summary>   Setting player 2 accuracy. </summary>
    ///
 
    ///
    /// <param name="acc">  The accumulate. </param>

	public void SetPlayer2Accuracy(float acc) {
		_playerTwoAccuracy = acc;
	}

    /// <summary>   Getting player 2 accuracy. </summary>
    ///
 
    ///
    /// <returns>   The player 2 accuracy. </returns>

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
    ///
 

	public void UpdateChestsMissed() {
		_chestsMissed++;
	}

    /// <summary>   Getting the number of chests missed. </summary>
    ///
 
    ///
    /// <returns>   The chests missed. </returns>

	public float GetChestsMissed() {
		return _chestsMissed;
	}

    /// <summary>   Updating the number of times the players have been killed. </summary>
    ///
 

	public void UpdateTimesKilled() {
		_timesKilled++;
	}

    /// <summary>   Getting the number of times the players have been killed. </summary>
    ///
 
    ///
    /// <returns>   The times killed. </returns>

	public float GetTimesKilled() {
		return _timesKilled;
	}

    /// <summary>   Updating the monsters killed. </summary>
    ///
 

	public void UpdateMonstersKilled() {
		_monstersKilled++;
	}

    /// <summary>   Getting the number of monsters killed. </summary>
    ///
 
    ///
    /// <returns>   The monsters killed. </returns>

	public float GetMonstersKilled() {
		return _monstersKilled;
	}

    /// <summary>   Getting the dictionary of items. </summary>
    ///
 
    ///
    /// <returns>   The player 1 item quantity dictionary. </returns>

	public Dictionary<string,int> GetPlayer1ItemQuantityDictionary() {
		return _player1ItemQuantityDictionary;
	}

    /// <summary>   Getting the dictionary of items. </summary>
    ///
 
    ///
    /// <returns>   The player 2 item quantity dictionary. </returns>

	public Dictionary<string,int> GetPlayer2ItemQuantityDictionary() {
		return _player2ItemQuantityDictionary;
	}

    /// <summary>   Setting the dictionary of items. </summary>
    ///
 
    ///
    /// <param name="item"> The item. </param>
    /// <param name="num">  Number of. </param>

	public void SetPlayer1ItemQuantityDictionary(string item, int num) {
		_player1ItemQuantityDictionary [item] = num;
	}

    /// <summary>   Setting the dictionary of items. </summary>
    ///
 
    ///
    /// <param name="item"> The item. </param>
    /// <param name="num">  Number of. </param>

	public void SetPlayer2ItemQuantityDictionary(string item, int num) {
		_player2ItemQuantityDictionary [item] = num;
	}

    /// <summary>   Updating the number of times the player has not hit an enemy. </summary>
    ///
 
    ///
    /// <param name="isPlayer1">    True if this object is player 1. </param>

	public void UpdatePlayerNumHitsMissed (bool isPlayer1) {
		if (isPlayer1) {
			_playerOneNumHitsMissed++;
		} else {
			_playerTwoNumHitsMissed++;
		}
	}

    /// <summary>   Updating the number of times the player has hit an enemy. </summary>
    ///
 
    ///
    /// <param name="isPlayer1">    True if this object is player 1. </param>

	public void UpdatePlayerNumHitsAchieved (bool isPlayer1) {
		if (isPlayer1) {
			_playerOneNumHitsAchieved++;
		} else {
			_playerTwoNumHitsAchieved++;
		}
	}
}
