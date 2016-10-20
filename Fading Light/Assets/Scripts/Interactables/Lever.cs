// file:	Assets\Scripts\Interactables\Lever.cs
//
// summary:	Implements the lever class

using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the levers animations and triggers as well as the animations of the
/// moving walls.
/// </summary>
///
/// <remarks>    . </remarks>

public class Lever : MonoBehaviour
{
    /// <summary>   True if pulled. </summary>
    private bool _pulled = false;

    /// <summary>   The lever pulled. </summary>
    public AudioClip leverPulled;
    /// <summary>   Source for the. </summary>
    private AudioSource _source;
    /// <summary>   True to play sound. </summary>
    public bool _playSound = false;
    /// <summary>   The right moving wall. </summary>
    private GameObject rightMovingWall;
    /// <summary>   The left moving wall. </summary>
    private GameObject leftMovingWall;

    /// <summary>   The wall sound. </summary>
    public AudioSource WallSound;

    /// <summary>   Awakes this object. </summary>
    ///
 

    void Awake()
    {
        //_source = GetComponent<AudioSource>();
    }

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        WallSound.loop = true;
    }

    /// <summary>   Called when the player is close enough to the lever, and presses T. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        //if T is pressed to interact with the lever, the walls move
		if (((other.name.Equals("Player 1") && Input.GetKeyDown(KeyCode.O)) || (other.name.Equals("Player2") && Input.GetKeyDown(KeyCode.Q))) && !_pulled)
        {

            //_source.PlayOneShot(leverPulled);
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            rightMovingWall = GameObject.FindGameObjectWithTag("Right Moving Wall");

            rightMovingWall.GetComponent<Animation>().Play("RightMovingWallOut");

            leftMovingWall = GameObject.FindGameObjectWithTag("Left Moving Wall");

            leftMovingWall.GetComponent<Animation>().Play("LeftMovingWallOut");
            _pulled = true;

            //hides the popup
            //GameObject leverPopup = GameObject.FindGameObjectWithTag("Lever Key Popup");
            //leverPopup.SetActive(false);

            _playSound = true;

        }
    }

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        if (_playSound)
        {
            if (rightMovingWall.GetComponent<Animation>().isPlaying || leftMovingWall.GetComponent<Animation>().isPlaying)
            {
                if (!WallSound.isPlaying)
                {
                    Debug.Log("Wall sounds are playing");
                    WallSound.Play();

                }
            }
            else if (!((rightMovingWall.GetComponent<Animation>().isPlaying || leftMovingWall.GetComponent<Animation>().isPlaying)))
            {
                WallSound.Stop();
                _playSound = false;
            }
            
        }
    }

    /// <summary>   Executes the trigger exit action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerExit(Collider other)
    {
        //_source.PlayOneShot(leverPulled);
    }
    
}
