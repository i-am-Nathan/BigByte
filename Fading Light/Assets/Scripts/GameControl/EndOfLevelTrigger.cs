using UnityEngine;
using System.Collections;

/// <summary>
/// Used to register when players have reached the end of the level
/// </summary>
public class EndOfLevelTrigger : MonoBehaviour {

	// Canvas indicating to players they have finished the level
    public Canvas LevelFinishedMenu;
	private bool _player1Entered = false;
	private bool _player2Entered = false;

	void Update () {
		if (_player1Entered && _player2Entered) {
			// Pauses the game and shows the menu indicating that players have completed the level
			Time.timeScale = 0;
			LevelFinishedMenu.enabled = true;
		}
	}

	/// <summary>
	/// Used for initialisation
	/// </summary>
    void Start()
    {
		// Hiding the menu
        LevelFinishedMenu.enabled = false;
    }

	/// <summary>
	/// Called when a player enters the box collider placed at the end of the level
	/// </summary>
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
			_player1Entered = true;
        }

		if (c.gameObject.tag == "Player2") {
			_player2Entered = true;
		}
    }

	void OnTriggerExit(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			_player1Entered = false;
		}

		if (c.gameObject.tag == "Player2") {
			_player2Entered = false;
		}
	}
}
