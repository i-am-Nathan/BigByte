// file:	Assets\Scripts\Puzzles\RotatingWall.cs
//
// summary:	Implements the rotating wall class

using UnityEngine;
using System.Collections;

/// <summary>   A rotating wall. </summary>
///
/// <remarks>    . </remarks>

public class RotatingWall : MonoBehaviour
{


    /// <summary>   The sliding door. </summary>
    public GameObject SlidingDoor;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = true;
    /// <summary>   The people. </summary>
    private int _people = 0;
    /// <summary>   True to rotating. </summary>
    private bool rotating = true;


    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        if (DEBUG) Debug.Log("Inside Rotating Wall");


    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="collider"> The collider. </param>

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag.Equals("Player"))
        {
            _people++;
        }
        if (collider.tag.Equals("Player2"))
        {


            _people++;
        }

    }

    /// <summary>   Executes the trigger exit action. </summary>
    ///
 
    ///
    /// <param name="collider"> The collider. </param>

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Debug.Log("P1Left");
            _people--;
        }
        if (collider.tag.Equals("Player2"))
        {
            Debug.Log("P2Left");
            _people--;
        }
        transform.Rotate(0, 0, 0, 0);
    }

    /// <summary>   Executes the trigger stay action. </summary>
    ///
 
    ///
    /// <param name="collider"> The collider. </param>

    void OnTriggerStay(Collider collider)
    {
        Debug.Log(_people);
        if (_people == 1)
        {
   
            if (rotating)
            {
                Vector3 to = new Vector3(0, 180, 0);
                if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
                {
                    transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
                }
                else
                {
                    transform.eulerAngles = to;
                    //Destroy(SlidingDoor);
                    Debug.Log("done");
                    rotating = false;
                }
            }


        }
        else
        {
            transform.Rotate(0, 0, 0, 0);
        }
    }
}


