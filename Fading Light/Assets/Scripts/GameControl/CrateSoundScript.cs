using UnityEngine;
using System.Collections;

public class CrateSoundScript : MonoBehaviour {

    public AudioSource CrateMovementSound;
    private Rigidbody rb;
	// Use this for initialization
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
