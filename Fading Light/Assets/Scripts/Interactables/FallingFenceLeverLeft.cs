using UnityEngine;
using System.Collections;

public class FallingFenceLeverLeft : MonoBehaviour {

	private bool _pulled = false;
	private bool _leftWall = false;

	/// <summary>
	/// Called when the player is close enough to the lever, and presses T
	/// </summary>
	void OnTriggerStay(Collider other)
	{
		//if T is pressed to interact with the lever, the walls move
		if (Input.GetKeyDown(KeyCode.T) && !_pulled)	
		{
			this.GetComponent<Animation>().Play("Armature|LeverDown");

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
