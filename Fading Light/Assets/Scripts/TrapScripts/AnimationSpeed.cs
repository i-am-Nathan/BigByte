using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour {

	public float AnimSpeed;
	private Animation _anim;
	public string AnimationName;
	// Use this for initialization
	void Start () {
		_anim = gameObject.GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		_anim [AnimationName].speed = AnimSpeed;
	}


}
