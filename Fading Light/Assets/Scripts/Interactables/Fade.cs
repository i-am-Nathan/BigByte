// file:	Assets\Scripts\Interactables\Fade.cs
//
// summary:	Implements the fade class

using UnityEngine;
using System.Collections;

/// <summary>   A fade. </summary>
///
/// <remarks>    . </remarks>

public static class Fade {

    /// <summary>   A Transform extension method that fade out 3D. </summary>
    ///
 
    ///
    /// <param name="t">            The t to act on. </param>
    /// <param name="targetAlpha">  Target alpha. </param>
    /// <param name="isVanish">     True if this object is vanish. </param>
    /// <param name="duration">     The duration. </param>
    ///
    /// <returns>   An IEnumerator. </returns>

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
