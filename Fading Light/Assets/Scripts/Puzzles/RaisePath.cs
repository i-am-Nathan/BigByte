using UnityEngine;
using System.Collections;

public class RaisePath : MonoBehaviour {

	private int _thingsOnTop = 0;
	public GameObject path;


	void OnTriggerEnter(Collider other)
	{

		//if the weight is heavy enough, then the plate is triggered
		if (other.name=="Player 1" || other.name=="Player2")

		{
			this.GetComponent<Animation>().Play("PressurePlateDown");
			path.GetComponent<Animation>().Play();

		}

	}
}
