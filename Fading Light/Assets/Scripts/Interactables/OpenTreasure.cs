﻿// file:	assets\scripts\interactables\opentreasure.cs
//
// summary:	Implements the opentreasure class

using UnityEngine;
using System.Collections;

/// <summary>   Script which is used when players interact with Treasure chests. </summary>
///
/// <remarks>    . </remarks>

public class OpenTreasure : MonoBehaviour {
    /// <summary>   True if destroy when activated. </summary>
	public bool DestroyWhenActivated;
    /// <summary>   True to open. </summary>
	private bool _open; 
    /// <summary>   The speed. </summary>
	private float _speed;
    /// <summary>   The minimum. </summary>
	public float Minimum;
    /// <summary>   The maximum. </summary>
	public float Maximum;
    /// <summary>   List of drops. </summary>
	public GameObject[] DropList;
    /// <summary>   The drop chance. </summary>
	public int[] DropChance;
    /// <summary>   The prefab. </summary>
	private GameObject _prefab;

    /// <summary>   The treasure sound. </summary>
	public AudioClip TreasureSound;
    /// <summary>   The treasure opening. </summary>
	public AudioClip TreasureOpening;
    /// <summary>   Source for the. </summary>
	private AudioSource _source;

    /// <summary>
    /// This will get the Audio source compoenent to enable the treasure opening sound.
    /// </summary>
    ///
 

	void Awake(){
		_source = GetComponent<AudioSource>();
	}

    /// <summary>   Indicates chest is unopen and speed of chest opening. </summary>
    ///
 

	void Start () {
		_open = false;
		_speed = 60;
	}

    /// <summary>
    /// When players press T when they are collding with the chest, it will open it.
    /// </summary>
    ///
 
    ///
    /// <param name="other">    Other. </param>

	void OnTriggerStay(Collider other){
		if (other.name == "Player 1" || other.name == "Player2") {
			if (Input.GetKeyDown (KeyCode.Q) || Input.GetKeyDown(KeyCode.O)) {
				if (!_open) {
					StartCoroutine (Open ());
					_open = true;
				}
			}
		}
	}

    /// <summary>   Function used to open the treasure chest. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator Open()
	{
        var _achievementManager = (AchievementManager)GameObject.FindGameObjectWithTag("AchievementManager").GetComponent(typeof(AchievementManager));
        _achievementManager.AchievementObtained("Money Money Money~");
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
			yield return new WaitForSeconds (0.1f);
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
			float[] randomValues = new float[6] {-90,-60, -50, 90 , 60,50};

			float randomX = Random.Range (0,6);
			float randomZ = Random.Range (0,6);
			_prefab.GetComponent<Rigidbody>().velocity = new Vector3 ( randomValues[(int)randomX], 10f, randomValues[(int)randomZ]);
			//Plays the sound of treasure coming out.
			_source.PlayOneShot (TreasureSound);
		}
		_source.Stop ();

		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		GameData gameDataScript = (GameData)go.GetComponent(typeof(GameData));
		gameDataScript.UpdateChestsMissed ();

		Destroy(this.transform.FindChild ("Eternal Light").gameObject);
	}


}