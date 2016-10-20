// file:	Assets\Scripts\UI\ToolTips.cs
//
// summary:	Implements the tool tips class

using UnityEngine;
using System.Collections;

/// <summary>   Used to enable and disable tool tips. </summary>
///
/// <remarks>    . </remarks>

public class ToolTips : MonoBehaviour {
    /// <summary>   The tags. </summary>
	private GameObject[] tags = GameObject.FindGameObjectsWithTag("DirectionToolTip");

    /// <summary>   Enables the tool tips. </summary>
    ///
 

    public void EnableToolTips(){
		foreach ( GameObject currentTag in tags){
			currentTag.SetActive (true);
		}
	}

    /// <summary>   Disables the tool tips. </summary>
    ///
 

    public void DisableToolTips(){
		foreach ( GameObject currentTag in tags){
            currentTag.SetActive (false);
		}
	}
		
}
