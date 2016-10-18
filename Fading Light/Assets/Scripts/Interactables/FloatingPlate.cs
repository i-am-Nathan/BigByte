using UnityEngine;
using System.Collections;

public class FloatingPlate : MonoBehaviour {


	public float TravelSpeed;
    private bool _goingUp = true;
    private float _velocity = 50;
	public Collider StartBoundary;
	public Collider EndBoundary;
	private int _direction =1 ;
	public bool Pressed;


	void Start(){
		_direction = 1;
	}
    void OnTriggerEnter(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") 
        {

            other.transform.parent = transform;

		} else if (other == StartBoundary || other == EndBoundary)
        {
			if (_direction == 1) {
				_direction = -1;
			} else {
				_direction = 1;
			}

		} 
    }
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") 
		{

			other.transform.parent = transform;

		}
	}

    public void Resume()
    {
		Pressed = true;
    }

    public void Stop()
    {
		Pressed = false;
    }

	void FixedUpdate(){
		if (Pressed) {
			transform.Translate (new Vector3(1,0,0)* TravelSpeed * _direction * Time.deltaTime);
		}
	}

    void OnTriggerExit(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") {
        
            other.transform.parent = null;

        }
    }
}
