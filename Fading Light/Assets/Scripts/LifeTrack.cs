using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Used to enable persistent data throughout scenes
/// </summary>
public class LifeTrack : MonoBehaviour
{
	// Shared attributes between scenes
    private int _numberOfLivesLeft;
    private float _totalTime;
    private Text _totalTimeText;

	/// <summary>
	/// Called before any Start methods called and is used for initialisation
	/// </summary>
    void Awake()
    {
		// Finding existing instances of this game object
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Life Track");

		// Finding the total time
        _totalTimeText = GameObject.FindWithTag("Total Time").GetComponent<Text>();

        // Checking if this life track objects already exists
        if (!(objects.Length > 0))
        {
			// Used to initialise this object with 3 lives and a time of 0
            // Assigning a tag and instantiating number of lives
            _numberOfLivesLeft = 3;
            this.gameObject.tag = "Life Track";
            _totalTime = 0f;
            DontDestroyOnLoad(GameObject.FindWithTag("Life Track").gameObject);
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
}

