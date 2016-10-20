using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class NeedleAndPlate : MonoBehaviour {

    private bool _pressed = false;
	private Transform needle;
	AudioSource _plateDown;
	AudioSource _plateUp;
	AudioSource _needleDown;
	AudioSource _needleUp;

	void Start(){
		needle = transform.parent.parent.Find ("Trap_Needle").Find ("Needle");
		AudioSource[] aSourcePlate = GetComponents<AudioSource>();
		AudioSource[] aSourceNeedle = needle.gameObject.GetComponents <AudioSource>();
		_plateDown = aSourcePlate [0];
		_plateUp = aSourcePlate [1];
		_needleDown = aSourceNeedle [0];
		_needleUp = aSourceNeedle [1];
	}

    /// <summary>
    /// Called when an object enters on top of the plate
    /// </summary>
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


    /// <summary>
    /// Called when an object leaves the plate
    /// </summary>
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
