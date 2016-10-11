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
            if (true)
            {

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
