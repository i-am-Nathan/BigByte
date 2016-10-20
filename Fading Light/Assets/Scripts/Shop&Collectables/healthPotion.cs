// file:	Assets\Scripts\Shop&Collectables\healthPotion.cs
//
// summary:	Implements the health potion class

using UnityEngine;
using System.Collections;

/// <summary>
/// Health potions which players can pick up and purchase items through the shop.
/// </summary>
///
/// <remarks>    . </remarks>

public class HealthPotion : MonoBehaviour {

	// Audio source
    /// <summary>   Source for the. </summary>
	private AudioSource _source;
    /// <summary>   The pick up sound. </summary>
	public AudioClip PickUpSound;

    /// <summary>   True to not picked up. </summary>
	private bool _notPickedUp;
    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;

    /// <summary>   Called to obtain the audio source. </summary>
    ///
 

	void Awake()
	{
		_source = GetComponent<AudioSource>();
		_notPickedUp = true;
	}

    /// <summary>
    /// This will load up the player objects so that when potions are picked up, they will go to the
    /// respective player.
    /// </summary>
    ///
 

	void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
	}

    /// <summary>
    /// When player collides with the potion, they will increment the player's number of potions and
    /// play a sound when picked up.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerEnter(Collider other)
	{
		// Checking if picked up by a player 
		if (_notPickedUp && (other.tag == "Player" || other.tag == "Player2"))
		{
			_notPickedUp = false;
			_source.PlayOneShot(PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, PickUpSound.length + 0.1f);

			// Updating subinventory manager
			if (other.tag == "Player")
			{
				SubInventoryManager SubInventoryManager = GameObject.Find("SubInventoryManager").GetComponent<SubInventoryManager>();
				SubInventoryManager.AddItemQuantity("Health Potion", true);

			} else
			{
				SubInventoryManager SubInventoryManager = GameObject.Find("SubInventoryManager").GetComponent<SubInventoryManager>();
				SubInventoryManager.AddItemQuantity("Health Potion", false);
			}
		}
	}
}
