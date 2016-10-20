// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\CameraFollow.cs
//
// summary:	Implements the camera follow class

using UnityEngine;
using System.Collections;

namespace Footsteps {

    /// <summary>   A camera follow. </summary>
    ///
 

	public class CameraFollow : MonoBehaviour {

        /// <summary>   Gets the Target for the. </summary>
        ///
        /// <value> The target. </value>

		[SerializeField] Transform target;
        /// <summary>   The follow linearly interpolate factor. </summary>
		[SerializeField] float followLerpFactor = 5f;

        /// <summary>   this transform. </summary>
		Transform thisTransform;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			if(!target) enabled = false;

			thisTransform = transform;
		}

        /// <summary>   Fixed update. </summary>
        ///
     

		void FixedUpdate() {
			thisTransform.position = Vector3.Lerp(thisTransform.position, target.position, Time.deltaTime * followLerpFactor);
		}
	}
}
