using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameData : MonoBehaviour {

    // Shared attributes between scenes
    private int _numberOfLivesLeft;

    private float _totalTime;
    private Text _totalTimeText;

    private int _sharedGold;
    private Text _sharedGoldText;

    private int _torchFuel;
    private Slider _torchFuelSlider;

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

        // Finding the total time
        _totalTimeText = GameObject.FindWithTag("Total Time").GetComponent<Text>();
        _sharedGoldText = GameObject.FindWithTag("Shared Gold").GetComponent<Text>();

        // Checking if this life track objects already exists
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

    /// <summary>
    /// Called every frame to set the total time
    /// </summary>
    void Update()
    {
        SetTime();
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
    /// Used to set the amount of shared gold
    /// </summary>
    public void SetAmountOfGold(int gold)
    {
        _sharedGold = gold;
    }


    /// <summary>
    /// Used to set the total time taken for the players
    /// </summary>
    private void SetTime()
    {
        // Getting current time since users started playing 
        _totalTime += Time.deltaTime;

        float minutes = Mathf.Floor(_totalTime / 60);
        float seconds = Mathf.RoundToInt(_totalTime % 60);
        string min = "";
        string sec = "";

        // Formatting the time
        if (minutes < 10)
        {
            min = "0" + minutes;
        }
        else
        {
            min = "" + minutes;
        }

        if (seconds < 10)
        {
            sec = "0" + seconds;
        }
        else
        {
            sec = "" + seconds;
        }

        // Setting the UI component
        _totalTimeText.text = "Time:  " + min + ":" + sec;

    }

    /// <summary>
    /// Updates the gold.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateGold(int amount)
    {
        _sharedGold += amount;
        try
        {
            _sharedGoldText.text = "" + _sharedGold;
        }
        catch { }

    }

}
