// file:	Assets\DownloadedContent\AllStarCharacterLibrary\Scripts\MouseOrbit.cs
//
// summary:	Implements the mouse orbit class

using UnityEngine;
using System.Collections;

/// <summary>   A mouse orbit. </summary>
///
/// <remarks>    . </remarks>

public class MouseOrbit : MonoBehaviour 
{
    /// <summary>   Target for the. </summary>
	public Transform target;
    /// <summary>   The distance. </summary>
	float distance = 15f;
    /// <summary>   The speed. </summary>
	float xSpeed = 4.0f;
    /// <summary>   The speed. </summary>
	float ySpeed = 1.0f;
    /// <summary>   The x coordinate. </summary>
	float x = 0.0f;
    /// <summary>   The y coordinate. </summary>
	float y = 2.0f;

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start () 
	{
	    Vector3 angles = transform.eulerAngles;
    	x = angles.y;
    	y = angles.x;

		// Make the rigid body not change rotation
   		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		
		//Screen.showCursor = false;
	}
	
	// Update is called once per frame

    /// <summary>   Late update. </summary>
    ///
 

	void LateUpdate () 
	{
		distance += Input.GetAxis("Mouse ScrollWheel") * 5 ;
		
		if ( Input.GetKey(KeyCode.LeftAlt))
		{
			if (Input.GetMouseButton(1))
			{
				distance += Input.GetAxis("Mouse Y") * 0.5f;
			}
			
			if (Input.GetMouseButton(0))
			{
				x += Input.GetAxis("Mouse X") * xSpeed*3;
				y -= Input.GetAxis("Mouse Y") * ySpeed*8;
				y = ClampAngle(y);
				x = ClampAngle(x);
				transform.rotation = Quaternion.Euler( y, x, 0.0f);
			}
			

			if (Input.GetMouseButton(2))
			{
				float x2 = Input.GetAxis("Mouse X");
				float y2 = Input.GetAxis("Mouse Y");
				target.transform.position += transform.right * (-x2*0.2f);
				target.transform.position += transform.up * (-y2 *0.2f);
			}

		}
		transform.position = target.transform.position - (transform.forward * distance);
	}

    /// <summary>   Clamp angle. </summary>
    ///
 
    ///
    /// <param name="angle">    The angle. </param>
    ///
    /// <returns>   A float. </returns>

	float ClampAngle (float angle) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return angle;
	}

/// <summary>   Executes the graphical user interface action. </summary>
///
/// <remarks>    . </remarks>

void OnGUI()
	{

		string tempString = "ALT+LMB to orbit,   ALT+RMB to zoom,   ALT+MMB to pan";
		GUI.Label (new Rect (10, 25,1000, 20), tempString);
	}
}
