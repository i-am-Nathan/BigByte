﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Used to add sound of a crate moving to the crates in the game
/// </summary>
public class CrateSoundScript : MonoBehaviour {

	// Audio source
    public AudioSource CrateMovementSound;
    private Rigidbody rb;

	/// <summary>
	/// Initialising crate sounds
	/// </summary>
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        CrateMovementSound.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if(rb.velocity.magnitude >=0.1 && !CrateMovementSound.isPlaying)
        {
            CrateMovementSound.Play();
        }
        else if (rb.velocity.magnitude == 0)
        {
            CrateMovementSound.Stop();
        }
	}
}
