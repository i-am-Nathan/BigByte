// file:	Assets\Scripts\TrapScripts\LevelTwoTreasure.cs
//
// summary:	Implements the level two treasure class

using UnityEngine;
using System.Collections;

/// <summary>   A level two treasure. </summary>
///
/// <remarks>    . </remarks>

public class LevelTwoTreasure : MonoBehaviour
{

    /// <summary>   The hammer trap. </summary>
    public GameObject[] hammerTrap;
    /// <summary>   The spear trap. </summary>
    public GameObject[] spearTrap;
    /// <summary>   True if pulled. </summary>
    private bool _pulled = false;

    /// <summary>   The lever sound. </summary>
    public AudioSource LeverSound;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        foreach (GameObject obj in hammerTrap)
        {
            obj.GetComponent<Animation>().Play();
        }
        foreach (GameObject obj2 in spearTrap)
        {
            obj2.GetComponent<Animation>().Play();
        }
    }

    /// <summary>   Called when the player is close enough to the lever, and presses T. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        //if Q is pressed to interact with the lever, the walls move
        if (Input.GetKeyDown(KeyCode.O) && !_pulled && other.gameObject.tag.Equals("Player"))
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            LeverSound.Play();
            foreach (GameObject obj in hammerTrap)
            {
                Destroy(obj);
            }
            foreach (GameObject obj2 in spearTrap)
            {

                Destroy(obj2);
            }
            _pulled = true;
        }
        //if O is pressed to interact with the lever, the walls move
        if (Input.GetKeyDown(KeyCode.Q) && !_pulled && other.gameObject.tag.Equals("Player2"))
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            LeverSound.Play();
            foreach (GameObject obj in hammerTrap)
            {
                Destroy(obj);
            }
            foreach (GameObject obj2 in spearTrap)
            {

                Destroy(obj2);
            }
            _pulled = true;
        }
    }
}
