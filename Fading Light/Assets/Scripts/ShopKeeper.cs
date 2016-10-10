﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour {
	public GameObject TextBox;
	public Camera MainCamera;
	public Camera ShopKeeperCamera;
	private bool _shopping;
	private Animation _transition;
	public PlayerController Player1;
	public Player2Controller Player2;
	private bool _hasPlayed;
	void Awake(){
		ShopKeeperCamera.enabled = false;
		_transition = ShopKeeperCamera.GetComponent<Animation> ();
	}
		
	void Start(){
		_hasPlayed = false;
		TextBox.SetActive (false);

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
				_shopping = false;
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
				TextBox.SetActive(false);
			}
		}
	}

	void Update(){
		if (!_transition.isPlaying && _hasPlayed && _shopping) {
			TextBox.SetActive(true);
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
				MainCamera.enabled = true;
				ShopKeeperCamera.enabled = false;
				_shopping = false;
				Player1.IsDisabled = false;
				Player2.IsDisabled = false;
				TextBox.SetActive(false);
			}
		}
	}
}