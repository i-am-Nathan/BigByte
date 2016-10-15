using UnityEngine;
using System.Collections;

public class FloatingPlate : MonoBehaviour {


	public float TravelSpeed;
    private bool _goingUp = true;
    private float _velocity = 50;
	private int _direction =1 ;
	private bool _pressedDown = false;


	void Start(){
		_direction = 1;
	}
    void OnTriggerEnter(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") 
        {

            other.transform.parent = transform;

		} else if (other.name == "Start" || other.name== "End")
        {
			Debug.Log ("In HERE");
			if (_direction == 1) {
				_direction = -1;
			} else {
				_direction = 1;
			}

		} 
    }

    public void Resume()
    {
		_pressedDown = true;
    }

    public void Stop()
    {
		_pressedDown = false;
    }

	void FixedUpdate(){
		if (_pressedDown) {
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
