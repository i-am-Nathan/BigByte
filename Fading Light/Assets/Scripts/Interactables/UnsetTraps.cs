using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class UnsetTraps : MonoBehaviour {

	private GameObject[] _otherPlates;
	private bool _pressed=false;

	void Awake(){
		_otherPlates = GameObject.FindGameObjectsWithTag ("SpearTrap");
	}
	void OnTriggerEnter(Collider other) {
		if (other.name == "Player 1" || other.name == "Player2") {
			if (!_pressed){
			gameObject.GetComponent<Animation> ().Play ("PressurePlateDown");
				_pressed = true;
				foreach (GameObject o in _otherPlates) {
				o.GetComponent<TrapPlate> ().UnsetTraps ();
			}
			}
		}
	}
		
}
