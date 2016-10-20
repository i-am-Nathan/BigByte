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

#endregion // Namespaces

// ######################################################################
// GA_FREE_Demo02 class
// - Animates all GUIAnimFREE elements in the scene.
// - Responds to user mouse click or tap on buttons.
//
// Note this class is attached with "-SceneController-" object in "GA FREE - Demo02 (960x600px)" scene.
// ######################################################################

/// <summary>   A ga free demo 02. </summary>
///
/// <remarks>    . </remarks>

public class GA_FREE_Demo02 : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################
	
	#region Variables

	// Canvas
    /// <summary>   The canvas. </summary>
	public Canvas m_Canvas;
	
	// GUIAnimFREE objects of Title text
    /// <summary>   The first m title. </summary>
	public GUIAnimFREE m_Title1;
    /// <summary>   The second m title. </summary>
	public GUIAnimFREE m_Title2;
	
	// GUIAnimFREE objects of Top and bottom
    /// <summary>   The top bar. </summary>
	public GUIAnimFREE m_TopBar;
    /// <summary>   The bottom bar. </summary>
	public GUIAnimFREE m_BottomBar;
	
	// GUIAnimFREE object of Dialog
    /// <summary>   The dialog. </summary>
	public GUIAnimFREE m_Dialog;
	
	#endregion // Variables
	
	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################
	
	#region MonoBehaviour
	
	// Awake is called when the script instance is being loaded.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html

    /// <summary>   Awakes this object. </summary>
    ///
 

	void Awake ()
	{
		if(enabled)
		{
			// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start ()
	{
		// MoveIn m_TopBar and m_BottomBar
		m_TopBar.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_BottomBar.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// MoveIn m_Title1 and m_Title2
		StartCoroutine(MoveInTitleGameObjects());

		// Disable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, false);
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
	// MoveIn/MoveOut functions
	// ########################################
	
	#region MoveIn/MoveOut
	
	// MoveIn m_Title1 and m_Title2

    /// <summary>   Move in title game objects. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator MoveInTitleGameObjects()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn m_Title1 and m_Title2
		m_Title1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		m_Title2.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		
		// MoveIn m_Dialog
		StartCoroutine(ShowDialog());
	}
	
	// MoveIn m_Dialog

    /// <summary>   Shows the dialog. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator ShowDialog()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn m_Dialog
		m_Dialog.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}
	
	// MoveOut m_Dialog

    /// <summary>   Hides all gu is. </summary>
    ///
 

	public void HideAllGUIs()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// MoveOut m_Title1 and m_Title2
		StartCoroutine(HideTitleTextMeshes());
	}
	
	// MoveOut m_Title1 and m_Title2

    /// <summary>   Hides the title text meshes. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator HideTitleTextMeshes()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveOut m_Title1 and m_Title2
		m_Title1.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
		m_Title2.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);
		
		// MoveOut m_TopBar and m_BottomBar
		m_TopBar.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_BottomBar.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	#endregion // MoveIn/MoveOut
	
	// ########################################
	// Enable/Disable button functions
	// ########################################
	
	#region Enable/Disable buttons
	
	// Enable/Disable all scene switch Coroutine

    /// <summary>   Enables all demo buttons. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator EnableAllDemoButtons()
	{
		yield return new WaitForSeconds(1.0f);

		// Enable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, true);
	}

	// Disable all buttons for a few seconds

    /// <summary>   Disables all buttons for seconds. </summary>
    ///
 
    ///
    /// <param name="DisableTime">  The disable time. </param>
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator DisableAllButtonsForSeconds(float DisableTime)
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);
		
		yield return new WaitForSeconds(DisableTime);
		
		// Enable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(true);
	}
	
	#endregion // Enable/Disable buttons
	
	// ########################################
	// UI Responder functions
	// ########################################
	
	#region UI Responder

    /// <summary>   Executes the button upper edge action. </summary>
    ///
 

	public void OnButton_UpperEdge()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		// MoveIn m_Dialog from top
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.UpperScreenEdge));
	}

    /// <summary>   Executes the button left edge action. </summary>
    ///
 

	public void OnButton_LeftEdge()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// MoveIn m_Dialog from left
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.LeftScreenEdge));
	}

    /// <summary>   Executes the button right edge action. </summary>
    ///
 

	public void OnButton_RightEdge()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from right
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.RightScreenEdge));
	}

    /// <summary>   Executes the button bottom edge action. </summary>
    ///
 

	public void OnButton_BottomEdge()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from bottom
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.BottomScreenEdge));
	}

    /// <summary>   Executes the button upper left action. </summary>
    ///
 

	public void OnButton_UpperLeft()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from upper left
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.UpperLeft));
	}

    /// <summary>   Executes the button upper right action. </summary>
    ///
 

	public void OnButton_UpperRight()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from upper right
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.UpperRight));
	}

    /// <summary>   Executes the button bottom left action. </summary>
    ///
 

	public void OnButton_BottomLeft()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from bottom left
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.BottomLeft));
	}

    /// <summary>   Executes the button bottom right action. </summary>
    ///
 

	public void OnButton_BottomRight()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from bottom right
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.BottomRight));
	}

    /// <summary>   Executes the button center action. </summary>
    ///
 

	public void OnButton_Center()
	{
		// MoveOut m_Dialog
		m_Dialog.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(2.0f));
		
		// MoveIn m_Dialog from center of screen
		StartCoroutine(DialogMoveIn(GUIAnimFREE.ePosMove.MiddleCenter));
	}
	
	#endregion // UI Responder
	
	// ########################################
	// Move dialog functions
	// ########################################
	
	#region Move Dialog

	// MoveIn m_Dialog by position

    /// <summary>   Dialog move in. </summary>
    ///
 
    ///
    /// <param name="PosMoveIn">    The position move in. </param>
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator DialogMoveIn(GUIAnimFREE.ePosMove PosMoveIn)
	{
		yield return new WaitForSeconds(1.5f);
		
		//Debug.Log("PosMoveIn="+PosMoveIn);
		switch(PosMoveIn)
		{
			// Set m_Dialog to move in from upper
			case GUIAnimFREE.ePosMove.UpperScreenEdge:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.UpperScreenEdge;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from left
			case GUIAnimFREE.ePosMove.LeftScreenEdge:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.LeftScreenEdge;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from right
			case GUIAnimFREE.ePosMove.RightScreenEdge:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.RightScreenEdge;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from bottom
			case GUIAnimFREE.ePosMove.BottomScreenEdge:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.BottomScreenEdge;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from upper left
			case GUIAnimFREE.ePosMove.UpperLeft:	
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.UpperLeft;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from upper right
			case GUIAnimFREE.ePosMove.UpperRight:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.UpperRight;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from bottom left
			case GUIAnimFREE.ePosMove.BottomLeft:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.BottomLeft;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from bottom right
			case GUIAnimFREE.ePosMove.BottomRight:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.BottomRight;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
			// Set m_Dialog to move in from center
			case GUIAnimFREE.ePosMove.MiddleCenter:
			default:
				m_Dialog.m_MoveIn.MoveFrom = GUIAnimFREE.ePosMove.MiddleCenter;
				m_Dialog.m_MoveOut.MoveTo = GUIAnimFREE.ePosMove.MiddleCenter;
				break;
		}

		// Reset m_Dialog
		m_Dialog.Reset();

		// MoveIn m_Dialog by position
		m_Dialog.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	#endregion //  Move Dialog
}