using UnityEngine;
using System.Collections;

public class CandleLight : MonoBehaviour {
    public bool _active = false;
    private bool _triggered = false;
    public GameObject _flame;
    public GameObject _spotlight;

    // Use this for initialization
    void Start () {
        _flame.GetComponent<ParticleSystem>().Stop();
        _spotlight.GetComponent<Light>().spotAngle = 0;
    }
	
	// Update is called once per frame
	void Update () {

    }

    //check collision with a player holding the torch
    // When collision occurs between two objects
    void OnTriggerStay(Collider other)
    {
        //get the torch fuel controller to get the check to see if player one has the torch or not
        GameObject torchFuelController = GameObject.FindWithTag("TorchFuelController");

        bool torchInPlayer1 = torchFuelController.GetComponent<TorchFuelController>().TorchInPlayer1;
        Debug.Log(torchInPlayer1);
        //if the person is player 1 and has the torch
        Debug.Log(other.gameObject.tag.Equals("Player1"));
        if (torchInPlayer1 && other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("p1");
            //check for key pressed
            if (Input.GetButtonDown("CandleLight") && !_triggered)
            {
                _triggered = true;
                Debug.Log(_active + " active");
                if (_active && _triggered)
                {
                    Debug.Log("bbbbbbbbb");
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                }
                else if (!_active && _triggered)
                {
                    Debug.Log("eee");
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;

                }
            }
            else if (Input.GetButtonUp("CandleLight"))
            {
                _triggered = false;
            }

            //torch in player 2
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            //check for key pressed
            if (Input.GetButtonDown("CandleLight"))
            {
                _triggered = true;
                Debug.Log(_active + " active");
                if (_active && _triggered)
                {
                    Debug.Log("bbbbbbbbb");
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                    _triggered = false;
                }
                else if (!_active && _triggered)
                {
                    Debug.Log("eee");
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;
                    _triggered = false;
                }

            }
            else if (Input.GetButtonUp("CandleLight"))
            {
                _triggered = false;
            }
            //torch in player 1 and player 2 selecting
        }
        else if (torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            if (Input.GetButtonDown("CandleLight") && !_triggered)
            {
                _triggered = true;
                if (!_active && _triggered)
                {
                    Debug.Log("eee");
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;
                    _triggered = false;
                }
            }
            //torch in player 2 and player is player 1
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player1"))
        {
            if (Input.GetButtonDown("CandleLight") && !_triggered)
            {
                _triggered = true;
                if (!_active && _triggered)
                {
                    Debug.Log("eee");
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;
                    _triggered = false;
                }
            }
        }
    }
}
