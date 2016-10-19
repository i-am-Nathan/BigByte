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

    void Awake()
    {
        //_source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        //if T is pressed to interact with the lever, the walls move
        if (Input.GetKeyDown(KeyCode.T) && !_pulled)
        {

            _source.PlayOneShot(leverPulled);
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            GameObject rightMovingWall = GameObject.FindGameObjectWithTag("Right Moving Wall");

            rightMovingWall.GetComponent<Animation>().Play("RightMovingWallOut");

            GameObject leftMovingWall = GameObject.FindGameObjectWithTag("Left Moving Wall");

            leftMovingWall.GetComponent<Animation>().Play("LeftMovingWallOut");
            _pulled = true;

            //hides the popup
            GameObject leverPopup = GameObject.FindGameObjectWithTag("Lever Key Popup");
            leverPopup.SetActive(false);

        }
    }
    
    void OnTriggerExit(Collider other)
    {
        //_source.PlayOneShot(leverPulled);
    }
    
}
