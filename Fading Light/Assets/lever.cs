using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour
{
    private bool _pulled = false;
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.T) && !_pulled)
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            GameObject rightMovingWall = GameObject.FindGameObjectWithTag("Right Moving Wall");

            rightMovingWall.GetComponent<Animation>().Play("RightMovingWallOut");

            GameObject leftMovingWall = GameObject.FindGameObjectWithTag("Left Moving Wall");

            leftMovingWall.GetComponent<Animation>().Play("LeftMovingWallOut");
            _pulled = true;
        }
    }
}
