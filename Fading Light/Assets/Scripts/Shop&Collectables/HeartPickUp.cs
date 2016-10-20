﻿// file:	Assets\Scripts\Shop&Collectables\HeartPickUp.cs
//
// summary:	Implements the heart pick up class

using UnityEngine;
using System.Collections;

/// <summary>   Extra lives which players can pick up. </summary>
///
/// <remarks>    . </remarks>

public class HeartPickUp : MonoBehaviour {

	// Audio source
    /// <summary>   Source for the. </summary>
	private AudioSource _source;
    /// <summary>   The pick up sound. </summary>
	public AudioClip PickUpSound;

    /// <summary>   True to not picked up. </summary>
	private bool _notPickedUp;
    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;
    /// <summary>   The life manager script. </summary>
	private LifeManager _lifeManagerScript;

    /// <summary>   Called to obtain the audio source. </summary>
    ///
 

	void Awake()
	{
		_source = GetComponent<AudioSource>();
		_notPickedUp = true;
	}

    /// <summary>   Obtaining scripts required. </summary>
    ///
 

	void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));

		GameObject go1 = GameObject.FindGameObjectWithTag("Life Manager");
		_lifeManagerScript = (LifeManager)go1.GetComponent(typeof(LifeManager));
	}

    /// <summary>
    /// When player collides with the heart, they will increment the shared lives between players.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerEnter(Collider other)
	{
		if (_notPickedUp && (other.tag == "Player" || other.tag == "Player2"))
		{
			// Destroying heart
			_notPickedUp = false;
			_source.PlayOneShot(PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, PickUpSound.length + 0.1f);

			// Updating the number of lives
			_gameDataScript.UpdateNumberOfLives ();
			_lifeManagerScript.UpdateHeartsOnUI ();
		}
	}
}
