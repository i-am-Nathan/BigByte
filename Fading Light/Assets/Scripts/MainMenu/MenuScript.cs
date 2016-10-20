using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Logic for the whole Main menu scene
/// </summary>
public class MenuScript : MonoBehaviour {

	//Canvas is used to pop up when the specified buttons are pressed
	public GameObject AchievementMenu;
	public GameObject HighscoreMenu;
    public GameObject LevelSelectMenu;
    public GameData GameData;


    public AudioSource ButtonClickSound;

    public static string[] LevelNames = { "Level1", "Level2", "Level3", "Level4" };

	// Use this for initialization
	void Start () {

        HideAllMenus();
    }

    private void PlayButtonSound()
    {
        ButtonClickSound.Play();
    }

    private void HideAllMenus()
    {
        HighscoreMenu.SetActive(false);
        AchievementMenu.SetActive(false);
        LevelSelectMenu.SetActive(false);
    }

	//Start game when the start text is pressed
    public void StartLevel()
    {
        //Loading level
        GameData.isMainMenu = false;
		SceneManager.LoadScene ("Level1");
        //StartLevel(0);
    }

	//Highscore should pop up when it is pressed, to be implemented.
    public void highScorePress()
    {
		PlayButtonSound();
        HideAllMenus();
        HighscoreMenu.SetActive(true);
        HighscoreMenu.transform.Find("Scrollbar").GetComponent<Scrollbar>().value = 1;
    }

	//When the achievements are pressed a pop up of achievements should pop up.
    public void achievementPress()
    {
        PlayButtonSound();
        HideAllMenus();
        AchievementMenu.SetActive(true);
    }

	//Go back to the main menu when the back button is pressed on the achievement menu.
	public void backPress(){
        PlayButtonSound();
        HideAllMenus();
    }

    public void levelPress()
    {
        PlayButtonSound();
        HideAllMenus();
        LevelSelectMenu.SetActive(true);

    }

    public void StartLevel(int levelIndex)
    {
        Application.LoadLevel(LevelNames[levelIndex]);
    }


   
}
