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

/// <summary>   A ge orbit camera user interface. </summary>
///
/// <remarks>    . </remarks>

public class GE_OrbitCameraUI : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Unity UI elements
    /// <summary>   The toggle yaw. </summary>
	Toggle m_ToggleYaw = null;
    /// <summary>   The toggle pitch. </summary>
	Toggle m_TogglePitch = null;
    /// <summary>   The toggle zoom. </summary>
	Toggle m_ToggleZoom = null;
    /// <summary>   The toggle help. </summary>
	Toggle m_ToggleHelp = null;
    /// <summary>   The toggle details. </summary>
	Toggle m_ToggleDetails = null;
    /// <summary>   The pinch zoom control. </summary>
	Button m_PinchZoom = null;
    /// <summary>   The scroll zoom control. </summary>
	Button m_VScrollZoom = null;

    /// <summary>   The panel settings. </summary>
	GUIAnimFREE m_PanelSettings = null;
    /// <summary>   The button settings. </summary>
	GUIAnimFREE m_ButtonSettings = null;

    /// <summary>   The first m panel help. </summary>
	GUIAnimFREE m_PanelHelp1 = null;
    /// <summary>   The second m panel help. </summary>
	GUIAnimFREE m_PanelHelp2 = null;
    /// <summary>   The panel details. </summary>
	GUIAnimFREE m_PanelDetails = null;

    /// <summary>   The ge orbit camera. </summary>
	GE_OrbitCamera m_GE_OrbitCamera = null;

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

		// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false, 
		// this will let you control all GUI Animator elements in the scene via scripts.
		if (enabled)
		{
			GUIAnimSystemFREE.Instance.m_GUISpeed = 1.0f;
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}

		m_GE_OrbitCamera = GameObject.FindObjectOfType<GE_OrbitCamera>();

		// Find Unity UI elements
		GameObject go = GameObject.Find("Toggle Invert X");
		if (go != null)
			m_ToggleYaw = go.GetComponent<Toggle>();
		go = GameObject.Find("Toggle Invert Y");
		if (go != null)
			m_TogglePitch = go.GetComponent<Toggle>();
		go = GameObject.Find("Toggle Invert Zoom");
		if (go != null)
			m_ToggleZoom = go.GetComponent<Toggle>();
		go = GameObject.Find("Toggle Help");
		if (go != null)
			m_ToggleHelp = go.GetComponent<Toggle>();
		go = GameObject.Find("Toggle Details");
		if (go != null)
			m_ToggleDetails = go.GetComponent<Toggle>();
		go = GameObject.Find("Button Pinch Zoom");
		if (go != null)
			m_PinchZoom = go.GetComponent<Button>();
		go = GameObject.Find("Button V-Scroll Zoom");
		if (go != null)
			m_VScrollZoom = go.GetComponent<Button>();
		go = GameObject.Find("Panel Settings");
		if (go != null)
			m_PanelSettings = go.GetComponent<GUIAnimFREE>();
		go = GameObject.Find("Button Settings");
		if (go != null)
			m_ButtonSettings = go.GetComponent<GUIAnimFREE>();
		if (m_ButtonSettings != null)
		{
			m_ButtonSettings.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		}

		go = GameObject.Find("Panel Help1");
		if (go != null)
			m_PanelHelp1 = go.GetComponent<GUIAnimFREE>();
		go = GameObject.Find("Panel Help2");
		if (go != null)
			m_PanelHelp2 = go.GetComponent<GUIAnimFREE>();
		go = GameObject.Find("Panel Details");
		if (go != null)
			m_PanelDetails = go.GetComponent<GUIAnimFREE>();
		if (m_ToggleHelp != null)
		{
			if (m_ToggleHelp.isOn == true)
			{
				if (m_PanelHelp1 != null)
				{
					m_PanelHelp1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
				}
				if (m_PanelHelp2 != null)
				{
					m_PanelHelp2.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
				}
			}
		}
		if (m_ToggleDetails != null && m_PanelDetails != null)
		{
			if (m_ToggleDetails.isOn == true)
			{
				if (m_PanelDetails != null)
				{
					m_PanelDetails.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
				}
			}
		}

		// Setup some Unity UI elements
		if (m_GE_OrbitCamera != null)
		{
			if (m_ToggleYaw != null)
				m_ToggleYaw.isOn = m_GE_OrbitCamera.m_XInvert;
			if (m_TogglePitch != null)
				m_TogglePitch.isOn = m_GE_OrbitCamera.m_YInvert;
			if (m_ToggleZoom != null)
				m_ToggleZoom.isOn = m_GE_OrbitCamera.m_ZoomInvert;
		}
		if (m_ToggleHelp != null)
			m_ToggleHelp.isOn = true;
		if (m_ToggleDetails != null)
			m_ToggleDetails.isOn = true;
		if (m_PinchZoom != null)
			m_PinchZoom.interactable = false;
		if (m_VScrollZoom != null)
			m_VScrollZoom.interactable = true;

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
	// UI Responder functions
	// ########################################

	#region UI Responder

	// Toggle Invert X axis

    /// <summary>   Executes the toggle invert x coordinate action. </summary>
    ///
 

	public void OnToggle_InvertX()
	{
		if (m_ToggleYaw != null && m_GE_OrbitCamera != null)
		{
			m_GE_OrbitCamera.m_XInvert = m_ToggleYaw.isOn;
		}
	}

	// Toggle Invert Y axis

    /// <summary>   Executes the toggle invert y coordinate action. </summary>
    ///
 

	public void OnToggle_InvertY()
	{
		if (m_TogglePitch != null && m_GE_OrbitCamera != null)
		{
			m_GE_OrbitCamera.m_YInvert = m_TogglePitch.isOn;
		}
	}

	// Toggle Invert Zoom

    /// <summary>   Executes the toggle invert zoom action. </summary>
    ///
 

	public void OnToggle_InvertZoom()
	{
		if (m_ToggleZoom != null && m_GE_OrbitCamera != null)
		{
			m_GE_OrbitCamera.m_ZoomInvert = m_ToggleZoom.isOn;
		}
	}

	// Toggle show/hide Help panel

    /// <summary>   Executes the toggle help action. </summary>
    ///
 

	public void OnToggle_Help()
	{
		if (m_ToggleHelp != null)
		{
			if (m_ToggleHelp.isOn == true)
			{
				if (m_PanelHelp1 != null)
				{
					m_PanelHelp1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
				}
				if (m_PanelHelp2 != null)
				{
					m_PanelHelp2.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
				}
			}
			else
			{
				if (m_PanelHelp1 != null)
				{
					m_PanelHelp1.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
				}
				if (m_PanelHelp2 != null)
				{
					m_PanelHelp2.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
				}
			}
		}
	}

	// Toggle show/hide Details panel

    /// <summary>   Executes the toggle details action. </summary>
    ///
 

	public void OnToggle_Details()
	{
		if (m_ToggleDetails != null && m_PanelDetails != null)
		{
			if (m_ToggleDetails.isOn == true)
			{
				m_PanelDetails.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
			}
			else
			{
				m_PanelDetails.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
			}
		}
	}

	// Use pinch to zoom

    /// <summary>   Executes the button pinch zoom action. </summary>
    ///
 

	public void OnButton_PinchZoom()
	{
		if (m_PinchZoom != null)
		{
			m_PinchZoom.interactable = !m_PinchZoom.interactable;
		}
		if (m_VScrollZoom != null)
		{
			m_VScrollZoom.interactable = !m_VScrollZoom.interactable;
		}
	}

	// Use Two fingers verticle-scroll to zoom

    /// <summary>   Executes the button v scroll zoom action. </summary>
    ///
 

	public void OnButton_VScrollZoom()
	{
		if (m_PinchZoom != null)
		{
			m_PinchZoom.interactable = !m_PinchZoom.interactable;
		}
		if (m_VScrollZoom != null)
		{
			m_VScrollZoom.interactable = !m_VScrollZoom.interactable;
		}
	}

	// User clicks on Settings button

    /// <summary>   Executes the button settings action. </summary>
    ///
 

	public void OnButton_Settings()
	{
		if (m_PanelSettings != null)
		{
			// Toggle show/hide m_PanelSettings
			if (m_PanelSettings.transform.localScale == new Vector3(0, 0, 0))
			{
				m_PanelSettings.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
			}
			else
			{
				m_PanelSettings.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
			}
		}
	}

	#endregion // UI Responder
}
