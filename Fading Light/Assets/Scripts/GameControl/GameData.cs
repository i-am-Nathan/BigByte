using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameData : MonoBehaviour {

    // Shared attributes between scenes
    private int _numberOfLivesLeft;

    private float _totalTime;
    private int _sharedGold;

    private int _torchFuel;
    private Slider _torchFuelSlider;

    private float _playerOneTotalDamageGiven;
    private float _playerTwoTotalDamageGiven;

    private float _playerOneTotalDamageTaken;
    private float _playerTwoTotalDamageTaken;

    private float _playerOneAccuracy;
    private float _playerTwoAccuracy;

	private InGameUiManager _inGameUiManager;

    // Inventory
    //private Item[] _p1Items;
    //private Item[] _p2Items;

    //private Achievement[] _achievements;

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
            // Used to initialise this object with 3 lives and a time of 0
            // Assigning a tag and instantiating number of lives
            _numberOfLivesLeft = 3;
            this.gameObject.tag = "Game Data";
            _totalTime = 0f;
            _sharedGold = 0;
            DontDestroyOnLoad(GameObject.FindWithTag("Game Data").gameObject);
        }
    }
		
	void Start() {
		// Getting the game data object which shows the total lives left
		GameObject go = GameObject.Find("InGameUiManager");
		_inGameUiManager = (InGameUiManager)go.GetComponent(typeof(InGameUiManager));	
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

}
