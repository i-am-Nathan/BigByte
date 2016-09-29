using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class PressurePlate : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		this.GetComponent<Animation> ().Play ("PressurePlateDown");
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = true;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}

	void OnTriggerExit(Collider other) {
		this.GetComponent<Animation> ().Play ("PressurePlateUp");
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = false;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}
}
