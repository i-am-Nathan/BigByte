using UnityEngine;
using System.Collections;

public class RotatableWall : MonoBehaviour
{
    private bool p1Contact = false;
    private bool p2Contact = false;
    private bool rotatable = false;
    public float speed =  10;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (p1Contact && p2Contact)
        {
            rotatable = true;
        }
        else
        {
            rotatable = false;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("In cols");
        Debug.Log(collision.gameObject.tag);
        Debug.Log(collision.collider.tag);
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("P1 hit");
            p1Contact = true;
        }
        if (collision.collider.tag.Equals("Player2"))
        {
            p2Contact = true;
            Debug.Log("P2 hit");
        }

    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag.Equals("Player"))
        {
            rotatable = false;
            p1Contact = false;
            Debug.Log("P1 out");
        }
        if (collisionInfo.collider.tag.Equals("Player2"))
        {
            rotatable = false;
            p2Contact = false;
            Debug.Log("P2 out");
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (rotatable)
        {
            Vector3 pos = transform.position;

            Debug.Log("Rotato");
            if (collisionInfo.relativeVelocity.x > 0)
            {
transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
                transform.position = pos;
            }
        }
    }
}
