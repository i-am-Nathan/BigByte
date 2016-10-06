using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {

    float weaponDamage = 30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BaseEntity>() != null)
        {
            other.gameObject.GetComponent<BaseEntity>().Damage(weaponDamage, this.transform.root);
        }
    }
}
