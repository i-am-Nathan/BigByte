﻿using UnityEngine;
using System.Collections;


/// <summary>
/// This script controls the pressure plate and falling wall's trigger animations
/// </summary>
public class PressurePlate : MonoBehaviour {

    private int _thingsOnTop = 0;
    private bool _pressed = false;

    /// <summary>
    /// Called when an object enters on top of the plate
    /// </summary>
    void OnTriggerEnter(Collider other) {
        
        // the crate has a weight of 2
        if (other.tag.Equals("Crate"))
        {
            _thingsOnTop += 2;

        } else
        {
            //players have a weight of 1
            _thingsOnTop++;
        }

        //if the weight is heavy enough, then the plate is triggered
        if (_thingsOnTop >= 2 && !_pressed)

        {
            this.GetComponent<Animation>().Play("PressurePlateDown");
            GameObject wall = GameObject.FindWithTag("Falling Wall");
            wall.GetComponent<Animation>().Play("FallingWallFall");
            _pressed = true;

        }
        
    }


    /// <summary>
    /// Called when an object leaves the plate
    /// </summary>
    void OnTriggerExit(Collider other) {
        //same as the method above, but for the upward motion.
        if (other.tag.Equals("Crate"))
        {
            _thingsOnTop -= 2;

        }
        else
        {
            _thingsOnTop--;
        }
        if (_thingsOnTop < 2 && _pressed)

        {
            this.GetComponent<Animation>().Play("PressurePlateUp");
            GameObject wall = GameObject.FindWithTag("Falling Wall");
            wall.GetComponent<Animation>().Play("FallingWallRaise");
            _pressed = false;

        }


    }
}
