using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LifeManager : MonoBehaviour
{

    private GameObject[] _lives;
    private int _numberOfLivesLeft;
    private LifeTrack _lifeTrackScript;
    public Canvas DeathScreen;

    void Start()
    {
        // Getting the life track object which shows the total lives left
        GameObject go = GameObject.FindGameObjectWithTag("Life Track");
        _lifeTrackScript = (LifeTrack)go.GetComponent(typeof(LifeTrack));

        // Getting the hearts on the UI
        _lives = GameObject.FindGameObjectsWithTag("Lives");
        _numberOfLivesLeft = _lifeTrackScript.GetNumberOfLives();

        // Setting the appropriate hearts to show
        for (int i = 0; i < _numberOfLivesLeft; i++)
        {
            _lives[i].SetActive(true);
        }

        // Setting the appropriate hearts to hide
        if (_numberOfLivesLeft != 3)
        {
            for (int i = _numberOfLivesLeft; i < 3; i++)
            {
                _lives[i].SetActive(false);
            }
        }


        DeathScreen.enabled = false;
    }

    public void LoseLife()
    {
        _numberOfLivesLeft = _numberOfLivesLeft - 1;
        _lifeTrackScript.SetNumberOfLives(_numberOfLivesLeft);

        if (_numberOfLivesLeft <= 0)
        {
            // Game over
            Time.timeScale = 0;
            DeathScreen.enabled = true;
        }
        else
        {
            // Removing a heart from the UI
            _lives[_numberOfLivesLeft - 1].SetActive(false);
            StartCoroutine("Wait");
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        // Restart level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Called when quit to main menu button is clicked
    // Loads the main menu screen
    public void QuitToMenu()
    {
        Time.timeScale = 1;
        _lifeTrackScript.SetNumberOfLives(3);
        SceneManager.LoadScene("MainMenu");
    }

    // Called when restart level button is clicked
    public void RestartLevel()
    {
        Time.timeScale = 1;
        // Gets the active scene and reloads it
        Scene scene = SceneManager.GetActiveScene();
        _lifeTrackScript.SetNumberOfLives(3);
        SceneManager.LoadScene(scene.name);
    }

}
