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
        if (collision.gameObject.tag.Equals("Player"))
        {
            p1Contact = true;
        }
        if (collision.gameObject.tag.Equals("Player 2"))
        {
            p2Contact = true;
        }

    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Player"))
        {
            rotatable = false;
            p1Contact = false;
        }
        if (collisionInfo.gameObject.tag.Equals("Player 2"))
        {
            rotatable = false;
            p2Contact = false;
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (rotatable)
        {
            if (collisionInfo.relativeVelocity.x > 0)
            {
                transform.Rotate(Vector3.up, speed * Time.deltaTime);
            }
        }
    }
}
