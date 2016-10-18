using UnityEngine;
using System.Collections;

public class CreatePath : MonoBehaviour {

	public GameObject[] Platforms;
	private bool _raised;
	// Use this for initialization
	void OnTriggerEnter(){
		if (!_raised) {
			_raised = true;

			StartCoroutine(PlatformRaise(Platforms));
			}



	}
	 IEnumerator PlatformRaise (GameObject[] platform){	
		for (int i = 0; i < Platforms.Length; i++) {

			yield return new WaitForSeconds (1);
			Debug.Log (Time.deltaTime);
			Platforms[i].GetComponent<Animation> ().Play ();
		}
	}
}
