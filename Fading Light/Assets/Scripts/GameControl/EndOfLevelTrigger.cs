// file:	assets\scripts\gamecontrol\endofleveltrigger.cs
//
// summary:	Implements the endofleveltrigger class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>   Used to register when players have reached the end of the level. </summary>
///
/// <remarks>    . </remarks>

public class EndOfLevelTrigger : MonoBehaviour {

    /// <summary>   True if player 1 entered. </summary>
	private bool _player1Entered = false;
    /// <summary>   True if player 2 entered. </summary>
	private bool _player2Entered = false;
    /// <summary>   True to end of game. </summary>
	private bool _endOfGame = false;
    /// <summary>   Number of total chests. </summary>
	private int _numTotalChests = 10;
    /// <summary>   The database scores script. </summary>
	private DatabaseScores _dbScoresScript;
    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;

	// Canvas indicating to players they have finished the level
    /// <summary>   The level finished menu. </summary>
	public Canvas LevelFinishedMenu;
    /// <summary>   The submit high score menu. </summary>
	public Canvas SubmitHighScoreMenu;
    /// <summary>   The submit high score control. </summary>
	public Button SubmitHighScoreButton;
    /// <summary>   Name of the player 1. </summary>
	public Text Player1Name;
    /// <summary>   Name of the player 2. </summary>
	public Text Player2Name;

    /// <summary>   Trigger end of level. </summary>
    ///
 

	public void TriggerEndOfLevel() {
		// Pauses the game and shows the menu indicating that players have completed the level
		Time.timeScale = 0;
		LevelFinishedMenu.enabled = true;
		Scene scene = SceneManager.GetActiveScene();

		if (scene.name == "Level4") {
			LevelFinishedMenu.enabled = false;
			_endOfGame = true;
			SubmitHighScoreMenu.enabled = true;
		}
	}

    /// <summary>   Obtaining the correct information and sending it to the database. </summary>
    ///
 

	public void SendToDatabase () {
		Player1Name = Player1Name.GetComponent<Text>();
		Player2Name = Player2Name.GetComponent<Text>();

		// Obtaining data from the UI for names and the game data script
		string playersNames = Player1Name.text + "-" + Player2Name.text;
		int gold = _gameDataScript.GetAmountOfGold ();
		float totalTime = _gameDataScript.GetTotalTime ();
		float player1Accuracy = _gameDataScript.GetPlayer1Accuracy ();
		float player2Accuracy = _gameDataScript.GetPlayer2Accuracy ();
		float monstersKilled = _gameDataScript.GetMonstersKilled ();
		float timesKilled = _gameDataScript.GetTimesKilled ();
		float chestsMissed = _numTotalChests - _gameDataScript.GetChestsMissed ();
		float mins = Mathf.Floor(totalTime / 60);
		float secs = Mathf.RoundToInt(totalTime % 60);

		// Sending to database
		StartCoroutine(_dbScoresScript.PostScores (playersNames, gold, player1Accuracy, player2Accuracy, mins, secs, monstersKilled, timesKilled, chestsMissed));
	}

    /// <summary>   Used for initialisation. </summary>
    ///
 

    void Start()
    {
		// Hiding the menu
        LevelFinishedMenu.enabled = false;
		SubmitHighScoreMenu.enabled = false;
		_dbScoresScript = GameObject.Find("DatabaseScores").GetComponent<DatabaseScores>();
		_gameDataScript = GameObject.Find("GameData").GetComponent<GameData>();
    }

    /// <summary>   Submits the highscore to the online database. </summary>
    ///
 

	public void SubmitHighScore () {
		SendToDatabase ();
		SubmitHighScoreMenu.enabled = false;
		SceneManager.LoadScene("MainMenu");
	}
}
