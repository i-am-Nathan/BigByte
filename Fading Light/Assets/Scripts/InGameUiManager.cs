using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// This class is used to handle button and canvas behaviour of the in game UI
public class InGameUiManager : MonoBehaviour {

    public Canvas ExitMenu;
    public Button RestartLevelButton;
    public Button QuitToMenuButton;
    public Button ContinueButton;

    void Start () {
        ExitMenu.enabled = false;
        RestartLevelButton = RestartLevelButton.GetComponent<Button>();
        QuitToMenuButton = QuitToMenuButton.GetComponent<Button>();
        ContinueButton = ContinueButton.GetComponent<Button>();
    }
	
	void Update () {
        // Used to show and hide the exit menu allowing users to exit/quit/continue
        // This will not pause the game as high score is based on time, and users may take advantage of the pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu.enabled = !ExitMenu.enabled;
        }
    }

    // Called when Continue button is called
    public void Continue ()
    {
        ExitMenu.enabled = false;
    }

    // Called when quit to main menu button is clicked
    // Loads the main menu screen
    public void QuitToMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Called when restart level button is clicked
    public void RestartLevel ()
    {
        // Gets the active scene and reloads it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
