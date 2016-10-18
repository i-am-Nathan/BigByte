using UnityEngine;
using System.Collections;

public class FireAOE : MonoBehaviour {

    public float damage = 20f;
    private bool DEBUG = false;

    void OnTriggerEnter(Collider other)
    {
        if (DEBUG) Debug.Log("Collison detected");
        //Destroy(gameObject, lifetime);
     
        if (other.tag == "Player" || other.tag == "Player2")
        {
            if (DEBUG) Debug.Log("Fireball collision: Player");
            other.Damage(damage, this.transform.root);

            _isExploded = true;

            if (DEBUG) Debug.Log("Creating fireball explosion");
            GameObject newFireball = (GameObject)Instantiate(Resources.Load("Explosion"));
            Vector3 newPos = new Vector3(this.transform.position.x, 6, this.transform.position.z);
            newFireball.transform.position = newPos;
            GameObject.Destroy(gameObject);
        }
    }
}
