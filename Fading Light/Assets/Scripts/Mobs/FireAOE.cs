using UnityEngine;
using System.Collections;

public class FireAOE : MonoBehaviour {

    public float DamagePerSec = 5f;
    private bool DEBUG = false;

    void OnTriggerEnter(Collider other)
    {
        if (DEBUG) Debug.Log("Collison detected");
        //Destroy(gameObject, lifetime);
     
        if (other.tag == "Player" || other.tag == "Player2")
        {
            if (DEBUG) Debug.Log("Moledog AOE collision: Player");
            //other.Damage(damage, this.transform.root);
            
        }
    }
}
