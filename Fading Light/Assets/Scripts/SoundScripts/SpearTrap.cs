// file:	Assets\Scripts\SoundScripts\SpearTrap.cs
//
// summary:	Implements the spear trap class

using UnityEngine;
using System.Collections;

/// <summary>   A spear trap. </summary>
///
/// <remarks>    . </remarks>

public class SpearTrap : MonoBehaviour {
    /// <summary>   The spear sound. </summary>
    public AudioSource SpearSound;
    /// <summary>   The spear speed. </summary>
    public float SpearSpeed = 3f;
    /// <summary>   The animation. </summary>
    private Animation anim;
	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        StartCoroutine(SpearAnim());
    }

    /// <summary>   Repeat coroutine. </summary>
    ///
 

    void RepeatCoroutine()
    {
        StartCoroutine(SpearAnim());
    }

    /// <summary>   Spear animation. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

    private IEnumerator SpearAnim()
    {
        anim.Play();
        SpearSound.Play();
        yield return new WaitForSeconds(SpearSpeed);
        RepeatCoroutine();
        yield return null;
    }
}
