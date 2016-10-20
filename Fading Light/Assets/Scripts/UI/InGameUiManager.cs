using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// This class is used to handle button and canvas behaviour of the in game UI
/// </summary>
public class InGameUiManager : MonoBehaviour {

	// UI items used to present information to the players
    public Canvas LevelFinishedMenu;
    public Canvas ExitMenu;
	public Canvas ControlsMenu;
    public Button RestartLevelButton;
    public Button QuitToMenuButton;
    public Button ContinueButton;

	private float _totalTime;
	private Text _totalTimeText;
	private int _sharedGold;
	private Text _sharedGoldText;

	private GameData _gameDataScript;

	/// <summary>
	/// Used for initialisation 
	/// </summary>
    void Start () {
		// Hiding menus which are not meant to be shown yet
        LevelFinishedMenu.enabled = false;
        ExitMenu.enabled = false;
        if(ControlsMenu != null)
        {
            ControlsMenu.enabled = false;
        }
	

        RestartLevelButton = RestartLevelButton.GetComponent<Button>();
        QuitToMenuButton = QuitToMenuButton.GetComponent<Button>();
        ContinueButton = ContinueButton.GetComponent<Button>();

		// Getting the game data object which shows the total lives left
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));

		_totalTime = _gameDataScript.GetTotalTime ();
		_sharedGold = _gameDataScript.GetAmountOfGold ();

		// Finding the total time
		_totalTimeText = GameObject.FindWithTag("Total Time").GetComponent<Text>();
		_sharedGoldText = GameObject.FindWithTag("Shared Gold").GetComponent<Text>();
		_sharedGoldText.text = "" + _sharedGold;
		SetTime ();
    }

	/// <summary>
	/// Used every frame
	/// </summary>
	void Update () {
        // Used to show and hide the exit menu allowing users to exit/quit/continue
        // This will not pause the game as high score is based on time, and users may take advantage of the pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu.enabled = !ExitMenu.enabled;
        }

		if (Input.GetKeyDown (KeyCode.Tab)) {
			ControlsMenu.enabled = !ControlsMenu.enabled;
		}
		SetTime();
    }

	/// <summary>
	/// Used to continue the game (called by Continue button)
	/// </summary>
    public void Continue ()
    {
		// Unpausing the game and hiding the exit menu which is being shown
        Time.timeScale = 1;
        ExitMenu.enabled = false;
    }

	/// <summary>
	/// Used to continue the game (called by Continue button)
	/// </summary>
	public void NextLevel ()
	{
		// Unpausing the game and hiding the exit menu which is being shown
		Time.timeScale = 1;
		ExitMenu.enabled = false;
		Scene scene = SceneManager.GetActiveScene();

		if (scene.name == "Level1") {
			SceneManager.LoadScene("Level2");
		} else if (scene.name == "Level2") {
			SceneManager.LoadScene("Level3");
		} else if (scene.name == "Level3") {
			SceneManager.LoadScene("Level4");
		}
	}
		
	/// <summary>
	/// Called when quit to main menu button is clicked
	/// Loads the main menu screen
	/// </summary>
    public void QuitToMenu ()
    {
		// Unpausing the game and loading the main menu scene
        Time.timeScale = 1;

		// Destroying game data to reset all data
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		Destroy (go);
        SceneManager.LoadScene("MainMenu");
    }

	/// <summary>
	/// Called when the restart button is clicked
	/// </summary>
    public void RestartLevel ()
    {
		// Unpaused the game and gets the active scene and reloads it
        Time.timeScale = 1;

		GameObject go = GameObject.FindGameObjectWithTag("Life Manager");
		LifeManager lifeManagerScript = (LifeManager)go.GetComponent(typeof(LifeManager));
		lifeManagerScript.LoseLife ();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	/// <summary>
	/// Used to set the total time taken for the players
	/// </summary>
	private void SetTime()
	{
        //return;
		_totalTime = _gameDataScript.GetTotalTime ();
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

	public void UpdateGold(int gold) {
		_sharedGold = gold;
		_sharedGoldText = GameObject.FindWithTag("Shared Gold").GetComponent<Text>();
		_sharedGoldText.text = "" + _sharedGold;
	}
}
