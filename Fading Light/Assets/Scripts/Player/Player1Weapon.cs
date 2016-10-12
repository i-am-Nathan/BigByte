using UnityEngine;
using System.Collections;

public class Player1Weapon : MonoBehaviour
{

    public float WeaponDamage = 30f;
    private bool DEBUG = false;

    // Use this for initialization
    void Start()
    {

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

        if (player1.isAttacking() && other.tag == "Enemy")
        {
            if (DEBUG) Debug.Log("Weapon collision: Enemy");
            if (player1.isAttackPotActive()) WeaponDamage = WeaponDamage * 2;
            other.transform.GetComponent<BaseEntity>().Damage(WeaponDamage, this.transform.root);
            player1.setAttacking(false);
        }
    }
}
