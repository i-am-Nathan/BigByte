using UnityEngine;
using System.Collections;

/// <summary>
/// This class is based on the puzzle which players will encounter during their gameplay. players will be able
/// to control a platform where one player focuses solely on rotating and other person focuses on moving forward
/// and backwards.
/// </summary>
public class ControlledPlatform : MonoBehaviour {

	public Collider NorthBoundary;
	public Collider EastBoundary;
	public Collider SouthBoundary;
	public Collider WestBoundary;
	public float TravelSpeed;
	private bool _northB;
	private bool _eastB;
	private bool _southB;
	private bool _westB;
	private bool _p1RotationMounted;
	private bool _p2TranslationMounted;
	private bool _p1OnPlate;
	private bool _p2OnPlate;

	/// <summary>
	/// Sets my varaibles
	/// </summary>
	void Start(){
		_p1RotationMounted=false;
		_p2TranslationMounted=false;
		_p1OnPlate = false;
		_p2OnPlate = false;
	}

	/// <summary>
	/// This will cause the player to be stuck to the moving platform and also set the boolean, acknowleding they are on the plate.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
			if (other.name == "Player 1") {
				other.transform.parent = transform;
			_p1OnPlate = true;
		} else if (other.name == "Player2") {
				other.transform.parent = transform;
			_p2OnPlate = true;
			}
	}

	/// <summary>
	/// Moves the plate up
	/// </summary>
	public void MoveUp()
	{
			transform.Translate (new Vector3(-1,0,0)* TravelSpeed *  Time.deltaTime);
	}

	/// <summary>
	/// Moves the plate down
	/// </summary>
	public void MoveDown()
	{
			transform.Translate (new Vector3(1,0,0)* TravelSpeed * Time.deltaTime);
	}


	/// <summary>
	/// Moves the plate left
	/// </summary>
	public void MoveLeft()
	{
			transform.Translate (new Vector3(0,0,-1)* TravelSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Moves the plate right.
	/// </summary>
	public void MoveRight()
	{
			transform.Translate (new Vector3(0,0,1)* TravelSpeed * Time.deltaTime);
	}
		
	/// <summary>
	/// This will update the player and acknowledge the keys from the players.
	/// </summary>
	void Update(){
		//Player 1

		//This will lock the player on the platform or unlock them depending on their current state. Used for player 1 who rotates the platform.
		if (Input.GetKeyDown (KeyCode.O)) {
			if (_p1OnPlate) {
				if (!_p1RotationMounted) {
					_p1RotationMounted = true;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().IsDisabled = true;
				} else if (_p1RotationMounted) {
					_p1RotationMounted = false;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().IsDisabled = false;
				} 
			}
		}

		//This will lock the player on the platform or unlock them depending on their current state. Used for plater 2 who translates the block.
		if (Input.GetKeyDown (KeyCode.Q)) {
			if (_p2OnPlate) {
				if (!_p2TranslationMounted) {
					_p2TranslationMounted = true;
					GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player2Controller> ().IsDisabled = true;
				} else {
					_p2TranslationMounted = false;
					GameObject.FindGameObjectWithTag ("Player2").GetComponent<Player2Controller> ().IsDisabled = false;
				}
			}
		}

		//Rotates the block
		if (Input.GetKey (KeyCode.LeftArrow) && _p1RotationMounted && _p2TranslationMounted) {
				transform.Rotate (new Vector3 (0, -1, 0) * 3);

		}else if (Input.GetKey (KeyCode.RightArrow) && _p1RotationMounted &&_p2TranslationMounted) {
				transform.Rotate (new Vector3(0,1,0) * 3);
		}

		//Translates the block
		if (Input.GetKey(KeyCode.W) && _p2TranslationMounted  && _p1RotationMounted){
			MoveUp ();
		}else if (Input.GetKey(KeyCode.S) && _p2TranslationMounted  && _p1RotationMounted){
			MoveDown ();
		}
	}

	/// <summary>
	/// This will check if the player has exited the block and set the appropriate boolean.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit(Collider other)
	{
		if (other.name == "Player 1") {
			other.transform.parent = null;
			_p1OnPlate = false;
		} 
		if (other.name == "Player2") {
			other.transform.parent = null;
			_p2OnPlate = false;
		}
	}
}
