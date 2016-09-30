using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class PressurePlate : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
        Debug.Log("aaa");
		this.GetComponent<Animation> ().Play ("PressurePlateDown");
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = true;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}

	void OnCollisionExit(Collision collision) {
		this.GetComponent<Animation> ().Play ("PressurePlateUp");
		GameObject wall = GameObject.FindWithTag ("Falling Wall");
		wall.GetComponent<FallingWall>().pressurePlate1 = false;
		wall.GetComponent<FallingWall>().pressurePlate2 = true;
	}
}
