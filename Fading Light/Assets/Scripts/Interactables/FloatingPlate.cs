using UnityEngine;
using System.Collections;

public class FloatingPlate : MonoBehaviour {

    public GameObject _backBumper;
    public GameObject _frontBumper;
    private bool _goingUp = true;
    private float _velocity = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("player") || other.tag.Equals("player2"))
        {

            other.gameObject.transform.parent = this.transform;
            Debug.LogWarning(other.gameObject.transform.parent);

        } else if (other.gameObject.Equals(_backBumper))
        {
            _goingUp = true;
            GetComponent<Rigidbody>().velocity = new Vector3(_velocity * -1, 0f, 0f);

        } else if (other.gameObject.Equals(_frontBumper))
        {
            _goingUp = false;
            GetComponent<Rigidbody>().velocity = new Vector3(_velocity, 0f, 0f);
        }
    }

    public void Resume()
    {
        if (_goingUp)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(_velocity * -1, 0f, 0f);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(_velocity, 0f, 0f);
        }

        
    }

    public void Stop()
    {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
    }



    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("player") || other.tag.Equals("player2"))
        {
            other.gameObject.transform.parent = null;

        }
    }
}
