using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the levers animations and triggers as well as the animations of the moving walls
/// </summary>
public class lever : MonoBehaviour
{
    private bool _pulled = false;

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        //if T is pressed to interact with the lever, the walls move
        if (Input.GetKeyDown(KeyCode.T) && !_pulled)
        {
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
}
