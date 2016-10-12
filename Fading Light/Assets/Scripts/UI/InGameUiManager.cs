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
    public Button RestartLevelButton;
    public Button QuitToMenuButton;
    public Button ContinueButton;

	/// <summary>
	/// Used for initialisation 
	/// </summary>
    void Start () {
		// Hiding menus which are not meant to be shown yet
        LevelFinishedMenu.enabled = false;
        ExitMenu.enabled = false;

        RestartLevelButton = RestartLevelButton.GetComponent<Button>();
        QuitToMenuButton = QuitToMenuButton.GetComponent<Button>();
        ContinueButton = ContinueButton.GetComponent<Button>();
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
	/// Called when quit to main menu button is clicked
	/// Loads the main menu screen
	/// </summary>
    public void QuitToMenu ()
    {
		// Unpausing the game and loading the main menu scene
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

	/// <summary>
	/// Called when the restart button is clicked
	/// </summary>
    public void RestartLevel ()
    {
		// Unpaused the game and gets the active scene and reloads it
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
