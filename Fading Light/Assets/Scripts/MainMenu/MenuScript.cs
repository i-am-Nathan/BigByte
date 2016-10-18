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

    public AudioSource buttonClickSound;


	// Use this for initialization
	void Start () {
        levelSelectText = levelSelectText.GetComponent<Button>();
        startText = startText.GetComponent<Button>();
        highScoreText = highScoreText.GetComponent<Button>();
        achievementText = achievementText.GetComponent<Button>();
    
		highscoreMenu.enabled = false;
		achievementMenu.enabled = false;
        levelSelectMenu.enabled = false;
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
        buttonClickSound.Play();
        highscoreMenu.enabled = true;
        startText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
        levelSelectText.enabled = false;
    }

	//When the achievements are pressed a pop up of achievements should pop up.
    public void achievementPress()
    {
        buttonClickSound.Play();
        achievementMenu.enabled = true;
        startText.enabled = false;
		highScoreText.enabled = false;
		achievementText.enabled = false;
        levelSelectText.enabled = false;
    }

	//Go back to the main menu when the back button is pressed on the achievement menu.
	public void backPress(){

        buttonClickSound.Play();
   
        levelSelectMenu.enabled = false;
        achievementMenu.enabled = false;
        highscoreMenu.enabled = false;
        startText.enabled = true;
		highScoreText.enabled = true;
		achievementText.enabled = true;
        levelSelectText.enabled = true;
	}

    public void levelPress()
    {
        buttonClickSound.Play();
        levelSelectMenu.enabled = true;
        startText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
        levelSelectText.enabled = false;
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
