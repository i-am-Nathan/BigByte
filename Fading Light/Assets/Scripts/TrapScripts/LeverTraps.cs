using UnityEngine;
using System.Collections;

public class LeverTraps : MonoBehaviour {
    public GameObject[] gameObjects;
    private bool _pressed = false;

    // Use this for initialization
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

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
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
