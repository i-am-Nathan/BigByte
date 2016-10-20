// file:	Assets\Scripts\Interactables\LeverGate.cs
//
// summary:	Implements the lever gate class

using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the levers animations and triggers as well as the animations of the
/// moving walls.
/// </summary>
///
/// <remarks>    . </remarks>

public class LeverGate : MonoBehaviour
{
    /// <summary>   True if pulled. </summary>
    private bool _pulled = false;

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

            Transform spearWall = transform.parent.transform.Find("spear_wall");

			foreach (Transform spearBlock in spearWall) {
				spearBlock.transform.Find ("Spear").GetComponent<Animation> ().Play ("Spear_Fall");
				if(spearBlock.name.Equals("spear_block (4)")) spearBlock.GetComponent<AudioSource>().Play();				
			}
            _pulled = true;
        }
    }
}
