using UnityEngine;
using System.Collections;

/// <summary>
/// This will correspond to a platform which will raise a path when pressed.
/// </summary>
public class RaisePath : MonoBehaviour {

	private int _thingsOnTop = 0;
	public GameObject path;
	private bool _pressed = false;

	/// <summary>
	/// Detects when the plate has been pressed and raises a path for it.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		if (!_pressed) {
			//if the weight is heavy enough, then the plate is triggered
			if (other.name == "Player 1" || other.name == "Player2") {
				this.GetComponent<Animation> ().Play ("PressurePlateDown");
				path.GetComponent<Animation> ().Play ();
				_pressed = true;
			}
		}
	}
}
