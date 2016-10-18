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
	private bool _p1TranslationMounted;
	private bool _p2RotationMounted;
	private bool _p2TranslationMounted;

	void Start(){
		_p1RotationMounted=false;
		_p1TranslationMounted=false;
		_p2RotationMounted=false;
		_p2TranslationMounted=false;
	}
	/// <summary>
	/// This will cause the player to be stuck to the moving platform and also calculate the approrpiate logic when it hits a boundary.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player 1" || other.name == "Player2") 
		{
			other.transform.parent = transform;

		} 
//		if (other == NorthBoundary)
//		{
//			_northB = true;
//		} 
//		if (other == EastBoundary)
//		{
//			_eastB = true;
//		} 
//		if (other == SouthBoundary)
//		{
//			_southB = true;
//		} 
//		if (other == WestBoundary)
//		{
//			_westB = true;
//		} 
	}
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1") {
			other.transform.parent = transform;
			if (Input.GetKeyDown (KeyCode.O)) {
				if (_p2RotationMounted && !_p1TranslationMounted && !_p1RotationMounted) {
					_p1TranslationMounted = true;
					other.GetComponent<PlayerController> ().IsDisabled = true;
				} else if (!_p2RotationMounted && !_p1RotationMounted && !_p1TranslationMounted) {
					_p1RotationMounted = true;
					other.GetComponent<PlayerController> ().IsDisabled = true;

				} else if (_p1RotationMounted) {
					_p1RotationMounted = false;
					other.GetComponent<PlayerController> ().IsDisabled = false;
				} else if (_p1TranslationMounted) {
					_p1TranslationMounted = false;
					other.GetComponent<PlayerController> ().IsDisabled = false;
				}
			}
		} else if (other.name == "Player2") {
			other.transform.parent = transform;
			if (Input.GetKeyDown (KeyCode.Q)) {
				if (_p1RotationMounted && !_p2TranslationMounted && !_p2RotationMounted) {
					_p2TranslationMounted = true;
					other.GetComponent<Player2Controller> ().IsDisabled = true;
				} else if (!_p1RotationMounted && !_p2RotationMounted && !_p2TranslationMounted) {
					_p2RotationMounted = true;
					other.GetComponent<Player2Controller> ().IsDisabled = true;

				} else if (_p2RotationMounted) {
					_p2RotationMounted = false;
					other.GetComponent<Player2Controller> ().IsDisabled = false;
				} else if (_p2TranslationMounted) {
					_p2TranslationMounted = false;
					other.GetComponent<Player2Controller> ().IsDisabled = false;
				}
			}
		}
			
	}

	public void MoveUp()
	{
		if (!_northB) {
			transform.Translate (new Vector3(-1,0,0)* TravelSpeed *  Time.deltaTime);
		}
	}
	public void MoveDown()
	{
		if (!_southB) {
			transform.Translate (new Vector3(1,0,0)* TravelSpeed * Time.deltaTime);
		}
	}

	public void MoveLeft()
	{
		if (!_westB) {
			transform.Translate (new Vector3(0,0,-1)* TravelSpeed * Time.deltaTime);
		}
	}

	public void MoveRight()
	{
		if (!_eastB) {
			transform.Translate (new Vector3(0,0,1)* TravelSpeed * Time.deltaTime);
		}
	}
		
	void Update(){
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (_p1RotationMounted) {
				transform.Rotate (new Vector3 (0, -1, 0) * 3);
			} else if (_p1TranslationMounted) {
				MoveLeft ();
			}
		}else if (Input.GetKey (KeyCode.RightArrow)) {
			if (_p1RotationMounted) {
				transform.Rotate (new Vector3(0,1,0) * 3);
			} else if (_p1TranslationMounted) {
				MoveRight ();
			}
		}else if (Input.GetKey(KeyCode.UpArrow) && _p1TranslationMounted){
			MoveUp ();
		}else if (Input.GetKey(KeyCode.DownArrow) && _p1TranslationMounted){
			MoveDown ();
		}else if (Input.GetKey (KeyCode.A)) {
			if (_p2RotationMounted) {
				transform.Rotate (new Vector3 (0, -1, 0) * 3);
			} else if (_p2TranslationMounted) {
				MoveLeft ();
			}
		}else if (Input.GetKey (KeyCode.D) && _p2RotationMounted) {
			if (_p2RotationMounted) {
				transform.Rotate (new Vector3(0,1,0) * 3);
			} else if (_p2TranslationMounted) {
				MoveLeft ();
			}

		}else if (Input.GetKey(KeyCode.W) && _p2TranslationMounted){
			MoveUp ();
		}else if (Input.GetKey(KeyCode.S) && _p2TranslationMounted){
			MoveDown ();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.name == "Player 1" || other.name == "Player2") {
			other.transform.parent = null;

		}
//		if (other == NorthBoundary)
//		{
//			_northB = false;
//		} 
//		if (other == EastBoundary)
//		{
//			_eastB = false;
//		} 
//		if (other == SouthBoundary)
//		{
//			_southB = false;
//		} 
//		if (other == WestBoundary)
//		{
//			_westB = false;
//		} 
	}
}
