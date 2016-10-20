using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Used to register when players have reached the end of the level
/// </summary>
public class EndOfLevelTrigger : MonoBehaviour {

	private bool _player1Entered = false;
	private bool _player2Entered = false;
	private bool _endOfGame = false;
	private int _numTotalChests = 10;
	private DatabaseScores _dbScoresScript;
	private GameData _gameDataScript;

	// Canvas indicating to players they have finished the level
	public Canvas LevelFinishedMenu;
	public Canvas SubmitHighScoreMenu;
	public Button SubmitHighScoreButton;
	public Text Player1Name;
	public Text Player2Name;

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

	/// <summary>
	/// Obtaining the correct information and sending it to the database
	/// </summary>
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

	/// <summary>
	/// Used for initialisation
	/// </summary>
    void Start()
    {
		// Hiding the menu
        LevelFinishedMenu.enabled = false;
		SubmitHighScoreMenu.enabled = false;
		_dbScoresScript = GameObject.Find("DatabaseScores").GetComponent<DatabaseScores>();
		_gameDataScript = GameObject.Find("GameData").GetComponent<GameData>();
    }
		
	/// <summary>
	/// Submits the highscore to the online database
	/// </summary>
	public void SubmitHighScore () {
		SendToDatabase ();
		SubmitHighScoreMenu.enabled = false;
		SceneManager.LoadScene("MainMenu");
	}
}
