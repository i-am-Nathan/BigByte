// file:	Assets\Scripts\Shop&Collectables\PickupItem.cs
//
// summary:	Implements the pickup item class

using UnityEngine;
using System.Collections;

/// <summary>   Class is used to pick up an item. </summary>
///
/// <remarks>    . </remarks>

public class PickupItem : MonoBehaviour {

	// Sound source and reference to player
    /// <summary>   The pick up sound. </summary>
	public AudioClip PickUpSound;
    /// <summary>   Source for the. </summary>
	private AudioSource _source;
    /// <summary>   True to not picked up. </summary>
	private bool _notPickedUp;
    /// <summary>   The player 1 script. </summary>
	private PlayerController _player1Script;
    /// <summary>   The player 2 script. </summary>
	private Player2Controller _player2Script;

    /// <summary>   Used to obtain sound. </summary>
    ///
 

	void Awake() 
	{
		_source = GetComponent<AudioSource> ();
		_notPickedUp = true;
	}

    /// <summary>   Used to obtain the appropriate scripts. </summary>
    ///
 

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		_player1Script = (PlayerController)go.GetComponent(typeof(PlayerController));

		GameObject tempGo = GameObject.FindGameObjectWithTag("Player2");
		_player2Script = (Player2Controller)tempGo.GetComponent(typeof(Player2Controller));
	}

    /// <summary>   Destroys the item when picked up. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player 1" && _notPickedUp)
		{
			_notPickedUp = false;
			_source.PlayOneShot(PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, PickUpSound.length + 0.1f);
		}		
		if (other.name == "Player2" && _notPickedUp)
		{
			_notPickedUp = false;
			_source.PlayOneShot (PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, PickUpSound.length + 0.1f);
		}

	}
}
