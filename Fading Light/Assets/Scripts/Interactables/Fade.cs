using UnityEngine;
using System.Collections;

public static class Fade {

	public static IEnumerator FadeOut3D (this Transform t, float targetAlpha, bool isVanish, float duration)
	{
		Renderer sr = t.GetComponent<Renderer> ();
		float diffAlpha = (targetAlpha - sr.material.color.a);

		float counter = 0;
		while (counter < duration) {
			float alphaAmount = sr.material.color.a + (Time.deltaTime * diffAlpha) / duration;
			sr.material.color = new Color (sr.material.color.r, sr.material.color.g, sr.material.color.b, alphaAmount);

			counter += Time.deltaTime;
			yield return null;
		}
		sr.material.color = new Color (sr.material.color.r, sr.material.color.g, sr.material.color.b, targetAlpha);
		if (isVanish) {
			sr.transform.gameObject.SetActive (false);
		}
	}
}
