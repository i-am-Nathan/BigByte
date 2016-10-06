using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {

    public float WeaponDamage = 30f;
    private bool DEBUG = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {        
        Player weaponHolder = this.transform.root.GetComponent<Player>();
        Player player1 = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
        Player player2 = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<Player>();

        if (DEBUG) Debug.Log("Weapon collision. Player1 is attacking: " + player1.isAttacking());
        if (DEBUG) Debug.Log("Weapon collision. Player2 is attacking: " + player2.isAttacking());        
        if (DEBUG) Debug.Log(other.GetComponent<BaseEntity>());

        if ((player1.isAttacking() || player2.isAttacking()) && other.tag == "Enemy")
        {
            if (DEBUG) Debug.Log("Weapon collision: Enemy");
            other.transform.GetComponent<BaseEntity>().Damage(WeaponDamage, this.transform.root);
            player1.setAttacking(false);
            player2.setAttacking(false);
        }
    }
}
