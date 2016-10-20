using UnityEngine;
using System.Collections;

public class FireAOE : MonoBehaviour {

    public float DamagePerFrame = 2f;
    private bool DEBUG = false;

    void OnTriggerStay(Collider other)
    {
        if (DEBUG) Debug.Log("Collison detected");
        if (other.tag == "Player" || other.tag == "Player2")
        {
            if (DEBUG) Debug.Log("Moledog AOE collision: Player");
            other.GetComponent<Player>().Damage(DamagePerFrame, this.transform.root);             
        }
    }
}
