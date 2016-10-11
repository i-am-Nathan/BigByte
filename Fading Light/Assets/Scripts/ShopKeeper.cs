using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class is used for the shop. Players can interact with the shopkeeper and purchase items using the gold they have 
/// accumulated during gameplay.
/// </summary>
public class ShopKeeper : MonoBehaviour {
	public Camera MainCamera;
	public Camera ShopKeeperCamera;
	private bool _shopping;
	private Animation _transition;
	public PlayerController Player1;
	public Player2Controller Player2;
	public GameObject ItemStand;
	private bool _hasPlayed;
    Animator _animator;

    void Awake(){
		ShopKeeperCamera.enabled = false;
		_transition = ShopKeeperCamera.GetComponent<Animation> ();
        _animator = GetComponentInChildren<Animator>();//need this...
    }
		
	void Start(){
		_hasPlayed = false;
		ItemStand.SetActive (false);
        _animator.SetFloat("speed", -1f);
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
				_hasPlayed = true;

			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				_hasPlayed = false;
				_transition.Stop ();
				_shopping = false;
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
				ItemStand.SetActive (false);
			}
		}
	}

	void Update(){
		if (!_transition.isPlaying && _hasPlayed) {
			ItemStand.SetActive (true);
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
				_hasPlayed = true;


			}else if (Input.GetKeyDown (KeyCode.T) && _shopping) {
				_hasPlayed = false;
				_transition.Stop ();
				_shopping = false;
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
				ItemStand.SetActive (false);
			}
		}
	}
}