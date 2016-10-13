using UnityEngine;
using System.Collections;

public class RotatingDoor1 : MonoBehaviour {

    public float forceAmount = 1000f;
    void OnMouseDown()
    {
        Debug.Log("yolo6");
        GetComponent<Rigidbody>().AddForce(-transform.forward * forceAmount, ForceMode.Acceleration);
    }
}
