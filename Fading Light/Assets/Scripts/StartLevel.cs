using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

	public void Start(){
		
		Fade ();
	}
	public void Fade(){
		StartCoroutine (FadeText ());
	}

	IEnumerator FadeText(){
		CanvasGroup cg = GetComponent<CanvasGroup> ();
		yield return new WaitForSeconds(2f);
		while (cg.alpha > 0) {
			cg.alpha -= Time.deltaTime / 2;
			yield return null;
		}
		cg.interactable = false;
		yield return null;
	}
}
