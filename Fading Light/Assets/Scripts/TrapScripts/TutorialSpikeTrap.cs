// file:	Assets\Scripts\TrapScripts\TutorialSpikeTrap.cs
//
// summary:	Implements the tutorial spike trap class

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>   A tutorial spike trap. </summary>
///
/// <remarks>    . </remarks>

public class TutorialSpikeTrap : MonoBehaviour {

    /// <summary>   The traps. </summary>
	public GameObject[] Traps;
    /// <summary>   The other plate. </summary>
	public GameObject OtherPlate; 

    /// <summary>   The other plate script. </summary>
	private TutorialSpikeTrap _otherPlateScript;
    /// <summary>   The things on top. </summary>
	private int _thingsOnTop = 0;
    /// <summary>   True if pressed. </summary>
	private bool _pressed = false;

    /// <summary>   The spike sound. </summary>
    public AudioSource SpikeSound;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		_otherPlateScript = OtherPlate.GetComponent<TutorialSpikeTrap>();	
	}

    /// <summary>   Query if this object is pressed. </summary>
    ///
 
    ///
    /// <returns>   True if pressed, false if not. </returns>

	public bool isPressed() {
		return _pressed;
	}

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

	void OnTriggerEnter(Collider other) {
		if (other.name == "Player 1" || other.name == "Player2")
		{
			_thingsOnTop += 1;
		} 

		//if the weight is heavy enough, then the plate is triggered
		if (_thingsOnTop >= 1 && !_pressed) {
            
			this.GetComponent<Animation>().Play("PressurePlateDown");
         
			Debug.Log ("Unsetting");
			UnsetTraps ();
			_pressed = true;
		}
	}

    /// <summary>   Executes the trigger exit action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

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
			if (!_otherPlateScript.isPressed()) {
				Debug.Log ("Setting");
				SetTraps ();
			}
			_pressed = false;
		}
	}

    /// <summary>   Unset traps. </summary>
    ///
 

	public void UnsetTraps(){
		foreach (GameObject o in Traps) {
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
		}
	}

    /// <summary>   Sets the traps. </summary>
    ///
 

	public void SetTraps(){
		foreach (GameObject o in Traps) {
			o.GetComponent<Animation>().Play("Anim_TrapNeedle_Show");
		}

        SpikeSound.Play();
    }
}
