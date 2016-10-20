// file:	Assets\Scripts\Puzzles\RotatableWall.cs
//
// summary:	Implements the rotatable wall class

using UnityEngine;
using System.Collections;

/// <summary>   A rotatable wall. </summary>
///
/// <remarks>    . </remarks>

public class RotatableWall : MonoBehaviour
{
    /// <summary>   True to 1 contact. </summary>
    private bool p1Contact = false;
    /// <summary>   True to 2 contact. </summary>
    private bool p2Contact = false;
    /// <summary>   True if rotatable. </summary>
    private bool rotatable = false;
    /// <summary>   The speed. </summary>
    public float speed =  10;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {

    }

    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {
        if (p1Contact && p2Contact)
        {
            rotatable = true;
        }
        else
        {
            rotatable = false;
        }

    }

    /// <summary>   Executes the collision enter action. </summary>
    ///
 
    ///
    /// <param name="collision">    The collision. </param>

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("In cols");
        Debug.Log(collision.gameObject.tag);
        Debug.Log(collision.collider.tag);
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("P1 hit");
            p1Contact = true;
        }
        if (collision.collider.tag.Equals("Player2"))
        {
            p2Contact = true;
            Debug.Log("P2 hit");
        }

    }

    /// <summary>   Executes the collision exit action. </summary>
    ///
 
    ///
    /// <param name="collisionInfo">    Information describing the collision. </param>

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag.Equals("Player"))
        {
            rotatable = false;
            p1Contact = false;
            Debug.Log("P1 out");
        }
        if (collisionInfo.collider.tag.Equals("Player2"))
        {
            rotatable = false;
            p2Contact = false;
            Debug.Log("P2 out");
        }
    }

    /// <summary>   Executes the collision stay action. </summary>
    ///
 
    ///
    /// <param name="collisionInfo">    Information describing the collision. </param>

    void OnCollisionStay(Collision collisionInfo)
    {
        if (rotatable)
        {
            Vector3 pos = transform.position;

            Debug.Log("Rotato");
            if (collisionInfo.relativeVelocity.x > 0)
            {
transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
                transform.position = pos;
            }
        }
    }
}
