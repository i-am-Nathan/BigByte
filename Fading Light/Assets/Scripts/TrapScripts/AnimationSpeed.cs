using UnityEngine;
using System.Collections;

/// <summary>
/// This will allow users to scale an animation speed for a animation of their choice.
/// </summary>
public class AnimationSpeed : MonoBehaviour {

	public float AnimSpeed;
	private Animation _anim;
	public string AnimationName;
	// Use this for initialization
	/// <summary>
	/// Get the animation component of the object this script is attached to.
	/// </summary>
	void Start () {
		_anim = gameObject.GetComponent<Animation> ();
	}
	
	/// <summary>
	/// Updates the animation speed.
	/// </summary>
	void Update () {
		_anim [AnimationName].speed = AnimSpeed;
	}


}
