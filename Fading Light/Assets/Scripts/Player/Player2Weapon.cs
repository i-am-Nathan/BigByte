// file:	Assets\Scripts\Player\Player2Weapon.cs
//
// summary:	Implements the player 2 weapon class

using UnityEngine;
using System.Collections;

/// <summary>   A player 2 weapon. </summary>
///
/// <remarks>    . </remarks>

public class Player2Weapon : MonoBehaviour {

    /// <summary>   The weapon damage. </summary>
    public float WeaponDamage = 30f;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;
    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
	
	}

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {        
        Player weaponHolder = this.transform.root.GetComponent<Player>();
        Player2Controller player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<Player2Controller>();     
		if(player2.isAttacking() && other.name.Equals("Breakable_Wall")){
			Breakable_Wall breakableWall = (Breakable_Wall)other.gameObject.GetComponent(typeof(Breakable_Wall));
			breakableWall.Fade ();
		}

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
