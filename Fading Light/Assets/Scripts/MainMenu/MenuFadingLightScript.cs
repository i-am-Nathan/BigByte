using UnityEngine;
using System.Collections;

/// <summary>
/// Fading light script is used for the light effects on the main menu.
/// </summary>
public class MenuFadingLightScript : MonoBehaviour {
    public Light lt;
    private bool isFade= false;

    // Use this for initialization
    void Start()
    {
        lt = GetComponent<Light>();
    }
    // Update is called once per frame
	//Updating the light
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
