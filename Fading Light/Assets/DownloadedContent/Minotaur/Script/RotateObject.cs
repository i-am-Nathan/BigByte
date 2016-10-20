// file:	Assets\DownloadedContent\Minotaur\Script\RotateObject.cs
//
// summary:	Implements the rotate object class

using UnityEngine;
using System.Collections;

/// <summary>   A rotate object. </summary>
///
/// <remarks>    . </remarks>

public class RotateObject : MonoBehaviour {
    /// <summary>   this transform. </summary>
	private Transform thisTransform;
	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {
		if(Input.GetMouseButton(0)){
        	thisTransform.Rotate(Vector3.up *-15* Input.GetAxis("Mouse X"));
      	}
	}
}
