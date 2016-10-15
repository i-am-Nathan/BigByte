using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the levers animations and triggers as well as the animations of the moving walls
/// </summary>
public class LeverGate : MonoBehaviour
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

            Transform spearWall = transform.parent.transform.Find("spear_wall");

            foreach (Transform spearBlock in spearWall)
                spearBlock.transform.Find("Spear").GetComponent<Animation>().Play("Spear_Fall");

            _pulled = true;
        }
    }
}
