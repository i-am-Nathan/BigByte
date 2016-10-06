using UnityEngine;
using System.Collections;

public class OpenTreasure : MonoBehaviour {
	public bool DestroyWhenActivated;
	private bool _open; 
	private float _speed;
	public float MinCoins;
	public float MaxCoins;
	public GameObject Coin;
	GameObject CoinPrefab;

	public AudioClip TreasureSound;
	public AudioClip TreasureOpening;
	private AudioSource _source;

	private AchievementManager _achievementManager;



	void Awake(){
		_source = GetComponent<AudioSource>();
	}


	void Start () {
		_open = false;
		_speed = 60;
		_achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag ("AchievementManager").GetComponent(typeof(AchievementManager));
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T)) {

				if (!_open) {
					_achievementManager.AddProgressToAchievement ("Money Money Money~", 1.0f);
					StartCoroutine (Open ());
					_open = true;
                    GameObject label = GameObject.FindGameObjectWithTag("tbl");
                    label.SetActive(false);
				}
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T)) {

				if (!_open) {
					_achievementManager.AddProgressToAchievement ("Money Money Money~", 1.0f);
					print ("STFU");
					StartCoroutine (Open ());
					_open = true;
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
			lid.transform.Rotate (new Vector3 (-1, 0, 0) * (_speed * Time.deltaTime));
			timePassed += Time.deltaTime;
			yield return null;

		}
		float randomNumber = Random.Range (5, 30);
		//for (int i = MinCoins; i < MaxCoins; i++) {  	//Add this later when you want to control the amount of coins in a treasure;
		for (int i = 0; i < randomNumber; i++) {
			yield return new WaitForSeconds (0.1f);

			CoinPrefab = Instantiate (Coin, transform.position + new Vector3(0,4,0), Quaternion.identity)as GameObject;
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
