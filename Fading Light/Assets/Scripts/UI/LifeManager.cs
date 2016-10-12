﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Used to manage the shared lives between the two players
/// </summary>
public class LifeManager : MonoBehaviour
{
	// Handling
    private GameObject[] _lives;
    private int _numberOfLivesLeft;
    private GameData _gameDataScript;

	// UI element
    public Canvas DeathScreen;

	/// <summary>
	/// Called when instance initialised
	/// </summary>
    void Start()
    {
        // Getting the life track object which shows the total lives left
        GameObject go = GameObject.FindGameObjectWithTag("Game Data");
        _gameDataScript = (GameData)go.GetComponent(typeof(GameData));

        // Getting the hearts on the UI
        _lives = GameObject.FindGameObjectsWithTag("Lives");
        _numberOfLivesLeft = _gameDataScript.GetNumberOfLives();

        // Setting the appropriate hearts to show
        for (int i = 0; i < _numberOfLivesLeft; i++)
        {
            _lives[i].SetActive(true);
        }

        // Setting the appropriate hearts to hide
        if (_numberOfLivesLeft != 3)
        {
            for (int i = 2; i > _numberOfLivesLeft - 1; i--)
            { 
            _lives[i].SetActive(false);
            }
        }

        // Hiding the game over screen
        DeathScreen.enabled = false;
    }

	/// <summary>
	/// Called whenever a player dies
	/// </summary>
    public void LoseLife()
    {
		// Decrementing the number of shared lives left
        _numberOfLivesLeft = _numberOfLivesLeft - 1;
        _gameDataScript.SetNumberOfLives(_numberOfLivesLeft);

		// Checking if all 3 lives are over
        if (_numberOfLivesLeft <= 0)
        {
            // Game over
            // Pausing the game and showing the end of game screen
            _lives[2 - _numberOfLivesLeft].SetActive(false);
            Time.timeScale = 0;
            DeathScreen.enabled = true;
        }
        else
        {
            // Removing a heart from the UI
            _lives[2-_numberOfLivesLeft].SetActive(false);
            StartCoroutine("Wait");
        }
    }

	/// <summary>
	/// Called when a delay of 3 seconds is required before reloading the level
	/// </summary>
    private IEnumerator Wait()
    {
		// Waiting for 3 seconds
        yield return new WaitForSeconds(2f);
        // Restart level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
		
	/// <summary>
	/// Called when quit to main menu button is clicked
	/// Loads the main menu screen
	/// </summary>
    public void QuitToMenu()
    {
		// Unpausing the game and loading the main menu
        Time.timeScale = 1;
        // Setting the number of shared lives back to 3
        _gameDataScript.SetNumberOfLives(3);
        SceneManager.LoadScene("MainMenu");
    }
		
	/// <summary>
	/// Called when restart level button is clicked
	/// </summary>
    public void RestartLevel()
    {
		// Unpausing the game
        Time.timeScale = 1;
        // Gets the active scene and reloads it
        Scene scene = SceneManager.GetActiveScene();



        ////////////////////////////////////////////////////////////////////////////////////////////// Need to make start of level script that records stuff
        _gameDataScript.SetNumberOfLives(3);
        SceneManager.LoadScene(scene.name);
    }

}