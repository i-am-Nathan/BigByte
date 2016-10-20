// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\FirstPersonController.cs
//
// summary:	Implements the first person controller class

using UnityEngine;

namespace Footsteps {

    /// <summary>   A controller for handling first persons. </summary>
    ///
 

	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour {

        /// <summary>   Gets or sets the velocity. </summary>
        ///
        /// <value> The velocity. </value>

		public Vector3 velocity { get; private set; }

        /// <summary>   Gets or sets a value indicating whether this object is jumping. </summary>
        ///
        /// <value> True if this object is jumping, false if not. </value>

		public bool isJumping { get; private set; }

        /// <summary>   Gets or sets a value indicating whether this object is grounded. </summary>
        ///
        /// <value> True if this object is grounded, false if not. </value>

		public bool isGrounded { get; private set; }

        /// <summary>   Gets or sets a value indicating whether the previously grounded. </summary>
        ///
        /// <value> True if previously grounded, false if not. </value>

		public bool previouslyGrounded { get; private set; }

        /// <summary>   The forward speed. </summary>
		[Header("Movement Settings")]
		[SerializeField] float forwardSpeed = 5f;
        /// <summary>   The backward speed. </summary>
		[SerializeField] float backwardSpeed = 4f;
        /// <summary>   The strafe speed. </summary>
		[SerializeField] float strafeSpeed = 5f;
        /// <summary>   The run multiplier. </summary>
		[SerializeField] float runMultiplier = 1.8f;
        /// <summary>   The acceleration. </summary>
		[SerializeField] float acceleration = 18f;
        /// <summary>   The deceleration. </summary>
		[SerializeField] float deceleration = 12f;
        /// <summary>   The movement energy. </summary>
		[SerializeField] float movementEnergy = 6f;

        /// <summary>   The jump base speed. </summary>
		[Header("Jump Settings")]
		[SerializeField] float jumpBaseSpeed = 5f;
        /// <summary>   The jump extra speed. </summary>
		[SerializeField] float jumpExtraSpeed = 1f;
        /// <summary>   The gravity. </summary>
		[SerializeField] float gravity = -20f;
        /// <summary>   The air control. </summary>
		[SerializeField] [Range(0f, 1f)] float airControl = 0.2f;

        /// <summary>   Gets the world camera. </summary>
        ///
        /// <value> The world camera. </value>

		[Header("References")]
		[SerializeField] Transform worldCamera;

		// References
        /// <summary>   this transform. </summary>
		Transform thisTransform;
        /// <summary>   this character controller. </summary>
		CharacterController thisCharacterController;

		// System
        /// <summary>   Target direction. </summary>
		Vector3 targetDirection;
        /// <summary>   The movement input. </summary>
		Vector2 movementInput;
        /// <summary>   Target speed. </summary>
		float targetSpeed;
        /// <summary>   The current speed. </summary>
		float currentSpeed;
        /// <summary>   The remained extra jump speed. </summary>
		float remainedExtraJumpSpeed;

		// States
        /// <summary>   True to jump. </summary>
		bool jump;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			thisTransform = transform;
			thisCharacterController = GetComponent<CharacterController>();

			// Searching for potential errors
			string errorMessage = "none";

			if(!thisCharacterController) errorMessage = "The script 'CharacterMotor' needs a CharacterController to work, none was found, this script will not work.";
			else if(!worldCamera) errorMessage = "Please assign 'world_camera' in the inspector, fps controller will not work.";

			if(errorMessage != "none") {
				enabled = false;
				Debug.LogError(errorMessage);

				return;
			}
		}

        /// <summary>   Updates this object. </summary>
        ///
     

		void Update() {
			HandleUserInput();
		}

        /// <summary>   Fixed update. </summary>
        ///
     

		void FixedUpdate() {
			previouslyGrounded = isGrounded;
			isGrounded = thisCharacterController.isGrounded;
			velocity = thisCharacterController.velocity;

			float accelRate = movementInput.sqrMagnitude > 0f ? acceleration : deceleration;
			float controlModifier = (isGrounded ? 1f : airControl);

			currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (Time.fixedDeltaTime * accelRate * controlModifier));
			Vector3 targetVelocity = targetDirection.normalized * currentSpeed;
			targetVelocity.y = thisCharacterController.velocity.y + gravity * Time.fixedDeltaTime;

			if(jump && isGrounded) {
				// Jumping
				targetVelocity = new Vector3(targetVelocity.x, jumpBaseSpeed, targetVelocity.z);
				isJumping = true;
			}
			else if(isGrounded && !previouslyGrounded) {
				if(isJumping) isJumping = false;

				remainedExtraJumpSpeed = jumpExtraSpeed;
			} 

			if(jump && thisCharacterController.velocity.y > 0f) {
				float jumpSpeedIncrement = remainedExtraJumpSpeed * Time.fixedDeltaTime;
				remainedExtraJumpSpeed -= jumpSpeedIncrement;

				if(jumpSpeedIncrement > 0f) {
					targetVelocity.y += jumpSpeedIncrement;
				}
			}

			Vector3 vel = Vector3.MoveTowards(thisCharacterController.velocity, targetVelocity, Time.fixedDeltaTime * movementEnergy);
			vel.y = targetVelocity.y;
			thisCharacterController.Move(vel * Time.fixedDeltaTime);

			jump = false;
		}

        /// <summary>   Handles the user input. </summary>
        ///
     

		void HandleUserInput() {
			float h = Input.GetAxisRaw("Horizontal");
			float v = Input.GetAxisRaw("Vertical");

			movementInput = new Vector2(h, v);

			jump = Input.GetButton("Jump");
			targetSpeed = 0f;

			if(movementInput.x > 0f || movementInput.x < 0f) {
				targetSpeed = strafeSpeed;
			}

			if(movementInput.y < 0f) {
				targetSpeed = backwardSpeed;
			}

			if(movementInput.y > 0f) {
				targetSpeed = forwardSpeed;
			}

			if(Input.GetKey(KeyCode.LeftShift)) {
				targetSpeed *= runMultiplier;
			}

			if(Mathf.Abs(h) != 0f || Mathf.Abs(v) != 0f) {
				targetDirection = thisTransform.forward * movementInput.y + thisTransform.right * movementInput.x;
			}
		}
	}
}