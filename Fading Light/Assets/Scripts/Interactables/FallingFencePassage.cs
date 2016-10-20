// file:	Assets\Scripts\Interactables\FallingFencePassage.cs
//
// summary:	Implements the falling fence passage class

using UnityEngine;
using System.Collections;

/// <summary>   A falling fence passage. </summary>
///
/// <remarks>    . </remarks>

public class FallingFencePassage : MonoBehaviour {

    /// <summary>   True to left wall. </summary>
	private bool _leftWall = false;
    /// <summary>   True to right wall. </summary>
	private bool _rightWall = false;
    /// <summary>   True to walls down. </summary>
	private bool _wallsDown = false;
    /// <summary>   The lever sound. </summary>
    public AudioSource LeverSound;

    /// <summary>   The left fence. </summary>
    private GameObject leftFence;
    /// <summary>   The right fence. </summary>
    private GameObject rightFence;

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
         LeverSound.loop = true;
    }

    /// <summary>   Sets left wall. </summary>
    ///
 

	public void SetLeftWall() {
		_leftWall = true;
	}

    /// <summary>   Sets right wall. </summary>
    ///
 

	public void SetRightWall() {
		_rightWall = true;
	}

    /// <summary>   Updates this object. </summary>
    ///
 

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
