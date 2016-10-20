// file:	Assets\Scripts\TrapScripts\LevelTwoMiddleTrap.cs
//
// summary:	Implements the level two middle trap class

using UnityEngine;
using System.Collections;

/// <summary>   A level two middle trap. </summary>
///
/// <remarks>    . </remarks>

public class LevelTwoMiddleTrap : MonoBehaviour {
    /// <summary>   The saw trap. </summary>
    public GameObject[] sawTrap;
    /// <summary>   The axe trap. </summary>
    public GameObject[] axeTrap;
    /// <summary>   The door. </summary>
    public GameObject door;
    /// <summary>   The lever sound. </summary>
    public AudioSource LeverSound;

    /// <summary>   True if pulled. </summary>
    private bool _pulled = false;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start () {
        foreach (GameObject obj in sawTrap)
        {
            obj.GetComponent<Animation>().Stop();
        }
        foreach (GameObject obj in axeTrap)
        {
            obj.GetComponent<Animation>().Stop();
        }
    }
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
	
	}

    /// <summary>   Called when the player is close enough to the lever, and presses T. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        //if O is pressed to interact with the lever, the walls move
		if (Input.GetKeyDown(KeyCode.O) && !_pulled && other.gameObject.tag.Equals("Player"))
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
            LeverSound.Play();
            foreach (GameObject obj in sawTrap)
            {
                obj.GetComponent<Animation>().Play();
            }
            foreach (GameObject obj in axeTrap)
            {
                obj.GetComponent<Animation>().Play();
            }
            Destroy(door);
            _pulled = true;
        }
		//if Q is pressed to interact with the lever, the walls move
		if (Input.GetKeyDown(KeyCode.Q) && !_pulled && other.gameObject.tag.Equals("Player2"))
		{
			this.GetComponent<Animation>().Play("Armature|LeverDown");
            LeverSound.Play();
			foreach (GameObject obj in sawTrap)
			{
				obj.GetComponent<Animation>().Play();
			}
			foreach (GameObject obj in axeTrap)
			{
				obj.GetComponent<Animation>().Play();
			}
			Destroy(door);
			_pulled = true;
		}
    }
}
