// file:	Assets\Scripts\UI\StartLevel.cs
//
// summary:	Implements the start level class

using UnityEngine;
using System.Collections;

/// <summary>   This class will be used for the level name at the beginning. </summary>
///
/// <remarks>    . </remarks>

public class StartLevel : MonoBehaviour
{
    //The time taken before the text starts to fade.
    /// <summary>   The wait time. </summary>
    public float WaitTime;

    /// <summary>   Start this instance. </summary>
    ///
 

    public void Start()
    {

        Fade();
    }

    /// <summary>   Method which starts a coroutine. </summary>
    ///
 

    public void Fade()
    {
        StartCoroutine(FadeText());
    }

    /// <summary>   This function will fade text over a period of time. </summary>
    ///
 
    ///
    /// <returns>   The text. </returns>

    IEnumerator FadeText()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(WaitTime);
        //This code will start removing the alpha value from the text, stripping it of its colour
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        cg.interactable = false;
        yield return null;
    }
}