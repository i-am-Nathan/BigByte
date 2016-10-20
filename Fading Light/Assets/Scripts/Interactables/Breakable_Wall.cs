using UnityEngine;
using System.Collections;

public class Breakable_Wall : MonoBehaviour
{
	public void Fade(){
		StartCoroutine (transform.FadeOut3D (0, true, 2));
	}

}