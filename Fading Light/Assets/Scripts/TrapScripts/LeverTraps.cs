// file:	Assets\Scripts\TrapScripts\LeverTraps.cs
//
// summary:	Implements the lever traps class

using UnityEngine;
using System.Collections;

/// <summary>   A lever traps. </summary>
///
/// <remarks>    . </remarks>

public class LeverTraps : MonoBehaviour {
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
            if (obj.tag.Equals("AxeTrap"))
            {
                obj.GetComponent<Animation>().Stop();
            }
        }
    }

    /// <summary>   Called when the player is close enough to the lever, and presses T. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        //if T is pressed to interact with the lever, the walls move
		if ((other.name.Equals("Player 1") && Input.GetKeyDown(KeyCode.O)) || (other.name.Equals("Player2") && Input.GetKeyDown(KeyCode.Q)))
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.tag.Equals("SawTrap"))
                {
                    obj.GetComponent<Animation>().Play("Anim_SawTrap02_Play");
                } else if (obj.tag.Equals("AxeTrap"))
                {
                    obj.GetComponent<Animation>().Play("Anim_AxeTrap_Play");
                }
            }
        }
    }
}
