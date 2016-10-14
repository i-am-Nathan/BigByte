using UnityEngine;
using System.Collections;

public class FloatingPressurePlate : MonoBehaviour {


    private int _thingsOnTop = 0;
    private bool _pressed = false;
    public GameObject floater;


    void OnTriggerEnter(Collider other)
    {
        
        _thingsOnTop++;

        //if the weight is heavy enough, then the plate is triggered
        if (_thingsOnTop >= 1 && !_pressed)

        {
            this.GetComponent<Animation>().Play("PressurePlateDown");
            floater.gameObject.GetComponent <FloatingPlate> ().Resume();
            _pressed = true;

        }

    }

    void OnTriggerExit(Collider other)
    {

        _thingsOnTop--;

        //if the weight is heavy enough, then the plate is triggered
        if (_thingsOnTop < 1 && _pressed)

        {
            this.GetComponent<Animation>().Play("PressurePlateUp");
            floater.gameObject.GetComponent<FloatingPlate>().Stop();
            _pressed = false;

        }

    }

}
