using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player 1") {
			print ("Player 1 Picked it up ");
			Destroy (gameObject);
		} else if (other.name == "Player2") {
			print ("Player 2 Picked it up ");
			Destroy (gameObject);

		}

	}
}
