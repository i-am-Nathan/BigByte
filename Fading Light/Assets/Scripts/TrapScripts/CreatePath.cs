// file:	Assets\Scripts\TrapScripts\CreatePath.cs
//
// summary:	Implements the create path class

using UnityEngine;
using System.Collections;

/// <summary>   This will create the path for the player one by one. </summary>
///
/// <remarks>    . </remarks>

public class CreatePath : MonoBehaviour {

    /// <summary>   The platforms. </summary>
	public GameObject[] Platforms;
    /// <summary>   True if raised. </summary>
	private bool _raised;
	// Use this for initialization

    /// <summary>   Raises the platform one by one. </summary>
    ///
 

	void OnTriggerEnter(){
		if (!_raised) {
			_raised = true;

			StartCoroutine(PlatformRaise(Platforms));
			}



	}

     /// <summary>  This will be used to raise the platforms one by one using WaitForSeconds. </summary>
     ///
     /// <remarks>   . </remarks>
     ///
     /// <param name="platform">    Platform. </param>
     ///
     /// <returns>  The raise. </returns>

	 IEnumerator PlatformRaise (GameObject[] platform){	
		for (int i = 0; i < Platforms.Length; i++) {

			yield return new WaitForSeconds (1);
			Debug.Log (Time.deltaTime);
			Platforms[i].GetComponent<Animation> ().Play ();
		}
	}
}
