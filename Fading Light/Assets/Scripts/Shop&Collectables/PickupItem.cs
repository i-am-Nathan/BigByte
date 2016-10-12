using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

	public AudioClip PickUpSound;
	private AudioSource _source;
	private bool _notPickedUp;
	private PlayerController _player1Script;
	private Player2Controller _player2Script;

	void Awake() 
	{
		_source = GetComponent<AudioSource> ();
		_notPickedUp = true;
	}
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		_player1Script = (PlayerController)go.GetComponent(typeof(PlayerController));

		GameObject tempGo = GameObject.FindGameObjectWithTag("Player2");
		_player2Script = (Player2Controller)tempGo.GetComponent(typeof(Player2Controller));
	}

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
