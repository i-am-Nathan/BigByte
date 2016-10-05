using UnityEngine;
using System.Collections;

public class OpenTreasure : MonoBehaviour {
	public bool destroyWhenActivated;
	private bool open; 
	private float speed;
	public float MinCoins;
	public float MaxCoins;
	public GameObject coin;
	GameObject CoinPrefab;

	public AudioClip TreasureSound;
	public AudioClip TreasureOpening;
	private AudioSource _source;


	void Awake(){
		_source = GetComponent<AudioSource>();
	}


	void Start () {
		open = false;
		speed = 60;
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.Y)) {

				if (!open) {
					print ("STFU");
					StartCoroutine (Open ());
					open = true;
				}
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.Y)) {

				if (!open) {
					print ("STFU");
					StartCoroutine (Open ());
					open = true;
				}
			}
		}
	}
		

		

	IEnumerator Open()
	{


		float timePassed = 0;
		_source.PlayOneShot (TreasureOpening);
		while (timePassed < 2)
		{

			GameObject lid = GameObject.FindGameObjectWithTag ("Lid");
			lid.transform.Rotate (new Vector3 (-1, 0, 0) * (speed * Time.deltaTime));
			timePassed += Time.deltaTime;
			yield return null;

		}
		float randomNumber = Random.Range (5, 30);
		//for (int i = MinCoins; i < MaxCoins; i++) {  	//Add this later when you want to control the amount of coins in a treasure;
		for (int i = 0; i < randomNumber; i++) {
			yield return new WaitForSeconds (0.1f);

			CoinPrefab = Instantiate (coin, transform.position + new Vector3(0,4,0), Quaternion.identity)as GameObject;
			float randomX = Random.Range (-5, 5)*5;
			float randomZ = Random.Range (-5, 5)*5;
			//CoinPrefab.GetComponent<Rigidbody> ().AddForce (new Vector3 (randomX,60000f, randomZ));
			CoinPrefab.GetComponent<Rigidbody>().velocity = new Vector3 ( randomX, 0.01f, randomZ);
			_source.PlayOneShot (TreasureSound);
			//CoinPrefab.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (randomX, 0.000000001f, randomZ));
			//CoinPrefab.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, -50, 0));
		}
		_source.Stop ();
	}


}
