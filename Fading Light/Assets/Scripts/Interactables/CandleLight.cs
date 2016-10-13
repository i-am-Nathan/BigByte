using UnityEngine;
using System.Collections;

public class CandleLight : MonoBehaviour {
    public bool _active = false;
    private bool _triggered = false;

    public float Radius = 20f;
    public GameObject _flame;
    public GameObject _spotlight;

    // Use this for initialization
    void Start () {
		if (!_active) {
			_flame.GetComponent<ParticleSystem> ().Stop ();
            _spotlight.GetComponent<Light>().spotAngle = 0;
        } else
        {
            _flame.GetComponent<ParticleSystem>().Play();
            _spotlight.GetComponent<Light>().spotAngle = 100;
        }
    }
	
    public bool isTriggered()
    {
        return _triggered;
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
        //if the person is player 1 and has the torch
        if (torchInPlayer1 && other.gameObject.tag.Equals("Player"))
        {
            //check for key pressed
            if (Input.GetKeyDown(KeyCode.T) && !_triggered)
            {
                _triggered = true;
                if (_active)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                }
                else
                {
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;

                }
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                _triggered = false;
            }

            //torch in player 2
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            //check for key pressed
            if (Input.GetKeyDown(KeyCode.T) && !_triggered)
            {
                _triggered = true;
                if (_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                }
                else if (!_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Play();
                    _spotlight.GetComponent<Light>().spotAngle = 100;
                    _active = true;
                }

            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                _triggered = false;
            }
            //torch in player 1 and player 2 selecting
        }
        else if (torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            if (Input.GetKeyDown(KeyCode.T) && !_triggered)
            {
                _triggered = true;
                if (_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                    _triggered = false;
                }
            }
            //torch in player 2 and player is player 1
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player"))
        {
            if (Input.GetKeyDown(KeyCode.T) && !_triggered)
            {
                _triggered = true;
                if (_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                    _triggered = false;
                }
            }
        }
    }
}
