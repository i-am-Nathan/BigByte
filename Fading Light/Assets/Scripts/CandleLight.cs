using UnityEngine;
using System.Collections;

public class CandleLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // When collision occurs between two objects
    void OnTriggerStay(Collider other)
    {

        // Checking if players are next to each other
        if (other.gameObject.tag.Equals("Player2") || other.gameObject.tag.Equals("Player1"))
        {
            Debug.Log("Hit candle");
            // Checking if users wanted to swap the torch
            if (Input.GetButtonDown("CandleLight"))
            {
                
            }
        }
    }
}
