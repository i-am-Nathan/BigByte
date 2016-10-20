// file:	Assets\Scripts\Player\Player1Weapon.cs
//
// summary:	Implements the player 1 weapon class

using UnityEngine;
using System.Collections;

/// <summary>   A player 1 weapon. </summary>
///
/// <remarks>    . </remarks>

public class Player1Weapon : MonoBehaviour
{

    /// <summary>   The weapon damage. </summary>
    public float WeaponDamage = 30f;
    /// <summary>   True to debug. </summary>
    private bool DEBUG = false;
    /// <summary>   The game data script. </summary>
	private GameData _gameDataScript;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
    }

    // Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update()
    {

    }

    /// <summary>   Executes the trigger enter action. </summary>
    ///
 
    ///
    /// <param name="other">    The other. </param>

    void OnTriggerEnter(Collider other)
    {
        Player weaponHolder = this.transform.root.GetComponent<Player>();
        PlayerController player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerController>();
        if (DEBUG) Debug.Log(other.GetComponent<BaseEntity>());

		if(player1.isAttacking() && other.name.Equals("Breakable_Wall")){
			Breakable_Wall breakableWall = (Breakable_Wall)other.gameObject.GetComponent(typeof(Breakable_Wall));
			breakableWall.Fade ();
		}

        if (player1.isAttacking() && other.tag == "Enemy")
        {
            if (DEBUG) Debug.Log("Weapon collision: Enemy");

			// Updating player 1's accuracy
			_gameDataScript.UpdatePlayerNumHitsAchieved (true);

			if (player1.isAttackPotActive ()) {
				WeaponDamage = WeaponDamage * 2;
			} else {
				WeaponDamage = 30f;
			}

            other.transform.GetComponent<BaseEntity>().Damage(WeaponDamage, this.transform.root);
            player1.setAttacking(false);
		} else if (player1.isAttacking () && other.tag != "Enemy") {
			// Updating player 1's accuracy
			_gameDataScript.UpdatePlayerNumHitsMissed (true);
		}
    }
}
