using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class PressurePlate : MonoBehaviour {

    private int _thingsOnTop = 0;
    private bool _pressed = false;

    void OnTriggerEnter(Collider other) {
        
        if (other.tag.Equals("Crate"))
        {
            _thingsOnTop += 2;

        } else
        {
            _thingsOnTop++;
        }
        if (_thingsOnTop >= 2 && !_pressed)

        {
            this.GetComponent<Animation>().Play("PressurePlateDown");
            GameObject wall = GameObject.FindWithTag("Falling Wall");
            wall.GetComponent<Animation>().Play("FallingWallFall");
            _pressed = true;

        }
        
    }



    void OnTriggerExit(Collider other) {

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
