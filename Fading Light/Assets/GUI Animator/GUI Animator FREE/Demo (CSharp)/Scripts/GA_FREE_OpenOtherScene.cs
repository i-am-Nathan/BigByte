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
// GA_FREE_OpenOtherScene class
// This class handles 8 buttons for changing scene.
// ######################################################################

/// <summary>   A ga free open other scene. </summary>
///
/// <remarks>    . </remarks>

public class GA_FREE_OpenOtherScene : MonoBehaviour
{
	
	// ########################################
	// MonoBehaviour Functions
	// ########################################
	
	#region MonoBehaviour
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () {		
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update () {		
	}
	
	#endregion // MonoBehaviour
	
	// ########################################
	// UI Responder functions
	// ########################################
	
	#region UI Responder
	
	// Open Demo Scene 1

    /// <summary>   Button open demo scene 1. </summary>
    ///
 

	public void ButtonOpenDemoScene1 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo01 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 2

    /// <summary>   Button open demo scene 2. </summary>
    ///
 

	public void ButtonOpenDemoScene2 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo02 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 3

    /// <summary>   Button open demo scene 3. </summary>
    ///
 

	public void ButtonOpenDemoScene3 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo03 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 4

    /// <summary>   Button open demo scene 4. </summary>
    ///
 

	public void ButtonOpenDemoScene4 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo04 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 5

    /// <summary>   Button open demo scene 5. </summary>
    ///
 

	public void ButtonOpenDemoScene5 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo05 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 6

    /// <summary>   Button open demo scene 6. </summary>
    ///
 

	public void ButtonOpenDemoScene6 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo06 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 7

    /// <summary>   Button open demo scene 7. </summary>
    ///
 

	public void ButtonOpenDemoScene7 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo07 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	// Open Demo Scene 8

    /// <summary>   Button open demo scene 8. </summary>
    ///
 

	public void ButtonOpenDemoScene8 ()
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableAllButtons(false);

		// Waits 1.5 secs for Moving Out animation then load next level
		GUIAnimSystemFREE.Instance.LoadLevel("GA FREE - Demo08 (960x600px)", 1.5f);
		
		gameObject.SendMessage("HideAllGUIs");
	}
	
	#endregion // UI Responder
}
