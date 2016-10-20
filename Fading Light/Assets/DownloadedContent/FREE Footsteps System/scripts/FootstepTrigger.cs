// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\FootstepTrigger.cs
//
// summary:	Implements the footstep trigger class

using UnityEngine;
using System.Collections;

namespace Footsteps {

    /// <summary>   A footstep trigger. </summary>
    ///
 

	[RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class FootstepTrigger : MonoBehaviour {

        /// <summary>   this collider. </summary>
		Collider thisCollider;
        /// <summary>   The footsteps. </summary>
		CharacterFootsteps footsteps;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			thisCollider = GetComponent<Collider>();
			footsteps = GetComponentInParent<CharacterFootsteps>();
			Rigidbody thisRigidbody = GetComponent<Rigidbody>();

			if(thisCollider) {
				thisCollider.isTrigger = true;
				SetCollisions();
			}

			if(thisRigidbody) thisRigidbody.isKinematic = true;

			string errorMessage = "";

			if(!footsteps) errorMessage = "No 'CharacterFootsteps' script found as a parent, this footstep trigger will not work";
			else if(!thisCollider) errorMessage = "Please attach a collider marked as a trigger to this gameobject, this footstep trigger will not work";
			else if(!thisRigidbody) errorMessage = "Please attach a rigidbody to this gameobject, this footstep trigger will not work";

			if(errorMessage != "") {
				Debug.LogError(errorMessage);
				enabled = false;

				return;
			}
		}

        /// <summary>   Executes the enable action. </summary>
        ///
     

		void OnEnable() {
			SetCollisions();
		}

        /// <summary>   Executes the trigger enter action. </summary>
        ///
     
        ///
        /// <param name="other">    The other. </param>

		void OnTriggerEnter(Collider other) {
			if(footsteps) {
				footsteps.TryPlayFootstep();
			}
		}

        /// <summary>   Sets the collisions. </summary>
        ///
     

		void SetCollisions() {
			if(!footsteps) return;

			Collider[] allColliders = footsteps.GetComponentsInChildren<Collider>();

			foreach(var collider in allColliders) {
				if(collider != GetComponent<Collider>()) {
					Physics.IgnoreCollision(thisCollider, collider);
				}
			}
		}
	}
}
