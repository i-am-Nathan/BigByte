// GUI Animator FREE
// Version: 1.0.1
// Unity 4.7.1 or higher and Unity 5.3.4 or higher compatilble, see more info in Readme.txt file.
//
// Author:				Gold Experience Team (http://www.ge-team.com)
//
// Unity Asset Store:					https://www.assetstore.unity3d.com/en/#!/content/58843
// GE Store:							http://www.ge-team.com/store/en/products/gui-animator-free/
// Full version on Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Full version on GE Store:			http://www.ge-team.com/store/en/products/gui-animator-for-unity-ui/
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion

// ######################################################################
// GUIAnimatorFREEDemo class
// This class shows buttons and it plays Move-In and Move-Out animations when user pressed the buttons.
// ######################################################################

/// <summary>   A graphical user interface animator free demo. </summary>
///
/// <remarks>    . </remarks>

public class GUIAnimatorFREEDemo : MonoBehaviour
{

	// ########################################
	// MonoBehaviour functions
	// ########################################

	#region MonoBehaviour Functions

    /// <summary>   The wait time. </summary>
	private float m_WaitTime = 4.0f;
    /// <summary>   Number of wait times. </summary>
	private float m_WaitTimeCount = 0;
    /// <summary>   True to show, false to hide the move in button. </summary>
	private bool m_ShowMoveInButton = true;

	// Use this for initialization

    /// <summary>   Awakes this object. </summary>
    ///
 

	void Awake()
	{
		// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false, 
		// this will let you control all GUI Animator elements in the scene via scripts.
		if (enabled)
		{
			GUIAnimSystemFREE.Instance.m_GUISpeed = 1.0f;
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	// Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start()
	{
	}

	// Update is called once per frame

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update()
	{

		// Count down timer for MoveIn/MoveOut buttons
		if (m_WaitTimeCount > 0 && m_WaitTimeCount <= m_WaitTime)
		{
			m_WaitTimeCount -= Time.deltaTime;
			if (m_WaitTimeCount <= 0)
			{
				m_WaitTimeCount = 0;

				// Switch status of m_ShowMoveInButton
				m_ShowMoveInButton = !m_ShowMoveInButton;
			}
		}
	}

    /// <summary>   Executes the graphical user interface action. </summary>
    ///
 

	void OnGUI()
	{
		// Show GUI button when ready
		if (m_WaitTimeCount <= 0)
		{
			Rect rect = new Rect((Screen.width - 100) / 2, (Screen.height - 50) / 2, 250, 50);
			// Show MoveIn button
			if (m_ShowMoveInButton == true)
			{
				if (GUI.Button(rect, "Play In-animations then Idle-animations"))
				{
					// Play MoveIn animations
					GUIAnimSystemFREE.Instance.MoveIn(this.transform, true);
					m_WaitTimeCount = m_WaitTime;
				}
			}
			// Show MoveOut button
			else
			{
				if (GUI.Button(rect, "Play Out-animations"))
				{
					// Play MoveOut animations
					GUIAnimSystemFREE.Instance.MoveOut(this.transform, true);
					m_WaitTimeCount = m_WaitTime;
				}
			}
		}
	}

	#endregion // MonoBehaviour Functions
}
