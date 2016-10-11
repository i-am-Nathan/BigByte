using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class TrapPlate : MonoBehaviour {

	private int _thingsOnTop = 0;
	private bool _pressed = false;

	/// <summary>
	/// Called when an object enters on top of the plate
	/// </summary>
	void OnTriggerEnter(Collider other) {
		Debug.Log ("GGGGG");
		// the crate has a weight of 2
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop += 1;
			Debug.Log ("GG");
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop >= 1 && !_pressed)

		{
			this.GetComponent<Animation>().Play("PressurePlateDown");
			GameObject wall = GameObject.FindWithTag("SpearTrap");
			wall.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
			_pressed = true;

		}

	}


	/// <summary>
	/// Called when an object leaves the plate
	/// </summary>
	void OnTriggerExit(Collider other) {
		//same as the method above, but for the upward motion.
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop--;

		} 

		if (_thingsOnTop < 1 && _pressed)

		{
			this.GetComponent<Animation>().Play("PressurePlateUp");
			GameObject wall = GameObject.FindWithTag("SpearTrap");
			wall.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");
			_pressed = false;

		}


	}
}
