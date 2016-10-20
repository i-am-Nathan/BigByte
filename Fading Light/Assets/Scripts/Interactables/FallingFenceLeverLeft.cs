using UnityEngine;
using System.Collections;

public class FallingFenceLeverLeft : MonoBehaviour {

	private bool _pulled = false;
	private bool _leftWall = false;
    public AudioClip LeverSound;
    private AudioSource _source;
    

	/// <summary>
	/// Called when the player is close enough to the lever, and presses T
	/// </summary>
	void OnTriggerStay(Collider other)
	{
		//if T is pressed to interact with the lever, the walls move
		if (((other.name.Equals("Player 1") && Input.GetKeyDown(KeyCode.O)) || (other.name.Equals("Player2") && Input.GetKeyDown(KeyCode.Q))) && !_pulled)	
		{
			this.GetComponent<Animation>().Play("Armature|LeverDown");
            _source.PlayOneShot(LeverSound);
			_pulled = true;
			GameObject go = GameObject.Find("Falling Fence Passage");
			FallingFencePassage fallingFencePassage = (FallingFencePassage)go.GetComponent(typeof(FallingFencePassage));

			if (_leftWall == false) {
				fallingFencePassage.SetLeftWall ();
				_leftWall = true;
			} 
		}
	}
}
