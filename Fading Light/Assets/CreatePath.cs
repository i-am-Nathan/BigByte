using UnityEngine;
using System.Collections;

public class CreatePath : MonoBehaviour {

	public GameObject[] Platforms;
	private bool _raised;
	// Use this for initialization
	void Update(){
		if (!_raised) {
			_raised = true;
			for (int i = 0; i < Platforms.Length; i++) {
				StartCoroutine(PlatformRaise(Platforms[i]));
			}

		}

	}
	 IEnumerator PlatformRaise (GameObject platform){
		yield return new WaitForSeconds (1);
		platform.GetComponent<Animation> ().Play ();
		yield return null;
	}
}
