using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Logic for the whole Main menu scene
/// </summary>
public class MenuScript : MonoBehaviour {

	//Canvas is used to pop up when the specified buttons are pressed
    public Canvas quitMenu;
	public Canvas achievementMenu;
	public Canvas highscoreMenu;

    public Button startText;
    public Button exitText;
    public Button highScoreText;
    public Button achievementText;


	// Use this for initialization
	void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        highScoreText = highScoreText.GetComponent<Button>();
        achievementText = achievementText.GetComponent<Button>();
        quitMenu.enabled = false;
		highscoreMenu.enabled = false;
		achievementMenu.enabled = false;
    }

	//When the exit button is pressed the quit menu should pop up
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
    }
	//When no is pressed on the quit menu close the pop up and enable all the buttons again so that it can be pressed.
    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        highScoreText.enabled = true;
       	achievementText.enabled = true;
    }

	//When exit game is pressed on the quit menu quit game
    public void ExitGame()
    {
        Application.Quit();
    }

	//Start game when the start text is pressed
    public void StartLevel()
    {
        //Loading level
        Application.LoadLevel("TutorialLevel");
    }

	//Highscore should pop up when it is pressed, to be implemented.
    public void highScorePress()
    {

    }

	//When the achievements are pressed a pop up of achievements should pop up.
    public void achievementPress()
    {
		achievementMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
		highScoreText.enabled = false;
		achievementText.enabled = false;
    }

	//Go back to the main menu when the back button is pressed on the achievement menu.
	public void backPress(){
		achievementMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
		highScoreText.enabled = true;
		achievementText.enabled = true;
	}


}
