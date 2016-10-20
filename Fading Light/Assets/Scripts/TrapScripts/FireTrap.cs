// file:	Assets\Scripts\TrapScripts\FireTrap.cs
//
// summary:	Implements the fire trap class

using UnityEngine;
using System.Collections;

/// <summary>   A fire trap. </summary>
///
/// <remarks>    . </remarks>

public class FireTrap : MonoBehaviour {

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		InvokeRepeating ("StartEmitting", 3, 6);
		InvokeRepeating ("StopLight", 5, 6);
	}

    /// <summary>   Starts an emitting. </summary>
    ///
 

	void StartEmitting(){
		GetComponent<AudioSource> ().Play ();
		gameObject.GetComponent<ParticleSystem> ().Play();
		transform.Find ("Point light").gameObject.SetActive(true);
	}		

    /// <summary>   Stops a light. </summary>
    ///
 

	void StopLight(){
		GetComponent<AudioSource> ().Stop ();
		transform.Find ("Point light").gameObject.SetActive (false);
	}
}
