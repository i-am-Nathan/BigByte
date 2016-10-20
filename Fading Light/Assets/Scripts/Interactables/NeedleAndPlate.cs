// file:	Assets\Scripts\Interactables\NeedleAndPlate.cs
//
// summary:	Implements the needle and plate class

using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations.
/// </summary>
///
/// <remarks>    . </remarks>

public class NeedleAndPlate : MonoBehaviour {

    /// <summary>   True if pressed. </summary>
    private bool _pressed = false;
    /// <summary>   The needle. </summary>
	private Transform needle;
    /// <summary>   The plate down. </summary>
	AudioSource _plateDown;
    /// <summary>   The plate up. </summary>
	AudioSource _plateUp;
    /// <summary>   The needle down. </summary>
	AudioSource _needleDown;
    /// <summary>   The needle up. </summary>
	AudioSource _needleUp;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start(){
		needle = transform.parent.parent.Find ("Trap_Needle").Find ("Needle");
		AudioSource[] aSourcePlate = GetComponents<AudioSource>();
		AudioSource[] aSourceNeedle = needle.gameObject.GetComponents <AudioSource>();
		_plateDown = aSourcePlate [0];
		_plateUp = aSourcePlate [1];
		_needleDown = aSourceNeedle [0];
		_needleUp = aSourceNeedle [1];
	}

    /// <summary>   Called when an object enters on top of the plate. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other) {
        //if the weight is heavy enough, then the plate is triggered
		if ((other.tag.Equals("Player") || other.tag.Equals("Player2")) && !_pressed)
        {
			_plateDown.Play ();
			_needleDown.Play ();
			GetComponent<Animation> ().Play ("ppdown");
			needle.GetComponent<Animation> ().Play("NeedleDown");
            _pressed = true;
        }
        
    }

    /// <summary>   Called when an object leaves the plate. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerExit(Collider other) {
        //same as the method above, but for the upward motion.

		if ((other.tag.Equals("Player") || other.tag.Equals("Player2")) && _pressed)
        {
			_plateUp.Play ();
			_needleUp.Play ();
			GetComponent<Animation> ().Play ("ppup");
			needle.GetComponent<Animation> ().Play("NeedleUp");
			_pressed = false;
        }
    }
}
