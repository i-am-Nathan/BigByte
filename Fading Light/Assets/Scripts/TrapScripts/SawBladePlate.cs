using UnityEngine;
using System.Collections;

public class SawBladePlate : MonoBehaviour {

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
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Called when an object enters on top of the plate
    /// </summary>
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
