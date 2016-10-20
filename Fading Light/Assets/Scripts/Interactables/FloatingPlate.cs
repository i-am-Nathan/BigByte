using UnityEngine;
using System.Collections;

/// <summary>
/// This is used for floating platforms which move back and forth from a start and end point.
/// Players can get on this platform to navigate to another area.
/// </summary>
public class FloatingPlate : MonoBehaviour {


	public float TravelSpeed;
    private bool _goingUp = true;
    private float _velocity = 50;
	public Collider StartBoundary;
	public Collider EndBoundary;
	private int _direction =1 ;
	public bool Pressed;
	private bool locked;

	/// <summary>
	/// Sets an initial direction.
	/// </summary>
	void Start(){
		_direction = 1;
	}

	/// <summary>
	/// Transform the player's parents to the plates to ensure taht it is in it's worldspace and prevents it from
	/// sliding off. Will also check when it hits a boundary and if so, it will go in the opposite direction.
	/// </summary>
	/// <param name="other">Other.</param>
    void OnTriggerEnter(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") 
        {
			if (other.transform.parent.name != "Floating") {
				
				other.transform.parent = transform;
			}

		} 
			if (other == StartBoundary || other == EndBoundary)
        {
			if (_direction == 1) {
				_direction = -1;
			} else {
				_direction = 1;
			}

		} 
    }

	/// <summary>
	/// Ensures that the player will stay on the platform.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") 
		{

			other.transform.parent = transform;

		}
	}

	/// <summary>
	/// This will be used when players step on a plate that corresponds to the movement of the current 
	/// floating platform.
	/// </summary>
    public void Resume()
    {
		Pressed = true;
    }

	/// <summary>
	/// This method will be called when players who were originally stepping on a pressure plate, gets off it
	/// and pauses the movement of the platform.
	/// </summary>
    public void Stop()
    {
		Pressed = false;
    }

	/// <summary>
	/// This will cause the platform to keep going back and forth.
	/// </summary>
	void FixedUpdate(){
		if (Pressed) {
			transform.Translate (new Vector3(1,0,0)* TravelSpeed * _direction * Time.deltaTime);
		}
	}

	/// <summary>
	/// When player's leave the platform, it will ensure that it reassigns the parent thus leaving the
	/// world space of the platform.
	/// </summary>
	/// <param name="other">Other.</param>
    void OnTriggerExit(Collider other)
    {
		if (other.name == "Player 1" || other.name == "Player2") {
			if (other.transform.parent == transform) {
				other.transform.parent = null;
			}

        }
    }
}
