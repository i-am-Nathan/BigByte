// file:	Assets\Scripts\Puzzles\RotatingDoor1.cs
//
// summary:	Implements the rotating door 1 class

using UnityEngine;
using System.Collections;

/// <summary>   A rotating door 1. </summary>
///
/// <remarks>    . </remarks>

public class RotatingDoor1 : MonoBehaviour {

    /// <summary>   The force amount. </summary>
    public float forceAmount = 1000f;

    /// <summary>   Executes the mouse down action. </summary>
    ///
 

    void OnMouseDown()
    {
        Debug.Log("yolo6");
        GetComponent<Rigidbody>().AddForce(-transform.forward * forceAmount, ForceMode.Acceleration);
    }
}
