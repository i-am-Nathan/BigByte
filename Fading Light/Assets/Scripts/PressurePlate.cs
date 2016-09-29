using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = true;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}

	void OnTriggerLeave(Collider other) {
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = false;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}
}
