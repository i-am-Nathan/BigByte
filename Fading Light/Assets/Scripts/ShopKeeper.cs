using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour {

	public Camera MainCamera;
	public Camera ShopKeeperCamera;
	private bool _shopping;
	private Animation _transition;
	public PlayerController Player1;
	public Player2Controller Player2;
	void Awake(){
		ShopKeeperCamera.enabled = false;
		_transition = ShopKeeperCamera.GetComponent<Animation> ();

	}
		
	void OnTriggerEnter(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T) && !_shopping) {
				Player1.IsDisabled = true;
				Player2.IsDisabled = true;
				MainCamera.enabled = false;
				ShopKeeperCamera.enabled = true;
				_transition.Play ();
				_shopping = true;


			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				_shopping = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
			}
		}
	}

	/// <summary>
	/// When players press T when they are collding with the chest, it will open it.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T) && !_shopping) {
				Player1.IsDisabled = true;
				Player2.IsDisabled = true;
				MainCamera.enabled = false;
				ShopKeeperCamera.enabled = true;
				_transition.Play ();
				_shopping = true;


			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				_shopping = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
			}
		}
	}
}