using UnityEngine;
using System.Collections;

public class Player2Weapon : MonoBehaviour {

    public float WeaponDamage = 30f;
    private bool DEBUG = false;
	private GameData _gameDataScript;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {        
        Player weaponHolder = this.transform.root.GetComponent<Player>();
        Player2Controller player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<Player2Controller>();     

        if (DEBUG) Debug.Log(other.GetComponent<BaseEntity>());

		if (player2.isAttacking () && other.tag == "Enemy") {
			// Updating player 2's accuracy
			_gameDataScript.UpdatePlayerNumHitsAchieved (false);

			if (DEBUG)
				Debug.Log ("Weapon collision: Enemy");

			if (player2.isAttackPotActive ()) {
				WeaponDamage = WeaponDamage * 2;
			} else {
				WeaponDamage = 30f;
			}

			other.transform.GetComponent<BaseEntity> ().Damage (WeaponDamage, this.transform.root);
			player2.setAttacking (false);
		} else if (player2.isAttacking () && other.tag != "Enemy") {
			// Updating player 2's accuracy
			_gameDataScript.UpdatePlayerNumHitsMissed (false);
		}
    }
}
