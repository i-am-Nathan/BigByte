// file:	Assets\LevelTwoTrapPlate.cs
//
// summary:	Implements the level two trap plate class

using UnityEngine;
using System.Collections;

/// <summary>   A level two trap plate. </summary>
///
/// <remarks>    . </remarks>

public class LevelTwoTrapPlate : MonoBehaviour
{
    /// <summary>   The axe. </summary>
    public GameObject[] axe;
    /// <summary>   The saws. </summary>
    public GameObject[] saws;
    /// <summary>   True if pressed. </summary>
    private bool _pressed = false;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        foreach (GameObject obj in axe)
        {
            obj.GetComponent<Animation>().Stop();
        }
        foreach (GameObject obj2 in saws)
        {
            obj2.GetComponent<Animation>().Stop();
        }
    }

    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {

    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {
        // the crate has a weight of 2
        if (other.name == "Player 1" || other.name == "Player2")
        {

            if (!_pressed)
            {
                foreach (GameObject obj in axe)
                {
                    obj.GetComponent<Animation>().Play();
                }
                foreach (GameObject obj2 in saws)
                {
                    obj2.GetComponent<Animation>().Play();
                }
                _pressed = true;
            }
        }
    }
}
