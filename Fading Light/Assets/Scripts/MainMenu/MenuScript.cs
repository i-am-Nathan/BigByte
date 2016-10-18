using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Logic for the whole Main menu scene
/// </summary>
public class MenuScript : MonoBehaviour {

	//Canvas is used to pop up when the specified buttons are pressed
	public Canvas achievementMenu;
	public Canvas highscoreMenu;
    public Canvas levelSelectMenu;

    public Button startText;
    public Button highScoreText;
    public Button achievementText;
    public Button levelSelectText;


	// Use this for initialization
	void Start () {
        levelSelectMenu = levelSelectMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        highScoreText = highScoreText.GetComponent<Button>();
        achievementText = achievementText.GetComponent<Button>();
        
		highscoreMenu.enabled = false;
		achievementMenu.enabled = false;
        levelSelectMenu.enabled = false;
    }

	//When the exit button is pressed the quit menu should pop up
    public void ExitPress()
    {
        startText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
    }
	//When no is pressed on the quit menu close the pop up and enable all the buttons again so that it can be pressed.
    public void NoPress()
    {
        startText.enabled = true;
        highScoreText.enabled = true;
       	achievementText.enabled = true;
    }


	//Start game when the start text is pressed
    public void StartLevel()
    {
        //Loading level
        Application.LoadLevel("ammar_ui_v2");
    }

	//Highscore should pop up when it is pressed, to be implemented.
    public void highScorePress()
    {
        highscoreMenu.enabled = true;
        startText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
    }

	//When the achievements are pressed a pop up of achievements should pop up.
    public void achievementPress()
    {
        highscoreMenu.enabled = false;
        startText.enabled = false;
		highScoreText.enabled = false;
		achievementText.enabled = false;
    }

	//Go back to the main menu when the back button is pressed on the achievement menu.
	public void backPress(){
		achievementMenu.enabled = false;
        highscoreMenu.enabled = false;
        startText.enabled = true;
		highScoreText.enabled = true;
		achievementText.enabled = true;
	}

    public void levelPress()
    {
        levelSelectMenu.enabled = true;
        startText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
    }
    public void levelbackPress()
    {
        levelSelectMenu.enabled = false;
        startText.enabled = true;
        highScoreText.enabled = true;
        achievementText.enabled = true;
    }

    public void tutLevelPress()
    {

    }

    public void levelOnePress()
    {

    }

    public void finalLevelPress()
    {

    }
}
