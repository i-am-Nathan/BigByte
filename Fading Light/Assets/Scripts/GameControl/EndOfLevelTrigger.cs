using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/// <summary>
/// Used to register when players have reached the end of the level
/// </summary>
public class EndOfLevelTrigger : MonoBehaviour {

	// Canvas indicating to players they have finished the level
    public Canvas LevelFinishedMenu;
	private bool _player1Entered = false;
	private bool _player2Entered = false;
	private bool _endOfGame = false;

	void Update () {
		if (_player1Entered && _player2Entered && !_endOfGame) {
			// Pauses the game and shows the menu indicating that players have completed the level
			Time.timeScale = 0;
			LevelFinishedMenu.enabled = true;
			Scene scene = SceneManager.GetActiveScene();
			if (scene.name == "ammar_ui_v2") {
				Debug.Log ("End of game");
				_endOfGame = true;
				// Get users names
				// Post scores to database
				DatabaseScores _dbScoresScript = GameObject.Find("DatabaseScores").GetComponent<DatabaseScores>();
				GameData _gameDataScript = GameObject.Find("GameData").GetComponent<GameData>();

				string playersNames = "ayyyyyyyyyyy";
				int gold = _gameDataScript.GetAmountOfGold ();
				float totalTime = _gameDataScript.GetTotalTime ();
				float player1Accuracy = _gameDataScript.GetPlayer1Accuracy ();
				float player2Accuracy = _gameDataScript.GetPlayer2Accuracy ();
				float monstersKilled = _gameDataScript.GetMonstersKilled ();
				float timesKilled = _gameDataScript.GetTimesKilled ();
				float chestsMissed = _gameDataScript.GetChestsMissed ();

				float mins = Mathf.Floor(totalTime / 60);
				float secs = Mathf.RoundToInt(totalTime % 60);
				StartCoroutine(_dbScoresScript.PostScores (playersNames, gold, player1Accuracy, player2Accuracy, mins, secs, monstersKilled, timesKilled, chestsMissed));
			}
		}
	}

	/// <summary>
	/// Used for initialisation
	/// </summary>
    void Start()
    {
		// Hiding the menu
        LevelFinishedMenu.enabled = false;
    }

	/// <summary>
	/// Called when a player enters the box collider placed at the end of the level
	/// </summary>
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
			_player1Entered = true;
        }

		if (c.gameObject.tag == "Player2") {
			_player2Entered = true;
		}
    }

	void OnTriggerExit(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			_player1Entered = false;
		}

		if (c.gameObject.tag == "Player2") {
			_player2Entered = false;
		}
	}
}
