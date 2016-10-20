using UnityEngine;
using System.Collections;

public class Level1End : MonoBehaviour {

	private EndOfLevelTrigger _endOfLevel;

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("EndOfLevelTrigger");
		_endOfLevel = (EndOfLevelTrigger)go.GetComponent(typeof(EndOfLevelTrigger));
	}

	/// <summary>
	/// Called when a player enters the box collider placed at the end of the level
	/// </summary>
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player2" || c.gameObject.tag == "Player1") {
			_endOfLevel.TriggerEndOfLevel ();
		}
	}
}
