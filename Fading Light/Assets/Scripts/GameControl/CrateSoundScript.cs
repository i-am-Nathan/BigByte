// file:	Assets\Scripts\GameControl\CrateSoundScript.cs
//
// summary:	Implements the crate sound script class

using UnityEngine;
using System.Collections;

/// <summary>   Used to add sound of a crate moving to the crates in the game. </summary>
///
/// <remarks>    . </remarks>

public class CrateSoundScript : MonoBehaviour {

	// Audio source
    /// <summary>   The crate movement sound. </summary>
    public AudioSource CrateMovementSound;
    /// <summary>   The rb. </summary>
    private Rigidbody rb;

    /// <summary>   Initialising crate sounds. </summary>
    ///
 

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        CrateMovementSound.loop = true;
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

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
