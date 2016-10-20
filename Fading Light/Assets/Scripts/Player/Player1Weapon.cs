using UnityEngine;
using System.Collections;

public class Player1Weapon : MonoBehaviour
{

    public float WeaponDamage = 30f;
    private bool DEBUG = false;
	private GameData _gameDataScript;

    // Use this for initialization
    void Start()
    {
		GameObject go = GameObject.FindGameObjectWithTag("Game Data");
		_gameDataScript = (GameData)go.GetComponent(typeof(GameData));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Player weaponHolder = this.transform.root.GetComponent<Player>();
        PlayerController player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerController>();
        if (DEBUG) Debug.Log(other.GetComponent<BaseEntity>());
		if(player1.isAttacking()&&other.name.StartsWith("Breakable Wall")){
			FadeObjectInOut fade = (FadeObjectInOut)other.GetComponent (typeof(FadeObjectInOut));
			fade.FadeOut ();
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
