// file:	Assets\Scripts\Interactables\CandleLight.cs
//
// summary:	Implements the candle light class

using UnityEngine;
using System.Collections;

/// <summary>   A candle light. </summary>
///
/// <remarks>    . </remarks>

public class CandleLight : MonoBehaviour {
    /// <summary>   True to active. </summary>
    public bool _active = false;
    /// <summary>   True if triggered. </summary>
    private bool _triggered = false;

    /// <summary>   The radius. </summary>
    public float Radius = 20f;
    /// <summary>   The flame. </summary>
    public GameObject _flame;
    /// <summary>   The spotlight. </summary>
    public GameObject _spotlight;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

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

    /// <summary>   Query if this object is active. </summary>
    ///
 
    ///
    /// <returns>   True if active, false if not. </returns>

    public bool isActive()
    {
        return _active;
    }

	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {

    }

    //check collision with a player holding the torch
    // When collision occurs between two objects

    /// <summary>   Executes the trigger stay action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerStay(Collider other)
    {
        //get the torch fuel controller to get the check to see if player one has the torch or not
        GameObject torchFuelController = GameObject.FindWithTag("TorchFuelController");

        bool torchInPlayer1 = torchFuelController.GetComponent<TorchFuelController>().TorchInPlayer1;
        //if the person is player 1 and has the torch
        if (torchInPlayer1 && other.gameObject.tag.Equals("Player"))
        {
            //check for key pressed
            if (Input.GetKeyDown(KeyCode.O) && !_triggered)
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
            else if (Input.GetKeyDown(KeyCode.O))
            {
                _triggered = false;
            }

            //torch in player 2
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            //check for key pressed
            if (Input.GetKeyDown(KeyCode.Q) && !_triggered)
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
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _triggered = false;
            }
            //torch in player 1 and player 2 selecting
        }
        else if (torchInPlayer1 && other.gameObject.tag.Equals("Player2"))
        {
            if (Input.GetKeyDown(KeyCode.Q) && !_triggered)
            {
                _triggered = true;
                if (_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _triggered = false;
            }
            //torch in player 2 and player is player 1
        }
        else if (!torchInPlayer1 && other.gameObject.tag.Equals("Player"))
        {
            if (Input.GetKeyDown(KeyCode.O) && !_triggered)
            {
                _triggered = true;
                if (_active && _triggered)
                {
                    _flame.GetComponent<ParticleSystem>().Stop();
                    _spotlight.GetComponent<Light>().spotAngle = 0;
                    _active = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                _triggered = false;
            }
        }
    }
}
