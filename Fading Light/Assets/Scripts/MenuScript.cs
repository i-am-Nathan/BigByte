using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button highScoreText;
    public Button optionsText;


	// Use this for initialization
	void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        highScoreText = highScoreText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        quitMenu.enabled = false;
    }
	
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        highScoreText.enabled = false;
        optionsText.enabled = false;
    }
    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        highScoreText.enabled = true;
        optionsText.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartLevel()
    {
        //Loading level
        //Application.LoadLevel(1);
    }
    public void highScorePress()
    {

    }
    public void optionPress()
    {

    }
}
