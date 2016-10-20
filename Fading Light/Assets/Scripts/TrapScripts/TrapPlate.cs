// file:	assets\scripts\trapscripts\trapplate.cs
//
// summary:	Implements the trapplate class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script controls the pressure plate animation as well as the spear trap animations.
/// </summary>
///
/// <remarks>    . </remarks>

public class TrapPlate : MonoBehaviour {

    /// <summary>   The traps. </summary>
	public GameObject[] traps;
    /// <summary>   List of traps. </summary>
	public List<GameObject> _trapList;
    /// <summary>   The things on top. </summary>
	private int _thingsOnTop = 0;
    /// <summary>   True if pressed. </summary>
	public bool Pressed = false;
    /// <summary>   The other plates. </summary>
	public GameObject[] otherPlates;
    /// <summary>   True to disable, false to enable. </summary>
	public bool Disabled;
//	AudioSource ppdown;
//	AudioSource ppup;

    /// <summary>   Initialises my variables and grabs my list of traps. </summary>
    ///
 

	void Awake(){
//		AudioSource[] sounds = GetComponents<AudioSource> ();
//		ppdown = sounds [0];
//		ppup = sounds [1];
		otherPlates = GameObject.FindGameObjectsWithTag ("SpearTrap");
		for (int i = 0; i < traps.Length; i++) {
			_trapList.Add(traps[i]);
		}
		Disabled = false;
	}

    /// <summary>
    /// This will set the plates down and unset the other plates that are current set.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerEnter(Collider other) {
		// the crate has a weight of 2
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop += 1;
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop >= 1 && !Pressed && !Disabled)

		{
			
			this.GetComponent<Animation>().Play("PressurePlateDown");
	//		ppdown.Play ();
			
			//Ensures that the animation plays smoothly.
			_trapList.Clear ();
			for (int i = 0; i < traps.Length; i++) {
				_trapList.Add(traps[i]);
			}
			for (int j = 0; j < otherPlates.Length; j++) {
				if (gameObject.name != otherPlates [j].name) {
					UnsetPlate (otherPlates [j]);
				}
			}
			UnsetTraps ();
			Pressed = true;

		}

	}

    /// <summary>
    /// Called when a player goes to another plate. This will reset all the traps for that particular
    /// plate.
    /// </summary>
    ///
 
    ///
    /// <param name="plate">    The plate. </param>

	public void UnsetPlate(GameObject plate) {
		GameObject[] otherTraps = plate.GetComponent<TrapPlate> ().traps;
		//Duplicate variable needed to ensure that you dont reset your own traps.
		bool duplicate = false;
		if (plate.GetComponent<TrapPlate>().Pressed) {
			plate.GetComponent<Animation> ().Play ("PressurePlateUp");
	//		ppup.Play ();
			for (int i = 0; i < otherTraps.Length; i++) {
				duplicate = false;
				for (int j = 0; j < traps.Length; j++) {
				if (otherTraps[i].name.Equals (traps [j].name)) {
						_trapList.Remove (traps[j]);
					duplicate = true;
					break;
				}
				}
				//Won't set your own trap.
				if (duplicate == false) {
					plate.GetComponent<TrapPlate> ().SetTraps (otherTraps [i]);
				}

			}
			plate.GetComponent<TrapPlate>().Pressed = false;
		}
	}

    /// <summary>   This will play the animation which hides the traps. </summary>
    ///
 

	public void UnsetTraps(){
		foreach (GameObject o in _trapList) {
			//o.GetComponent<AudioSource> ().Play ();
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
		}
	}

    /// <summary>   This will play the animation where it sets the traps. </summary>
    ///
 
    ///
    /// <param name="trap"> Trap. </param>

	public void SetTraps(GameObject trap){
		//trap.GetComponent<AudioSource> ().Play ();
		trap.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");

	}
}
