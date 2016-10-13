using UnityEngine;
using System.Collections;

public class LevelTwoTreasure : MonoBehaviour {

    public GameObject[] spikeTrap;
    public GameObject[] spearTrap;

    // Use this for initialization
    void Start () {
        foreach (GameObject obj in spikeTrap)
        {
            obj.GetComponent<Animation>().Play("Anim_TrapNeedle_Hide");
        }
        foreach (GameObject obj2 in spearTrap)
        {
            obj2.GetComponent<Animation>().Stop();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
