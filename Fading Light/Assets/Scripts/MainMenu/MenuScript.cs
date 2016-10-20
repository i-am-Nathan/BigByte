// file:	Assets\Scripts\MainMenu\MenuScript.cs
//
// summary:	Implements the menu script class

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>   Logic for the whole Main menu scene. </summary>
///
/// <remarks>    . </remarks>

public class MenuScript : MonoBehaviour {

	//Canvas is used to pop up when the specified buttons are pressed
    /// <summary>   The achievement menu. </summary>
	public GameObject AchievementMenu;
    /// <summary>   The highscore menu. </summary>
	public GameObject HighscoreMenu;
    /// <summary>   The level select menu. </summary>
    public GameObject LevelSelectMenu;
    /// <summary>   Information describing the game. </summary>
    public GameData GameData;


    /// <summary>   The button click sound. </summary>
    public AudioSource ButtonClickSound;

    /// <summary>   List of names of the levels. </summary>
    public static string[] LevelNames = { "Level1", "Level2", "Level3", "Level4" };

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {

        HideAllMenus();
    }

    /// <summary>   Play button sound. </summary>
    ///
 

    private void PlayButtonSound()
    {
        ButtonClickSound.Play();
    }

    /// <summary>   Hides all menus. </summary>
    ///
 

    private void HideAllMenus()
    {
        HighscoreMenu.SetActive(false);
        AchievementMenu.SetActive(false);
        LevelSelectMenu.SetActive(false);
    }

	//Start game when the start text is pressed

    /// <summary>   Starts a level. </summary>
    ///
 

    public void StartLevel()
    {
        //Loading level
        GameData.isMainMenu = false;
		SceneManager.LoadScene ("Level1");
        //StartLevel(0);
    }

	//Highscore should pop up when it is pressed, to be implemented.

    /// <summary>   High score press. </summary>
    ///
 

    public void highScorePress()
    {
		PlayButtonSound();
        HideAllMenus();
        HighscoreMenu.SetActive(true);
        HighscoreMenu.transform.Find("Scrollbar").GetComponent<Scrollbar>().value = 1;
    }

	//When the achievements are pressed a pop up of achievements should pop up.

    /// <summary>   Achievement press. </summary>
    ///
 

    public void achievementPress()
    {
        PlayButtonSound();
        HideAllMenus();
        AchievementMenu.SetActive(true);
    }

	//Go back to the main menu when the back button is pressed on the achievement menu.

    /// <summary>   Back press. </summary>
    ///
 

	public void backPress(){
        PlayButtonSound();
        HideAllMenus();
    }

    /// <summary>   Level press. </summary>
    ///
 

    public void levelPress()
    {
        PlayButtonSound();
        HideAllMenus();
        LevelSelectMenu.SetActive(true);

    }

    /// <summary>   Starts a level. </summary>
    ///
 
    ///
    /// <param name="levelIndex">   Zero-based index of the level. </param>

    public void StartLevel(int levelIndex)
    {
        Application.LoadLevel(LevelNames[levelIndex]);
    }


   
}
