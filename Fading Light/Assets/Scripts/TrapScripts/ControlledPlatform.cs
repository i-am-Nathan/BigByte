using UnityEngine;
using System.Collections;

public class ControlledPlatform : MonoBehaviour {


	private float _platformSpeed;
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

	void Start(){
		_p1RotationMounted=false;
		_p2TranslationMounted=false;
		_p1OnPlate = false;
		_p2OnPlate = false;
	}
	/// <summary>
	/// This will cause the player to be stuck to the moving platform and also calculate the approrpiate logic when it hits a boundary.
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

	public void MoveUp()
	{
			transform.Translate (new Vector3(-1,0,0)* TravelSpeed *  Time.deltaTime);
	}
	public void MoveDown()
	{
			transform.Translate (new Vector3(1,0,0)* TravelSpeed * Time.deltaTime);
	}

	public void MoveLeft()
	{
			transform.Translate (new Vector3(0,0,-1)* TravelSpeed * Time.deltaTime);
	}

	public void MoveRight()
	{
			transform.Translate (new Vector3(0,0,1)* TravelSpeed * Time.deltaTime);
	}
		
	void Update(){
		//Player 1

		if (Input.GetKeyDown (KeyCode.O)) {
			if (_p1OnPlate) {
				Debug.Log ("IN HERE GG");
				if (!_p1RotationMounted) {
					_p1RotationMounted = true;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().IsDisabled = true;
				} else if (_p1RotationMounted) {
					_p1RotationMounted = false;
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().IsDisabled = false;
				} 
			}
		}


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
		if (Input.GetKey (KeyCode.LeftArrow) && _p1RotationMounted && _p2TranslationMounted) {
				transform.Rotate (new Vector3 (0, -1, 0) * 3);

		}else if (Input.GetKey (KeyCode.RightArrow) && _p1RotationMounted &&_p2TranslationMounted) {
				transform.Rotate (new Vector3(0,1,0) * 3);
		}

		//Player 2
		if (Input.GetKey(KeyCode.W) && _p2TranslationMounted  && _p1RotationMounted){
			MoveUp ();
		}else if (Input.GetKey(KeyCode.S) && _p2TranslationMounted  && _p1RotationMounted){
			MoveDown ();
		}
	}
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
