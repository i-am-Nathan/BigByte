using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSpikeTrap : MonoBehaviour {

	public GameObject[] Traps;
	private int _thingsOnTop = 0;
	private bool _pressed = false;

	void OnTriggerEnter(Collider other) {
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop += 1;
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop >= 1 && !_pressed) {
			this.GetComponent<Animation>().Play("PressurePlateDown");
			UnsetTraps ();
			_pressed = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name == "Player 1" || other.name == "Player2")
		{
			if (_thingsOnTop != 0) {
				_thingsOnTop -= 1;
			}
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop == 0 && _pressed) {
			this.GetComponent<Animation>().Play("PressurePlateUp");
			SetTraps ();
			_pressed = false;
		}
	}
		
	public void UnsetTraps(){
		foreach (GameObject o in Traps) {
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
		}
	}

	public void SetTraps(){
		foreach (GameObject o in Traps) {
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");
		}
	}
}
