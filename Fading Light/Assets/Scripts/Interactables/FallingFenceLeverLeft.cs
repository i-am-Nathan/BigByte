// file:	Assets\Scripts\Interactables\FallingFenceLeverLeft.cs
//
// summary:	Implements the falling fence lever left class

using UnityEngine;
using System.Collections;

/// <summary>   A falling fence lever left. </summary>
///
/// <remarks>    . </remarks>

public class FallingFenceLeverLeft : MonoBehaviour {

    /// <summary>   True if pulled. </summary>
	private bool _pulled = false;
    /// <summary>   True to left wall. </summary>
	private bool _leftWall = false;
    /// <summary>   The lever sound. </summary>
    public AudioSource LeverSound;

    /// <summary>   Called when the player is close enough to the lever, and presses T. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

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

			if (_leftWall == false) {
				fallingFencePassage.SetLeftWall ();
				_leftWall = true;
			} 
		}
	}
}
