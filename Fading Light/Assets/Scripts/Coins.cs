using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {
	public AudioClip PickUpSound;
	private AudioSource _source;
	private bool _notPickedUp;

	void Awake(){
		_source = GetComponent<AudioSource>();
		_notPickedUp = true;
	}
		
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player 1" && _notPickedUp) {
			_notPickedUp = false;
			_source.PlayOneShot (PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy (gameObject, PickUpSound.length+0.1f);

		} else if (other.name == "Player2" && _notPickedUp) {
			_notPickedUp = false;
			_source.PlayOneShot (PickUpSound);
			GetComponent<Renderer>().enabled = false;
			Destroy (gameObject, PickUpSound.length+0.1f);


		}

	}
}
