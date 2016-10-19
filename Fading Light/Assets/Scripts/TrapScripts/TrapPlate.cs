using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class TrapPlate : MonoBehaviour {

	public GameObject[] traps;
	public List<GameObject> _trapList;
	private int _thingsOnTop = 0;
	public bool Pressed = false;
	public GameObject[] otherPlates;
	public bool Disabled;
	/// <summary>
	/// Called when an object enters on top of the plate
	/// </summary>

	void Awake(){
		otherPlates = GameObject.FindGameObjectsWithTag ("SpearTrap");
		for (int i = 0; i < traps.Length; i++) {
			_trapList.Add(traps[i]);
		}
		Disabled = false;
	}


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
	/// Called when an object leaves the plate
	/// </summary>
	public void UnsetPlate(GameObject plate) {
		GameObject[] otherTraps = plate.GetComponent<TrapPlate> ().traps;
		bool duplicate = false;
		if (plate.GetComponent<TrapPlate>().Pressed) {
			plate.GetComponent<Animation> ().Play ("PressurePlateUp");
			for (int i = 0; i < otherTraps.Length; i++) {
				duplicate = false;
				for (int j = 0; j < traps.Length; j++) {
				if (otherTraps[i].name.Equals (traps [j].name)) {
						_trapList.Remove (traps[j]);
					duplicate = true;
					break;
				}
				}
				if (duplicate == false) {
					plate.GetComponent<TrapPlate> ().SetTraps (otherTraps [i]);
				}

			}
			plate.GetComponent<TrapPlate>().Pressed = false;
		}
	}

	public void UnsetTraps(){
		foreach (GameObject o in _trapList) {
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
		}
	}

	public void SetTraps(GameObject trap){
			trap.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");

	}
}
