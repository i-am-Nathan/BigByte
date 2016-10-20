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
// GE_OrbitCamera class
// Handles mouse and touch inputs for orbiting the camera around the target object.
// ######################################################################

/// <summary>   A ge toggle full screen user interface. </summary>
///
/// <remarks>    . </remarks>

public class GE_ToggleFullScreenUI : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Screen resolutions
    /// <summary>   Width of the definition. </summary>
	int m_DefWidth;
    /// <summary>   Height of the definition. </summary>
	int m_DefHeight;

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
 

	void Start ()
	{
		
		// Store current screen resolution
		m_DefWidth = Screen.width;
		m_DefHeight = Screen.height;

		if(Application.isEditor==true)
		{
		}
		else
		{			
			// Show Toogle Full Screen button when player is Unity Web, WebGL, Standalone
			if (Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.WindowsWebPlayer ||
				Application.platform == RuntimePlatform.WebGLPlayer ||
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.LinuxPlayer)
			{
				this.gameObject.SetActive (true);			
			}
			// Other cases hide the button
			else
			{
				this.gameObject.SetActive (false);
			}
		}
		
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update ()
	{
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder Functions
	// ########################################

	#region UI Responder

	// User click on Toggle Fullscreen button

    /// <summary>   Executes the button toggle full screen action. </summary>
    ///
 

	public void OnButton_ToggleFullScreen ()
	{
		// Disable this button on Unity Editor
		if(Application.isEditor==true)
		{
			if(this.gameObject.activeSelf==true)
			{
				Button pButton = this.gameObject.GetComponent<Button>();
				pButton.interactable = false;
				foreach(Transform tr in this.transform)
				{
					tr.gameObject.SetActive(true);
				}
			}
		}
		// Toggle full screen
		else
		{
			Screen.fullScreen = !Screen.fullScreen;
			if (!Screen.fullScreen)
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
			else
			{
				Screen.SetResolution(m_DefWidth, m_DefHeight, false);
			}
		}
	}

	#endregion // UI Responder
}
