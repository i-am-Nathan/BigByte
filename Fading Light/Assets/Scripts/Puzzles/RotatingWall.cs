using UnityEngine;
using System.Collections;

public class RotatingWall : MonoBehaviour
{


    public GameObject SlidingDoor;
    private bool DEBUG = true;
    private int _people = 0;


    // Use this for initialization
    void Start()
    {
        if (DEBUG) Debug.Log("Inside Rotating Wall");


    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag.Equals("Player"))
        {
            _people++;
        }
        if (collider.tag.Equals("Player2"))
        {


            _people++;
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Debug.Log("P1Left");
            _people--;
        }
        if (collider.tag.Equals("Player2"))
        {
            Debug.Log("P2Left");
            _people--;
        }
        transform.Rotate(0, 0, 0, 0);
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log(_people);
        if (_people == 2)
        {
            Debug.Log("Pushing");
            if (transform.rotation.y < 0)
            {
                Debug.Log("Sound to indicate rotation complete and open door sound");
                Destroy(SlidingDoor);
            }
            else
            {
                transform.Rotate(new Vector3(0, 1, 0), Space.Self);
            }


        }
        else
        {
            transform.Rotate(0, 0, 0, 0);
        }
    }
}


