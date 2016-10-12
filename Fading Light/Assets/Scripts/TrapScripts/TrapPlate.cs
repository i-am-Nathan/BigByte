using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class TrapPlate : MonoBehaviour {

	public GameObject[] traps;
	private int _thingsOnTop = 0;
	public bool _pressed = false;
	public GameObject[] otherPlates;

	/// <summary>
	/// Called when an object enters on top of the plate
	/// </summary>

	void Awake(){
		otherPlates = GameObject.FindGameObjectsWithTag ("SpearTrap");
	}


	void OnTriggerEnter(Collider other) {
		// the crate has a weight of 2
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop += 1;
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop >= 1 && !_pressed)

		{
			this.GetComponent<Animation>().Play("PressurePlateDown");

			for (int j = 0; j < otherPlates.Length; j++) {
				otherPlates [j].GetComponent<TrapPlate> ().UnsetPlate ();
			}
			UnsetTraps ();
			_pressed = true;

		}

	}


	/// <summary>
	/// Called when an object leaves the plate
	/// </summary>
	public void UnsetPlate() {

		if (_pressed) {
			this.GetComponent<Animation>().Play("PressurePlateUp");
			this.GetComponent<TrapPlate> ().SetTraps ();;
			_pressed = false;
		}
			
	}

	public void UnsetTraps(){
		for (int i = 0; i < traps.Length; i++) {
			traps[i].GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
		}
	}

	public void SetTraps(){
		for (int i = 0; i < traps.Length; i++) {
			traps[i].GetComponent<Animation>().Play("Anim_TrapNeedle_Show");

		}

	}
}
