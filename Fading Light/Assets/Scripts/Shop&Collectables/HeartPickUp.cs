﻿using UnityEngine;
using System.Collections;

public class HeartPickUp : MonoBehaviour {

	private AudioSource _source;
	public AudioClip PickUpSound;

	private bool _notPickedUp;
	private GameData _gameDataScript;
	private LifeManager _lifeManagerScript;

	void Awake()
	{
		_source = GetComponent<AudioSource>();
		_notPickedUp = true;
	}
		
	/// <summary>
	/// This will load up the player objects so that when coins are picked up, they will go to the respective player.
	/// </summary>
	void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));

		GameObject go1 = GameObject.FindGameObjectWithTag("Life Manager");
		_lifeManagerScript = (LifeManager)go1.GetComponent(typeof(LifeManager));
	}

	/// <summary>
	/// When player collides with the coin, they will increment the player's gold and play a sound when picked up.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other)
	{
		if (_notPickedUp && (other.tag == "Player" || other.tag == "Player2"))
		{
			_notPickedUp = false;
			_source.PlayOneShot(PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, PickUpSound.length + 0.1f);
			_gameDataScript.UpdateNumberOfLives ();
			_lifeManagerScript.UpdateHeartsOnUI ();
		}
	}
}
