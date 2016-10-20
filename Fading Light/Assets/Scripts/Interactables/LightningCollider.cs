using UnityEngine;
using System.Collections;

public class LightningCollider : MonoBehaviour {

    private float _timer;

	// Use this for initialization
	void Start () {
        _timer = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.fixedTime - _timer) > 2f)
        {
            Destroy(gameObject);
        }
            
	}



}
