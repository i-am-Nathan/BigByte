using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// This class is used to handle button and canvas behaviour of the in game UI
public class InGameUiManager : MonoBehaviour {

    public Canvas LevelFinishedMenu;
    public Canvas ExitMenu;
    public Button RestartLevelButton;
    public Button QuitToMenuButton;
    public Button ContinueButton;

    private BoxCollider _levelFinishedTrigger;

    void Start () {
        LevelFinishedMenu.enabled = false;
        ExitMenu.enabled = false;
        RestartLevelButton = RestartLevelButton.GetComponent<Button>();
        QuitToMenuButton = QuitToMenuButton.GetComponent<Button>();
        ContinueButton = ContinueButton.GetComponent<Button>();
        _levelFinishedTrigger = GameObject.FindWithTag("EndOfLevelTrigger").GetComponent<BoxCollider>();
    }
	
	void Update () {
        // Used to show and hide the exit menu allowing users to exit/quit/continue
        // This will not pause the game as high score is based on time, and users may take advantage of the pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu.enabled = !ExitMenu.enabled;
        }
    }

    void OnTriggerEnter ()
    {
        // Pauses the game and shows the menu indicating that players have completed the level
        Time.timeScale = 0;
        LevelFinishedMenu.enabled = true;
    }

    // Called when Continue button is called
    public void Continue ()
    {
        Time.timeScale = 1;
        ExitMenu.enabled = false;
    }

    // Called when quit to main menu button is clicked
    // Loads the main menu screen
    public void QuitToMenu ()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // Called when restart level button is clicked
    public void RestartLevel ()
    {
        Time.timeScale = 1;
        // Gets the active scene and reloads it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
