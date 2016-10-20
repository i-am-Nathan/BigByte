using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the levers animations and triggers as well as the animations of the moving walls
/// </summary>
public class Lever : MonoBehaviour
{
    private bool _pulled = false;

    public AudioClip leverPulled;
    private AudioSource _source;
    public bool _playSound = false;
    private GameObject rightMovingWall;
    private GameObject leftMovingWall;

    public AudioSource WallSound;
    void Awake()
    {
        //_source = GetComponent<AudioSource>();
    }

    void Start()
    {
        WallSound.loop = true;
    }

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
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
    void OnTriggerExit(Collider other)
    {
        //_source.PlayOneShot(leverPulled);
    }
    
}
