// - AUTHOR : Pavel Cristian.
// - WHERE SHOULD BE ATTACHED : This script should be attached on the main root of the character, 
//	 on the GameObject the Rigidbody / CharacterController script is attached.
// - PURPOSE OF THE SCRIPT : The purpose of this script is to gather data from the ground below the character and use the
//   data to find a user-defined sound for the type of ground found.

// DISCLAIMER : THIS SCRIPT CAN BE USED IN ANY WAY, MENTIONING MY WORK WILL BE GREATLY APPRECIATED BUT NOT REQUIRED.

using UnityEngine;

namespace Footsteps {

    /// <summary>   Values that represent triggered bies. </summary>
    ///
 

	public enum TriggeredBy {
        /// <summary>
        /// The footstep sound will be played when the physical foot collides with the ground.
        /// </summary>
		COLLISION_DETECTION,
        /// <summary>
        /// The footstep sound will be played after the character has traveled a certain distance.
        /// </summary>
		TRAVELED_DISTANCE
	}

    /// <summary>   Values that represent controller types. </summary>
    ///
 

	public enum ControllerType {
        /// <summary>   An enum constant representing the rigidbody option. </summary>
		RIGIDBODY,
        /// <summary>   An enum constant representing the character controller option. </summary>
		CHARACTER_CONTROLLER
	}

    /// <summary>   A character footsteps. </summary>
    ///
 

	public class CharacterFootsteps : MonoBehaviour {

        /// <summary>   Gets the amount to triggered by. </summary>
        ///
        /// <value> Amount to triggered by. </value>

		[Tooltip("The method of triggering footsteps.")]
		[SerializeField] TriggeredBy triggeredBy;

        /// <summary>   The distance between steps. </summary>
		[Tooltip("This is used to determine what distance has to be traveled in order to play the footstep sound.")]
		[SerializeField] float distanceBetweenSteps = 1.8f;

        /// <summary>   Gets the type of the controller. </summary>
        ///
        /// <value> The type of the controller. </value>

		[Tooltip("To know how much the character moved, a reference to a rigidbody / character controller is needed.")]
		[SerializeField] ControllerType controllerType;

        /// <summary>   Gets the character rigidbody. </summary>
        ///
        /// <value> The character rigidbody. </value>

		[SerializeField] Rigidbody characterRigidbody;

        /// <summary>   Gets the character controller. </summary>
        ///
        /// <value> The character controller. </value>

		[SerializeField] CharacterController characterController;

        /// <summary>   Gets the audio source. </summary>
        ///
        /// <value> The audio source. </value>

		[Tooltip("You need an audio source to play a footstep sound.")]
		[SerializeField] AudioSource audioSource;

		// Random volume between this limits
        /// <summary>   The minimum volume. </summary>
		[SerializeField] float minVolume = 0.3f;
        /// <summary>   The maximum volume. </summary>
		[SerializeField] float maxVolume = 0.5f;

        /// <summary>   The debug mode. </summary>
		[Tooltip("If this is enabled, you can see how far the script will check for ground, and the radius of the check.")]
		[SerializeField] bool debugMode = true;

        /// <summary>   Height of the ground check. </summary>
		[Tooltip("How high, relative to the character's pivot point the start of the ray is.")]
		[SerializeField] float groundCheckHeight = 0.5f;

        /// <summary>   The ground check radius. </summary>
		[Tooltip("What is the radius of the ray.")]
		[SerializeField] float groundCheckRadius = 0.5f;

        /// <summary>   The ground check distance. </summary>
		[Tooltip("How far the ray is casted.")]
		[SerializeField] float groundCheckDistance = 0.3f;

        /// <summary>   Gets the ground layers. </summary>
        ///
        /// <value> The ground layers. </value>

		[Tooltip("What are the layers that should be taken into account when checking for ground.")]
		[SerializeField] LayerMask groundLayers;

        /// <summary>   this transform. </summary>
		Transform thisTransform;
        /// <summary>   Information describing the current ground. </summary>
		RaycastHit currentGroundInfo;
        /// <summary>   The step cycle progress. </summary>
		float stepCycleProgress;
        /// <summary>   The last play time. </summary>
		float lastPlayTime;
        /// <summary>   True if previously grounded. </summary>
		bool previouslyGrounded;
        /// <summary>   True if this object is grounded. </summary>
		bool isGrounded;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			if(groundLayers.value == 0) {
				groundLayers = 1;
			}

			thisTransform = transform;
			string errorMessage = "";

			if(!audioSource) errorMessage = "No audio source assigned in the inspector, footsteps cannot be played";
			else if(triggeredBy == TriggeredBy.TRAVELED_DISTANCE && !characterRigidbody && !characterController) errorMessage = "Please assign a Rigidbody or CharacterController component in the inspector, footsteps cannot be played";
			else if(!FindObjectOfType<SurfaceManager>()) errorMessage = "Please create a Footstep Database, otherwise footsteps cannot be played, you can create a database" +
																		" by clicking 'FootstepsCreator' in the main menu";

			if(errorMessage != "") {
				Debug.LogError(errorMessage);
				enabled = false;
			}
		}

        /// <summary>   Updates this object. </summary>
        ///
     

		void Update() {
			CheckGround();

			if(triggeredBy == TriggeredBy.TRAVELED_DISTANCE) {
				float speed = (characterController ? characterController.velocity : characterRigidbody.velocity).magnitude;

				if(isGrounded) {
					// Advance the step cycle only if the character is grounded.
					AdvanceStepCycle(speed * Time.deltaTime);
				}
			}
		}

        /// <summary>   Try play footstep. </summary>
        ///
     

		public void TryPlayFootstep() {
			if(isGrounded) {
				PlayFootstep();
			}
		}

        /// <summary>   Play land sound. </summary>
        ///
     

		void PlayLandSound() {
			audioSource.PlayOneShot(SurfaceManager.singleton.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point));
		}

        /// <summary>   Advance step cycle. </summary>
        ///
     
        ///
        /// <param name="increment">    Amount to increment by. </param>

		void AdvanceStepCycle(float increment) {
			stepCycleProgress += increment;

			if(stepCycleProgress > distanceBetweenSteps) {
				stepCycleProgress = 0f;
				PlayFootstep();
			}
		}

        /// <summary>   Play footstep. </summary>
        ///
     

		void PlayFootstep() {
			AudioClip randomFootstep = SurfaceManager.singleton.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);
			float randomVolume = Random.Range(minVolume, maxVolume);

			if(randomFootstep) {
				audioSource.PlayOneShot(randomFootstep, randomVolume);
			}
		}

        /// <summary>   Executes the draw gizmos action. </summary>
        ///
     

		void OnDrawGizmos() {
			if(debugMode) {
				Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
				Gizmos.color = Color.red;
				Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
			}
		}

        /// <summary>   Check ground. </summary>
        ///
     

		void CheckGround() {
			previouslyGrounded = isGrounded;
			Ray ray = new Ray(thisTransform.position + Vector3.up * groundCheckHeight, Vector3.down);

			if(Physics.SphereCast(ray, groundCheckRadius, out currentGroundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore)) {
				isGrounded = true;
			}
			else {
				isGrounded = false;
			}

			if(!previouslyGrounded && isGrounded) {
				PlayLandSound();
			}
			// print(isGrounded);
		}
	}
}
