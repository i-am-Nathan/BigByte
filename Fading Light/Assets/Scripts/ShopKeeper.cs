using UnityEngine;
using System.Collections;

public class ShopKeeper : MonoBehaviour {

	public Camera MainCamera;
	public Camera ShopKeeperCamera;
	private bool _shopping;
	void Awake(){
		ShopKeeperCamera.enabled = false;
	}
		
	void OnTriggerEnter(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T) && !_shopping) {
				MainCamera.enabled = false;
				ShopKeeperCamera.enabled = true;
				ShopKeeperCamera.GetComponent<Animation> ().Play ();
				_shopping = true;
			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				_shopping = false;
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
				MainCamera.enabled = false;
				ShopKeeperCamera.enabled = true;
				ShopKeeperCamera.GetComponent<Animation> ().Play ();
				_shopping = true;
			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				_shopping = false;
			}
		}

}
}