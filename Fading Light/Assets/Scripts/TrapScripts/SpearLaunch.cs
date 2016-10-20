// file:	Assets\Scripts\TrapScripts\SpearLaunch.cs
//
// summary:	Implements the spear launch class

using UnityEngine;
using System.Collections;

/// <summary>   A spear launch. </summary>
///
/// <remarks>    . </remarks>

public class SpearLaunch : MonoBehaviour {

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		InvokeRepeating ("PlaySound", 0, 3);
	}
	
	// Update is called once per frame

    /// <summary>   Play sound. </summary>
    ///
 

	void PlaySound () {
		AudioSource[] a = GetComponents<AudioSource> ();
		a [1].Play ();
	}
}
