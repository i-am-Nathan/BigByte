using UnityEngine;
using System.Collections;

public class SkeleWeapon : MonoBehaviour
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
        SkeleBoss skele = this.transform.root.GetComponent<SkeleBoss>();

        if (skele.isAttacking() && (other.tag == "Player" || other.tag == "Player2"))
        {
            if (DEBUG) Debug.Log("Weapon collision: Enemy");
            other.transform.GetComponent<BaseEntity>().Damage(WeaponDamage, this.transform.root);
            skele.setAttacking(false);
        }
    }
}
