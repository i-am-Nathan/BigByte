using UnityEngine;
using System.Collections;

public class Breakable_Wall : MonoBehaviour
{
	public void Fade(){
		GetComponent<AudioSource>().Play();
		StartCoroutine (transform.FadeOut3D (0, true, 2));
	}

}