// file:	Assets\Scripts\TrapScripts\SawBladePlate.cs
//
// summary:	Implements the saw blade plate class

using UnityEngine;
using System.Collections;

/// <summary>   A saw blade plate. </summary>
///
/// <remarks>    . </remarks>

public class SawBladePlate : MonoBehaviour {

    /// <summary>   The game objects. </summary>
    public GameObject[] gameObjects;
    /// <summary>   True if pressed. </summary>
    private bool _pressed = false;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start () {
	    foreach (GameObject obj in gameObjects)
        {
            if (obj.tag.Equals("SawTrap"))
            {
                obj.GetComponent<Animation>().Stop();
            }
        }
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
	
	}

    /// <summary>   Called when an object enters on top of the plate. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {
        if (_pressed)
        {
            return;
        }
        if (other.name == "Player 1" || other.name == "Player2")
        {
            _pressed = true;
            this.GetComponent<Animation>().Play("PressurePlateDown");
            foreach (GameObject obj in gameObjects)
            {
                if (obj.tag.Equals("SawTrap"))
                {
                    obj.GetComponent<Animation>().Play("Anim_SawTrap02_Play");
                }
            }
        }
    }
}
