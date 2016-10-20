// file:	Assets\Scripts\Interactables\Breakable_Wall.cs
//
// summary:	Implements the breakable wall class

using UnityEngine;
using System.Collections;

/// <summary>   A breakable wall. </summary>
///
/// <remarks>   Jack, 21/10/2016. </remarks>

public class Breakable_Wall : MonoBehaviour
{
    /// <summary>   Fades this object. </summary>
    ///
    /// <remarks>   Jack, 21/10/2016. </remarks>

	public void Fade(){
		Destroy (GetComponent<BoxCollider> ());
		GetComponent<AudioSource>().Play();
		StartCoroutine (transform.FadeOut3D (0, true, 2));
	}

}