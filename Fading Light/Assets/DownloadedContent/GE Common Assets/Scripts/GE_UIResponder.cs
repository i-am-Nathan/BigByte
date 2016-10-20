// GE Common Assets 1.0
// Free asssets for using in many packages of Gold Experience Team.
//
// Author:	Gold Experience Team (http://www.ge-team.com)

// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

#region Namespaces

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// GE_UIResponder class
// Changes Title name and open webpage when user clicks on title name.
// ######################################################################

/// <summary>   A ge user interface responder. </summary>
///
/// <remarks>    . </remarks>

public class GE_UIResponder : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

    /// <summary>   The package title. </summary>
	public string m_PackageTitle = "-";
    /// <summary>   URL of the target. </summary>
	public string m_TargetURL = "www.unity3d.com";

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start()
	{

		GameObject go = GameObject.Find("Text Package Title");
		if (go != null)
		{
			Text m_PackageText = go.GetComponent<Text>();
			m_PackageText.text = m_PackageTitle;
		}
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update()
	{
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder Functions
	// ########################################

	#region UI Responder

	// User click/touch on title name

    /// <summary>   Executes the button asset name action. </summary>
    ///
 

	public void OnButton_AssetName()
	{
		// http://docs.unity3d.com/ScriptReference/Application.OpenURL.html
		Application.OpenURL(m_TargetURL);
	}

	#endregion // UI Responder
}
