using UnityEngine;
using System.Collections;

/// <summary>
/// This will create the path for the player one by one.
/// </summary>
public class CreatePath : MonoBehaviour {

	public GameObject[] Platforms;
	private bool _raised;
	// Use this for initialization
	/// <summary>
	/// Raises the platform one by one.
	/// </summary>
	void OnTriggerEnter(){
		if (!_raised) {
			_raised = true;

			StartCoroutine(PlatformRaise(Platforms));
			}



	}
	/// <summary>
	/// This will be used to raise the platforms one by one using WaitForSeconds.
	/// </summary>
	/// <returns>The raise.</returns>
	/// <param name="platform">Platform.</param>
	 IEnumerator PlatformRaise (GameObject[] platform){	
		for (int i = 0; i < Platforms.Length; i++) {

			yield return new WaitForSeconds (1);
			Debug.Log (Time.deltaTime);
			Platforms[i].GetComponent<Animation> ().Play ();
		}
	}
}
