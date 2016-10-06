using UnityEngine;
using System.Collections;

public class ToolTips : MonoBehaviour {
	private GameObject[] tags = GameObject.FindGameObjectsWithTag("DirectionToolTip");

	public void EnableToolTips(){
		foreach ( GameObject currentTag in tags){
			currentTag.SetActive (true);
		}
	}

	public void DisableToolTips(){
		foreach ( GameObject currentTag in tags){
            currentTag.SetActive (false);
		}
	}
		
}
