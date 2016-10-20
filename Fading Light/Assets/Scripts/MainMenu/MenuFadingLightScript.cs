// file:	Assets\Scripts\MainMenu\MenuFadingLightScript.cs
//
// summary:	Implements the menu fading light script class

using UnityEngine;
using System.Collections;

/// <summary>   Fading light script is used for the light effects on the main menu. </summary>
///
/// <remarks>    . </remarks>

public class MenuFadingLightScript : MonoBehaviour {
    /// <summary>   The lt. </summary>
    public Light lt;
    /// <summary>   True if this object is fade. </summary>
    private bool isFade= false;

    // Use this for initialization

    /// <summary>   Starts this object. </summary>
    ///
 

    void Start()
    {
        lt = GetComponent<Light>();
    }
    // Update is called once per frame
	//Updating the light

    /// <summary>   Updates this object. </summary>
    ///
 

    void Update () {
        if(lt.intensity < 0.3f)
        {
            isFade = false;
        }
        else if(lt.intensity > 1.5f)
        {
            isFade = true;
        }

        if (isFade == false)
        {
            lt.intensity += 0.005f;
        }
        else if (isFade == true) 
        {
            lt.intensity -= 0.005f;
        }
	}
}
