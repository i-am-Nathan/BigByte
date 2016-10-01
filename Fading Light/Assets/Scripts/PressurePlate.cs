﻿using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class PressurePlate : MonoBehaviour {

    private int thingsOnTop = 0;

    void OnTriggerEnter(Collider other) {
        
        thingsOnTop++;
        if (thingsOnTop == 2)

        {
            this.GetComponent<Animation>().Play("PressurePlateDown");
            GameObject wall = GameObject.FindWithTag("Falling Wall");
            wall.GetComponent<FallingWall>().pressurePlate1 = true;
           
        }
        
    }



    void OnTriggerExit(Collider other) {

        thingsOnTop--;
        if (thingsOnTop == 0)
        {
            this.GetComponent<Animation>().Play("PressurePlateUp");
            GameObject wall = GameObject.FindWithTag("Falling Wall");
            wall.GetComponent<FallingWall>().pressurePlate1 = false;
            
        }

    }
}
