using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Vector3 cameraTarget;
	public PlayerController player;
	private Transform target;
	
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
		if (target != null) { 
			cameraTarget = new Vector3 (target.position.x, transform.position.y, target.position.z+1.5f);
			transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.deltaTime * 8);
		}
	}
}
