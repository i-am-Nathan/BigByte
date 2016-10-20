using UnityEngine;
using System.Collections;

public class FallingFencePassage : MonoBehaviour {

	private bool _leftWall = false;
	private bool _rightWall = false;
	private bool _wallsDown = false;
    public AudioSource LeverSound;

    private GameObject leftFence;
    private GameObject rightFence;
    void Start()
    {
         LeverSound.loop = true;
    }
	public void SetLeftWall() {
		_leftWall = true;
	}

	public void SetRightWall() {
		_rightWall = true;
	}

	void Update () {
		if (_leftWall && _rightWall && !_wallsDown) {
			Debug.Log ("Playing final animation");
			rightFence = GameObject.Find("Falling Fence Right");
			rightFence.GetComponent<Animation>().Play("FallingWallFall");

			leftFence = GameObject.Find("Falling Fence Left");
			leftFence.GetComponent<Animation>().Play("FallingWallFall");
            LeverSound.Play();

			_wallsDown = true;
		}
        else if(_wallsDown && !(leftFence.GetComponent<Animation>().isPlaying || rightFence.GetComponent<Animation>().isPlaying))
        {
            LeverSound.Stop();
        }
	}
}
