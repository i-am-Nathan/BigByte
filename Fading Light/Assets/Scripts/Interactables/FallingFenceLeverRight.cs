using UnityEngine;
using System.Collections;

public class FallingFenceLeverRight : MonoBehaviour {

	private bool _pulled = false;
	private bool _rightWall = false;
	public AudioSource LeverSound;

	/// <summary>
	/// Called when the player is close enough to the lever, and presses T
	/// </summary>
	void OnTriggerStay(Collider other)
	{
		//if T is pressed to interact with the lever, the walls move
		if (((other.name.Equals("Player 1") && Input.GetKeyDown(KeyCode.O)) || (other.name.Equals("Player2") && Input.GetKeyDown(KeyCode.Q))) && !_pulled)	
		{
			this.GetComponent<Animation>().Play("Armature|LeverDown");
            LeverSound.Play();
			_pulled = true;
			GameObject go = GameObject.Find("Falling Fence Passage");
			FallingFencePassage fallingFencePassage = (FallingFencePassage)go.GetComponent(typeof(FallingFencePassage));

			if (_rightWall == false) {
				fallingFencePassage.SetRightWall ();
				_rightWall = true;
			} 
		}
	}
}
