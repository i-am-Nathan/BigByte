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
// GA_FREE_Demo04 class
// - Animates all GUIAnimFREE elements in the scene.
// - Responds to user mouse click or tap on buttons.
//
// Note this class is attached with "-SceneController-" object in "GA FREE - Demo04 (960x600px)" scene.
// ######################################################################

/// <summary>   A ga free demo 04. </summary>
///
/// <remarks>    . </remarks>

public class GA_FREE_Demo04 : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################
	
	#region Variables

	// Canvas
    /// <summary>   The canvas. </summary>
	public Canvas m_Canvas;
	
	// GUIAnimFREE objects of title text
    /// <summary>   The first m title. </summary>
	public GUIAnimFREE m_Title1;
    /// <summary>   The second m title. </summary>
	public GUIAnimFREE m_Title2;
	
	// GUIAnimFREE objects of top and bottom bars
    /// <summary>   The top bar. </summary>
	public GUIAnimFREE m_TopBar;
    /// <summary>   The bottom bar. </summary>
	public GUIAnimFREE m_BottomBar;
	
	// GUIAnimFREE object of dialogs
    /// <summary>   The first m dialog. </summary>
	public GUIAnimFREE m_Dialog1;
    /// <summary>   The second m dialog. </summary>
	public GUIAnimFREE m_Dialog2;
    /// <summary>   The third m dialog. </summary>
	public GUIAnimFREE m_Dialog3;
    /// <summary>   The fourth m dialog. </summary>
	public GUIAnimFREE m_Dialog4;
	
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
		
		// MoveIn m_Title1 m_Title2
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
	
	// Move In m_Title1 and m_Title2

    /// <summary>   Move in title game objects. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator MoveInTitleGameObjects()
	{
		yield return new WaitForSeconds(1.0f);
		
		// Move In m_Title1 and m_Title2
		m_Title1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		m_Title2.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		
		// MoveIn dialogs
		StartCoroutine(MoveInPrimaryButtons());
	}
	
	// MoveIn dialogs

    /// <summary>   Move in primary buttons. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn dialogs
		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);		
		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);		
		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);		
		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}

    /// <summary>   Hides all gu is. </summary>
    ///
 

	public void HideAllGUIs()
	{
		// MoveOut dialogs
		m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
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

	// Disable a button for a few seconds

    /// <summary>   Disables the button for seconds. </summary>
    ///
 
    ///
    /// <param name="GO">           The go. </param>
    /// <param name="DisableTime">  The disable time. </param>
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator DisableButtonForSeconds(GameObject GO, float DisableTime)
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, false);
		
		yield return new WaitForSeconds(DisableTime);
		
		// Enable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, true);
	}
	
	#endregion // Enable/Disable buttons
	
	// ########################################
	// UI Responder functions
	// ########################################
	
	#region UI Responder

    /// <summary>   Executes the button dialog 1 action. </summary>
    ///
 

	public void OnButton_Dialog1()
	{
		// MoveOut m_Dialog1
		m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable m_Dialog1 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.0f));
		
		// Moves m_Dialog1 back to screen
		StartCoroutine(Dialog1_MoveIn());
	}

    /// <summary>   Executes the button dialog 2 action. </summary>
    ///
 

	public void OnButton_Dialog2()
	{
		// MoveOut m_Dialog2
		m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable m_Dialog2 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog2.gameObject, 2.0f));
		
		// Moves m_Dialog2 back to screen
		StartCoroutine(Dialog2_MoveIn());
	}

    /// <summary>   Executes the button dialog 3 action. </summary>
    ///
 

	public void OnButton_Dialog3()
	{
		// MoveOut m_Dialog3
		m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable m_Dialog3 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog3.gameObject, 2.0f));
		
		// Moves m_Dialog3 back to screen
		StartCoroutine(Dialog3_MoveIn());
	}

    /// <summary>   Executes the button dialog 4 action. </summary>
    ///
 

	public void OnButton_Dialog4()
	{
		// MoveOut m_Dialog4
		m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// Disable m_Dialog4 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog4.gameObject, 2.0f));
		
		// Moves m_Dialog4 back to screen
		StartCoroutine(Dialog4_MoveIn());
	}

    /// <summary>   Executes the button move out all dialogs action. </summary>
    ///
 

	public void OnButton_MoveOutAllDialogs()
	{
		
		// Disable m_Dialog1, m_Dialog2, m_Dialog3, m_Dialog4 for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.0f));
		StartCoroutine(DisableButtonForSeconds(m_Dialog2.gameObject, 2.0f));
		StartCoroutine(DisableButtonForSeconds(m_Dialog3.gameObject, 2.0f));
		StartCoroutine(DisableButtonForSeconds(m_Dialog4.gameObject, 2.0f));

		if(m_Dialog1.m_MoveOut.Began==false && m_Dialog1.m_MoveOut.Done==false)
		{
			// Move m_Dialog1 out
			m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
			// Move m_Dialog1 back to screen with Coroutines
			StartCoroutine(Dialog1_MoveIn());
		}
		if(m_Dialog2.m_MoveOut.Began==false && m_Dialog2.m_MoveOut.Done==false)
		{
			// Move m_Dialog2 out
			m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
			// Move m_Dialog2 back to screen with Coroutines
			StartCoroutine(Dialog2_MoveIn());
		}
		if(m_Dialog3.m_MoveOut.Began==false && m_Dialog3.m_MoveOut.Done==false)
		{
			// Move m_Dialog3 out
			m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
			// Move m_Dialog3 back to screen with Coroutines
			StartCoroutine(Dialog3_MoveIn());
		}
		if(m_Dialog4.m_MoveOut.Began==false && m_Dialog4.m_MoveOut.Done==false)
		{
			// Move m_Dialog4 out
			m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
			// Move m_Dialog4 back to screen with Coroutines
			StartCoroutine(Dialog4_MoveIn());
		}

	}
	
	#endregion // UI Responder
	
	// ########################################
	// Move dialog functions
	// ########################################
	
	#region Move Dialog
	
	// MoveIn m_Dialog1

    /// <summary>   Dialog 1 move in. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator Dialog1_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog1
		m_Dialog1.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen
		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog2

    /// <summary>   Dialog 2 move in. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator Dialog2_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog2
		m_Dialog2.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen
		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog3

    /// <summary>   Dialog 3 move in. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator Dialog3_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog3
		m_Dialog3.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen
		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog4

    /// <summary>   Dialog 4 move in. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator Dialog4_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog4
		m_Dialog4.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen to screen
		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	#endregion // Move Dialog
}
