using UnityEngine;
using System.Collections;

public class RotatingWall : MonoBehaviour
{


    public GameObject SlidingDoor;
    private bool DEBUG = true;
    private int _people = 0;
    private bool rotating = true;


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
        if (_people == 1)
        {
   
            if (rotating)
            {
                Vector3 to = new Vector3(0, 180, 0);
                if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
                {
                    transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
                }
                else
                {
                    transform.eulerAngles = to;
                    //Destroy(SlidingDoor);
                    Debug.Log("done");
                    rotating = false;
                }
            }


        }
        else
        {
            transform.Rotate(0, 0, 0, 0);
        }
    }
}


