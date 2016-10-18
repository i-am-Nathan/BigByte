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
	public bool P1Mounted;
	public bool P2Mounted;

	void Start(){
		P1Mounted = false;
		P2Mounted = false;
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
		if (other == NorthBoundary)
		{
			_northB = true;
		} 
		if (other == EastBoundary)
		{
			_eastB = true;
		} 
		if (other == SouthBoundary)
		{
			_southB = true;
		} 
		if (other == WestBoundary)
		{
			_westB = true;
		} 
	}
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {

			other.transform.parent = transform;
			if (Input.GetKeyDown (KeyCode.T)) {
				if (other.name == "Player 1") {

					if (!P1Mounted && !P2Mounted) {
						P1Mounted = true;
						other.GetComponent<PlayerController> ().IsDisabled = true;
					} else {
						P1Mounted = false;
						P2Mounted = false;
						other.GetComponent<PlayerController> ().IsDisabled = false;
					}
				} else {
						if (!P1Mounted && !P2Mounted) {
							P2Mounted = true;
							other.GetComponent<Player2Controller> ().IsDisabled = true;
						}else {
							P1Mounted = false;
							P2Mounted = false;
							other.GetComponent<Player2Controller> ().IsDisabled = false;
						}
					
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
		if (Input.GetKey (KeyCode.LeftArrow) && P1Mounted) {
			transform.Rotate (new Vector3(0,-1,0) * 3);
		}else if (Input.GetKey (KeyCode.RightArrow) && P1Mounted) {
			transform.Rotate (new Vector3(0,1,0) * 3);
		}else if (Input.GetKey (KeyCode.A) && P2Mounted) {
			transform.Rotate (new Vector3(0,-1,0) * 3);
		}else if (Input.GetKey (KeyCode.D) && P2Mounted) {
			transform.Rotate (new Vector3(0,1,0) * 3);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.name == "Player 1" || other.name == "Player2") {
			other.transform.parent = null;
			P1Mounted = false;
			P2Mounted = false;

		}
		if (other == NorthBoundary)
		{
			_northB = false;
		} 
		if (other == EastBoundary)
		{
			_eastB = false;
		} 
		if (other == SouthBoundary)
		{
			_southB = false;
		} 
		if (other == WestBoundary)
		{
			_westB = false;
		} 
	}
}
