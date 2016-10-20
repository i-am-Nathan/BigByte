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
// GA_FREE_Demo06 class
// - Animates all GUIAnimFREE elements in the scene.
// - Responds to user mouse click or tap on buttons.
//
// Note this class is attached with "-SceneController-" object in "GA FREE - Demo06 (960x600px)" scene.
// ######################################################################

/// <summary>   A ga free demo 06. </summary>
///
/// <remarks>    . </remarks>

public class GA_FREE_Demo06 : MonoBehaviour
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
	
	// GUIAnimFREE objects of primary buttons
    /// <summary>   The first m primary button. </summary>
	public GUIAnimFREE m_PrimaryButton1;
    /// <summary>   The second m primary button. </summary>
	public GUIAnimFREE m_PrimaryButton2;
    /// <summary>   The third m primary button. </summary>
	public GUIAnimFREE m_PrimaryButton3;
    /// <summary>   The fourth m primary button. </summary>
	public GUIAnimFREE m_PrimaryButton4;
    /// <summary>   The fifth m primary button. </summary>
	public GUIAnimFREE m_PrimaryButton5;

	// GUIAnimFREE objects of secondary buttons
    /// <summary>   The first m secondary button. </summary>
	public GUIAnimFREE m_SecondaryButton1;
    /// <summary>   The second m secondary button. </summary>
	public GUIAnimFREE m_SecondaryButton2;
    /// <summary>   The third m secondary button. </summary>
	public GUIAnimFREE m_SecondaryButton3;
    /// <summary>   The fourth m secondary button. </summary>
	public GUIAnimFREE m_SecondaryButton4;
    /// <summary>   The fifth m secondary button. </summary>
	public GUIAnimFREE m_SecondaryButton5;
	
	// Toggle state of buttons
    /// <summary>   True if button 1 is on. </summary>
	bool m_Button1_IsOn = false;
    /// <summary>   True if button 2 is on. </summary>
	bool m_Button2_IsOn = false;
    /// <summary>   True if button 3 is on. </summary>
	bool m_Button3_IsOn = false;
    /// <summary>   True if button 4 is on. </summary>
	bool m_Button4_IsOn = false;
    /// <summary>   True if button 5 is on. </summary>
	bool m_Button5_IsOn = false;
	
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

		// Enable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, true);
	}
	
	// MoveIn all primary buttons

    /// <summary>   Move in primary buttons. </summary>
    ///
 
    ///
    /// <returns>   An IEnumerator. </returns>

	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn all primary buttons
		m_PrimaryButton1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);	
		m_PrimaryButton2.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);	
		m_PrimaryButton3.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);	
		m_PrimaryButton4.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);

		m_PrimaryButton5.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);
		
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}
	
	// MoveOut all primary buttons

    /// <summary>   Hides all gu is. </summary>
    ///
 

	public void HideAllGUIs()
	{
		// MoveOut all primary buttons
		m_PrimaryButton1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_PrimaryButton2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_PrimaryButton3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_PrimaryButton4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_PrimaryButton5.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// MoveOut all secondary buttons
		if(m_Button1_IsOn == true)
			m_SecondaryButton1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if(m_Button2_IsOn == true)
			m_SecondaryButton2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if(m_Button3_IsOn == true)
			m_SecondaryButton3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if(m_Button4_IsOn == true)
			m_SecondaryButton4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		if(m_Button5_IsOn == true)
			m_SecondaryButton5.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
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

    /// <summary>   Executes the button 1 action. </summary>
    ///
 

	public void OnButton_1()
	{
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(0.6f));

		// Toggle m_Button1
		ToggleButton_1();
		
		// Toggle other buttons
		if(m_Button2_IsOn==true)
		{
			ToggleButton_2();
		}
		if(m_Button3_IsOn==true)
		{
			ToggleButton_3();
		}
		if(m_Button4_IsOn==true)
		{
			ToggleButton_4();
		}
		if(m_Button5_IsOn==true)
		{
			ToggleButton_5();
		}
	}

    /// <summary>   Executes the button 2 action. </summary>
    ///
 

	public void OnButton_2()
	{
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(0.6f));

		// Toggle m_Button2
		ToggleButton_2();
		
		// Toggle other buttons
		if(m_Button1_IsOn==true)
		{
			ToggleButton_1();
		}
		if(m_Button3_IsOn==true)
		{
			ToggleButton_3();
		}
		if(m_Button4_IsOn==true)
		{
			ToggleButton_4();
		}
		if(m_Button5_IsOn==true)
		{
			ToggleButton_5();
		}
	}

    /// <summary>   Executes the button 3 action. </summary>
    ///
 

	public void OnButton_3()
	{
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(0.6f));

		// Toggle m_Button3
		ToggleButton_3();
		
		// Toggle other buttons
		if(m_Button1_IsOn==true)
		{
			ToggleButton_1();
		}
		if(m_Button2_IsOn==true)
		{
			ToggleButton_2();
		}
		if(m_Button4_IsOn==true)
		{
			ToggleButton_4();
		}
		if(m_Button5_IsOn==true)
		{
			ToggleButton_5();
		}
	}

    /// <summary>   Executes the button 4 action. </summary>
    ///
 

	public void OnButton_4()
	{
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(0.6f));

		// Toggle m_Button4
		ToggleButton_4();
		
		// Toggle other buttons
		if(m_Button1_IsOn==true)
		{
			ToggleButton_1();
		}
		if(m_Button2_IsOn==true)
		{
			ToggleButton_2();
		}
		if(m_Button3_IsOn==true)
		{
			ToggleButton_3();
		}
		if(m_Button5_IsOn==true)
		{
			ToggleButton_5();
		}
	}

    /// <summary>   Executes the button 5 action. </summary>
    ///
 

	public void OnButton_5()
	{
		// Disable all buttons for a few seconds
		StartCoroutine(DisableAllButtonsForSeconds(0.6f));

		// Toggle m_Button5
		ToggleButton_5();
		
		// Toggle other buttons
		if(m_Button1_IsOn==true)
		{
			ToggleButton_1();
		}
		if(m_Button2_IsOn==true)
		{
			ToggleButton_2();
		}
		if(m_Button3_IsOn==true)
		{
			ToggleButton_3();
		}
		if(m_Button4_IsOn==true)
		{
			ToggleButton_4();
		}
	}
	
	#endregion // UI Responder
	
	// ########################################
	// Toggle button functions
	// ########################################
	
	#region Toggle Button
	
	// Toggle m_Button1

    /// <summary>   Toggle button 1. </summary>
    ///
 

	void ToggleButton_1()
	{
		m_Button1_IsOn = !m_Button1_IsOn;
		if(m_Button1_IsOn==true)
		{
			// MoveIn m_SecondaryButton1
			m_SecondaryButton1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// MoveOut m_SecondaryButton1
			m_SecondaryButton1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}
	
	// Toggle m_Button2

    /// <summary>   Toggle button 2. </summary>
    ///
 

	void ToggleButton_2()
	{
		m_Button2_IsOn = !m_Button2_IsOn;
		if(m_Button2_IsOn==true)
		{
			// MoveIn m_SecondaryButton2
			m_SecondaryButton2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// MoveOut m_SecondaryButton2
			m_SecondaryButton2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}
	
	// Toggle m_Button3

    /// <summary>   Toggle button 3. </summary>
    ///
 

	void ToggleButton_3()
	{
		m_Button3_IsOn = !m_Button3_IsOn;
		if(m_Button3_IsOn==true)
		{
			// MoveIn m_SecondaryButton3
			m_SecondaryButton3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// MoveOut m_SecondaryButton3
			m_SecondaryButton3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}
	
	// Toggle m_Button4

    /// <summary>   Toggle button 4. </summary>
    ///
 

	void ToggleButton_4()
	{
		m_Button4_IsOn = !m_Button4_IsOn;
		if(m_Button4_IsOn==true)
		{
			// MoveIn m_SecondaryButton4
			m_SecondaryButton4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// MoveOut m_SecondaryButton4
			m_SecondaryButton4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}
	
	// Toggle m_Button5

    /// <summary>   Toggle button 5. </summary>
    ///
 

	void ToggleButton_5()
	{
		m_Button5_IsOn = !m_Button5_IsOn;
		if(m_Button5_IsOn==true)
		{
			// MoveIn m_SecondaryButton5
			m_SecondaryButton5.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// MoveOut m_SecondaryButton5
			m_SecondaryButton5.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}
	
	#endregion // Toggle Button
}
