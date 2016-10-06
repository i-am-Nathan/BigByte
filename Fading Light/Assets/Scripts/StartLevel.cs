using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {
	public float WaitTime;
	public void Start(){
		
		Fade ();
	}
	public void Fade(){
		StartCoroutine (FadeText ());
	}

	IEnumerator FadeText(){
		CanvasGroup cg = GetComponent<CanvasGroup> ();
		yield return new WaitForSeconds(WaitTime);
		while (cg.alpha > 0) {
			cg.alpha -= Time.deltaTime / 2;
			yield return null;
		}
		cg.interactable = false;
		yield return null;
	}
}
