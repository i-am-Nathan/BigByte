// file:	Assets\Scripts\GameControl\Level1End.cs
//
// summary:	Implements the level 1 end class

using UnityEngine;
using System.Collections;

/// <summary>
/// Class use specifically to determine when the players have successfully completed level one.
/// </summary>
///
/// <remarks>    . </remarks>

public class Level1End : MonoBehaviour {

    /// <summary>   The end of level. </summary>
	private EndOfLevelTrigger _endOfLevel;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("EndOfLevelTrigger");
		_endOfLevel = (EndOfLevelTrigger)go.GetComponent(typeof(EndOfLevelTrigger));
	}

    /// <summary>
    /// Called when a player enters the box collider placed at the end of the level.
    /// </summary>
    ///
 
    ///
    /// <param name="c">    The Collider to process. </param>

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player2" || c.gameObject.tag == "Player1") {
			// Triggering end of level function
			_endOfLevel.TriggerEndOfLevel ();
		}
	}
}
