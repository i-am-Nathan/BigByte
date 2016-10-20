// file:	Assets\Scripts\Interactables\UnsetTraps.cs
//
// summary:	Implements the unset traps class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script controls the spiked traps and unsets all the traps when the player reaches it.
/// </summary>
///
/// <remarks>    . </remarks>

public class UnsetTraps : MonoBehaviour {

    /// <summary>   The other plates. </summary>
	private GameObject[] _otherPlates;
    /// <summary>   True if pressed. </summary>
	private bool _pressed=false;

    /// <summary>   Gets a reference to all the traps. </summary>
    ///
 

	void Awake(){
		_otherPlates = GameObject.FindGameObjectsWithTag ("SpearTrap");
	}

    /// <summary>
    /// When a player enters the plate it will unset all the needles and disable the plates.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerEnter(Collider other) {
		if (other.name == "Player 1" || other.name == "Player2") {
			if (!_pressed){
			gameObject.GetComponent<Animation> ().Play ("PressurePlateDown");

				_pressed = true;
				foreach (GameObject o in _otherPlates) {
					o.GetComponent<TrapPlate> ().Disabled = true;
				o.GetComponent<TrapPlate> ().UnsetTraps ();
			}
			}
		}
	}
		
}
