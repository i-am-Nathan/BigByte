// file:	Assets\Scripts\TrapScripts\AnimationSpeed.cs
//
// summary:	Implements the animation speed class

using UnityEngine;
using System.Collections;

/// <summary>
/// This will allow users to scale an animation speed for a animation of their choice.
/// </summary>
///
/// <remarks>    . </remarks>

public class AnimationSpeed : MonoBehaviour {

    /// <summary>   The animation speed. </summary>
	public float AnimSpeed;
    /// <summary>   The animation. </summary>
	private Animation _anim;
    /// <summary>   Name of the animation. </summary>
	public string AnimationName;
	// Use this for initialization

    /// <summary>   Get the animation component of the object this script is attached to. </summary>
    ///
 

	void Start () {
		_anim = gameObject.GetComponent<Animation> ();
	}

    /// <summary>   Updates the animation speed. </summary>
    ///
 

	void Update () {
		_anim [AnimationName].speed = AnimSpeed;
	}


}
