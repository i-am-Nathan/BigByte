using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("StartEmitting", 3, 6);
		InvokeRepeating ("StopLight", 5, 6);
	}

	void StartEmitting(){
		gameObject.GetComponent<ParticleSystem> ().Play();
		transform.Find ("Point light").gameObject.SetActive(true);
	}		
	void StopLight(){
		transform.Find ("Point light").gameObject.SetActive (false);
	}
}
