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
	//private DatabaseScores _dbScoresScript;
	private GameData _gameDataScript;

	// Canvas indicating to players they have finished the level
	public Canvas LevelFinishedMenu;
	public Canvas SubmitHighScoreMenu;
	public Button SubmitHighScoreButton;
	public Text Player1Name;
	public Text Player2Name;

	void Update () {
		if (_player1Entered && _player2Entered && !_endOfGame) {
			// Pauses the game and shows the menu indicating that players have completed the level
			Time.timeScale = 0;
			LevelFinishedMenu.enabled = true;
			Scene scene = SceneManager.GetActiveScene();

			if (scene.name == "ammar_ui_v2") {
				_endOfGame = true;
				SubmitHighScoreMenu.enabled = true;
			}
		}
	}

	public void SendToDatabase () {
		Player1Name = Player1Name.GetComponent<Text>();
		Player2Name = Player2Name.GetComponent<Text>();

		string playersNames = Player1Name.text + "-" + Player2Name.text;
		int gold = _gameDataScript.GetAmountOfGold ();
		float totalTime = _gameDataScript.GetTotalTime ();
		float player1Accuracy = _gameDataScript.GetPlayer1Accuracy ();
		float player2Accuracy = _gameDataScript.GetPlayer2Accuracy ();
		float monstersKilled = _gameDataScript.GetMonstersKilled ();
		float timesKilled = _gameDataScript.GetTimesKilled ();
		float chestsMissed = _gameDataScript.GetChestsMissed ();
		float mins = Mathf.Floor(totalTime / 60);
		float secs = Mathf.RoundToInt(totalTime % 60);

		//StartCoroutine(_dbScoresScript.PostScores (playersNames, gold, player1Accuracy, player2Accuracy, mins, secs, monstersKilled, timesKilled, chestsMissed));
	}

	/// <summary>
	/// Used for initialisation
	/// </summary>
    void Start()
    {
		// Hiding the menu
        LevelFinishedMenu.enabled = false;
		SubmitHighScoreMenu.enabled = false;
		//_dbScoresScript = GameObject.Find("DatabaseScores").GetComponent<DatabaseScores>();
		_gameDataScript = GameObject.Find("GameData").GetComponent<GameData>();
    }
		
	public void SubmitHighScore () {
		SendToDatabase ();
		SubmitHighScoreMenu.enabled = false;
		//SceneManager.LoadScene("MainMenu");
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
