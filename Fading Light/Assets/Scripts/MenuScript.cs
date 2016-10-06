using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

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
	
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        highScoreText.enabled = false;
        achievementText.enabled = false;
    }
    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        highScoreText.enabled = true;
       	achievementText.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartLevel()
    {
        //Loading level
        Application.LoadLevel("juno_tutlevel1");
    }
    public void highScorePress()
    {

    }
    public void achievementPress()
    {
		achievementMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
		highScoreText.enabled = false;
		achievementText.enabled = false;
    }

	public void backPress(){
		achievementMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
		highScoreText.enabled = true;
		achievementText.enabled = true;
	}


}
