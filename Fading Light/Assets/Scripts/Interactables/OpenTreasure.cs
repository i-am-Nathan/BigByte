using UnityEngine;
using System.Collections;
/// <summary>
/// Script which is used when players interact with Treasure chests.
/// </summary>
public class OpenTreasure : MonoBehaviour {
	public bool DestroyWhenActivated;
	private bool _open; 
	private float _speed;
	public float Minimum;
	public float Maximum;
	public GameObject[] DropList;
	public int[] DropChance;
	private GameObject _prefab;

	public AudioClip TreasureSound;
	public AudioClip TreasureOpening;
	private AudioSource _source;

	/// <summary>
	/// This will get the Audio source compoenent to enable the treasure opening sound.
	/// </summary>
	void Awake(){
		_source = GetComponent<AudioSource>();
	}


	/// <summary>
	/// Indicates chest is unopen and speed of chest opening
	/// </summary>
	void Start () {
		_open = false;
		_speed = 60;
	}


	/// <summary>
	/// When players press T when they are collding with the chest, it will open it.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T)) {
				if (!_open) {
					StartCoroutine (Open ());
					_open = true;
				}
			}
		}
	}

	/// <summary>
	/// When players press T when they are collding with the chest, it will open it.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.T)) {
				if (!_open) {
					StartCoroutine (Open ());
					_open = true;
				}
			}
		}
	}
		

		

	/// <summary>
	/// Function used to open the treasure chest
	/// </summary>
	IEnumerator Open()
	{

		//Plays a sound when treasure is opening.
		float timePassed = 0;
		_source.PlayOneShot (TreasureOpening);
		while (timePassed < 2)
		{
			//Rotates the treasure lid and opens it over a period of time.
			
			transform.Find("box_top").transform.Rotate (new Vector3 (-1, 0, 0) * (_speed * Time.deltaTime));
			timePassed += Time.deltaTime;
			yield return null;

		}

		float randomNumber = Random.Range (Minimum, Maximum);
		//for (int i = MinCoins; i < MaxCoins; i++) {  	//Add this later when you want to control the amount of coins in a treasure;
		//This will instantiate a coin which will fly out in random directins from the chest. 
	
		for (int i = 0; i < randomNumber; i++) {
			yield return new WaitForSeconds (0.01f);
			int itemIndex = 0;
			float drop = Random.Range (1, 100);
			for (int j = 0; j < DropChance.Length; j++) {
				drop = drop - DropChance [j];
				if (drop <= 0 ) {
					itemIndex = j;
					break;
				}

			}
			_prefab = Instantiate (DropList[itemIndex], transform.position + new Vector3(0,4,0), Quaternion.identity)as GameObject;
			float[] randomValues = new float[10] {-40,-30,-50, -10, -20, 10,20,30,40,50};

			float randomX = Random.Range (0,10);
			float randomZ = Random.Range (0,10);
			_prefab.GetComponent<Rigidbody>().velocity = new Vector3 ( randomValues[(int)randomX], 10f, randomValues[(int)randomZ]);
			//Plays the sound of treasure coming out.
			_source.PlayOneShot (TreasureSound);
		}
		_source.Stop ();
		Destroy(this.transform.FindChild ("Eternal Light").gameObject);
	}


}