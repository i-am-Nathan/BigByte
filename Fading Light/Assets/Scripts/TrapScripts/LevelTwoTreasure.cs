﻿using UnityEngine;
using System.Collections;

public class LevelTwoTreasure : MonoBehaviour
{

    public GameObject[] hammerTrap;
    public GameObject[] spearTrap;
    private bool _pulled = false;

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when the player is close enough to the lever, and presses T
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        //if T is pressed to interact with the lever, the walls move
        if (Input.GetKeyDown(KeyCode.T) && !_pulled)
        {
            this.GetComponent<Animation>().Play("Armature|LeverDown");
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