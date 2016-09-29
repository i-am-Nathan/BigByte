using UnityEngine;
using System.Collections;

public class FallingWall : MonoBehaviour {

	public bool pressurePlate1 = false;
	public bool pressurePlate2 = false;
	public Animator animator;

	void Start() {
		animator.enabled = false;
	}


	// Update is called once per frame
	void Update () {
		if (pressurePlate1 && pressurePlate2) {
			Debug.Log ("Falling");
			animator.enabled = true;
		}
	}
}
