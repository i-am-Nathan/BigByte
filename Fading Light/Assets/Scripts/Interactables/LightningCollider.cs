// file:	Assets\Scripts\Interactables\LightningCollider.cs
//
// summary:	Implements the lightning collider class

using UnityEngine;
using System.Collections;

/// <summary>   A lightning collider. </summary>
///
/// <remarks>    . </remarks>

public class LightningCollider : MonoBehaviour {

    /// <summary>   The timer. </summary>
    private float _timer;

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
        _timer = Time.fixedTime;
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
        if ((Time.fixedTime - _timer) > 2f)
        {
            Destroy(gameObject);
        }
            
	}



}
