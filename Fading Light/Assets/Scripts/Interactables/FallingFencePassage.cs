﻿using UnityEngine;
using System.Collections;

public class FallingFencePassage : MonoBehaviour {

	private bool _leftWall = false;
	private bool _rightWall = false;
	private bool _wallsDown = false;

	public void SetLeftWall() {
		_leftWall = true;
	}

	public void SetRightWall() {
		_rightWall = true;
	}

	void Update () {
		if (_leftWall && _rightWall && !_wallsDown) {
			Debug.Log ("Playing final animation");
			GameObject rightFence = GameObject.Find("Falling Fence Right");
			rightFence.GetComponent<Animation>().Play("FallingWallFall");

			GameObject leftFence = GameObject.Find("Falling Fence Left");
			leftFence.GetComponent<Animation>().Play("FallingWallFall");

			_wallsDown = true;
		}
	}
}
