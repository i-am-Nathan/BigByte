using UnityEngine;
using System.Collections;

/// <summary>
/// This class refers to the pressure plate which can control a moving platform.
/// </summary>
public class FloatingPressurePlate : MonoBehaviour {


    private int _thingsOnTop = 0;
    private bool _pressed = false;
    public GameObject floater;

	/// <summary>
	/// This will be used to detect when players get on the plate and depending on which floating platform it corresponds
	/// to, it will resume it's movement.
	/// </summary>
	/// <param name="other">Other.</param>
    void OnTriggerEnter(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") {
			_thingsOnTop++;

			//if the weight is heavy enough, then the plate is triggered
			if (_thingsOnTop >= 1 && !_pressed) {
				this.GetComponent<Animation> ().Play ("PressurePlateDown");
				floater.gameObject.GetComponent <FloatingPlate> ().Resume ();
				_pressed = true;

			}
		}

    }

	/// <summary>
	/// This will pause the movement of the platform the plate corresponds to.
	/// </summary>
	/// <param name="other">Other.</param>
    void OnTriggerExit(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") {
			_thingsOnTop--;

			//if the weight is heavy enough, then the plate is triggered
			if (_thingsOnTop < 1 && _pressed) {
				this.GetComponent<Animation> ().Play ("PressurePlateUp");
				floater.gameObject.GetComponent<FloatingPlate> ().Stop ();
				_pressed = false;

			}
		}

    }

}
