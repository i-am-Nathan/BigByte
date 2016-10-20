// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\CameraView.cs
//
// summary:	Implements the camera view class

using UnityEngine;
using System.Collections;

namespace Footsteps {

    /// <summary>   A camera view. </summary>
    ///
 

	public class CameraView : MonoBehaviour {

        /// <summary>   The minimum tilt angle. </summary>
		[SerializeField] float minTiltAngle = -70f;
        /// <summary>   The maximum tilt angle. </summary>
		[SerializeField] float maxTiltAngle = 80f;
        /// <summary>   The sensitivity. </summary>
		[SerializeField] float sensitivity = 3f;
        /// <summary>   The smooth. </summary>
		[SerializeField] bool smooth = true;
        /// <summary>   The smooth factor. </summary>
		[SerializeField] float smoothFactor = 15f;

        /// <summary>   Gets a value indicating whether the invert. </summary>
        ///
        /// <value> True if invert, false if not. </value>

		[SerializeField] bool invert;

        /// <summary>   Gets the world camera. </summary>
        ///
        /// <value> The world camera. </value>

		[Header("References")]
		[SerializeField] Transform worldCamera;

        /// <summary>   The character transform. </summary>
		Transform characterTransform;

        /// <summary>   The character target rotation. </summary>
		Quaternion characterTargetRotation;
        /// <summary>   The camera target rotation. </summary>
		Quaternion cameraTargetRotation;
        /// <summary>   The last mouse position. </summary>
		Vector2 lastMousePosition;
        /// <summary>   The angle. </summary>
		float xAngle;
        /// <summary>   The delta y coordinate angle. </summary>
		float deltaYAngle;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			characterTransform = transform;
			Cursor.lockState = CursorLockMode.Locked;

			if(!worldCamera) {
				Debug.LogError("Please assign 'world_camera' in the inspector, fps controller will not work.");
				enabled = false;

				return;
			}
		}

        /// <summary>   Fixed update. </summary>
        ///
     

		void FixedUpdate() {
			// Modify the angle based on the user input
			xAngle += Input.GetAxis("Mouse Y") * sensitivity * (invert ? -1 : 1);
			xAngle = Mathf.Clamp(xAngle, minTiltAngle, maxTiltAngle);
			deltaYAngle = Input.GetAxis("Mouse X") * sensitivity;

			characterTransform.rotation *= Quaternion.Euler(Vector3.up * deltaYAngle);
			cameraTargetRotation = Quaternion.Euler (-xAngle, 0f, 0f);

			// Rotate the camera 
			worldCamera.localRotation = smooth ? Quaternion.Lerp(worldCamera.localRotation, cameraTargetRotation, Time.fixedDeltaTime * smoothFactor) : cameraTargetRotation;
		}
	}
}
