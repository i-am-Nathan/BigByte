using UnityEngine;
using System.Collections;

public class SpearLaunch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("PlaySound", 0, 3);
	}
	
	// Update is called once per frame
	void PlaySound () {
		AudioSource[] a = GetComponents<AudioSource> ();
		a [1].Play ();
	}
}
