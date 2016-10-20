// file:	Assets\Scripts\Interactables\FallingFenceLeverRight.cs
//
// summary:	Implements the falling fence lever right class

using UnityEngine;
using System.Collections;

/// <summary>   A falling fence lever right. </summary>
///
/// <remarks>    . </remarks>

public class FallingFenceLeverRight : MonoBehaviour {

    /// <summary>   True if pulled. </summary>
	private bool _pulled = false;
    /// <summary>   True to right wall. </summary>
	private bool _rightWall = false;
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

			if (_rightWall == false) {
				fallingFencePassage.SetRightWall ();
				_rightWall = true;
			} 
		}
	}
}
