using UnityEngine;
using System.Collections;

/// <summary>
/// Class is used to pick up an item
/// </summary>
public class PickupItem : MonoBehaviour {

	// Sound source and reference to player
	public AudioClip PickUpSound;
	private AudioSource _source;
	private bool _notPickedUp;
	private PlayerController _player1Script;
	private Player2Controller _player2Script;

	/// <summary>
	/// Used to obtain sound
	/// </summary>
	void Awake() 
	{
		_source = GetComponent<AudioSource> ();
		_notPickedUp = true;
	}

	/// <summary>
	/// Used to obtain the appropriate scripts
	/// </summary>
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		_player1Script = (PlayerController)go.GetComponent(typeof(PlayerController));

		GameObject tempGo = GameObject.FindGameObjectWithTag("Player2");
		_player2Script = (Player2Controller)tempGo.GetComponent(typeof(Player2Controller));
	}

	/// <summary>
	/// Destroys the item when picked up
	/// </summary>
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
