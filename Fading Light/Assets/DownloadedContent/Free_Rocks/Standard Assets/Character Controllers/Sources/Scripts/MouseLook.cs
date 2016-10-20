// file:	Assets\DownloadedContent\Free_Rocks\Standard Assets\Character Controllers\Sources\Scripts\MouseLook.cs
//
// summary:	Implements the mouse look class

using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// <summary>
/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head.
///   The character already turns.)
/// </summary>
///
/// <remarks>    . </remarks>

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

    /// <summary>   Values that represent rotation axes. </summary>
    ///
 

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    /// <summary>   The axes. </summary>
	public RotationAxes axes = RotationAxes.MouseXAndY;
    /// <summary>   The sensitivity x coordinate. </summary>
	public float sensitivityX = 15F;
    /// <summary>   The sensitivity y coordinate. </summary>
	public float sensitivityY = 15F;

    /// <summary>   The minimum x coordinate. </summary>
	public float minimumX = -360F;
    /// <summary>   The maximum x coordinate. </summary>
	public float maximumX = 360F;

    /// <summary>   The minimum y coordinate. </summary>
	public float minimumY = -60F;
    /// <summary>   The maximum y coordinate. </summary>
	public float maximumY = 60F;

    /// <summary>   The rotation y coordinate. </summary>
	float rotationY = 0F;

    /// <summary>   Updates this object. </summary>
    ///
 

	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}

    /// <summary>   Starts this object. </summary>
    ///
 

	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}