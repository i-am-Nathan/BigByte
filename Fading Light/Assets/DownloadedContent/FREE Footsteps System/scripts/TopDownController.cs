﻿// file:	Assets\DownloadedContent\FREE Footsteps System\scripts\TopDownController.cs
//
// summary:	Implements the top down controller class

using UnityEngine;
using System.Collections;

namespace Footsteps {

    /// <summary>   A controller for handling top downs. </summary>
    ///
 

	[RequireComponent(typeof(Rigidbody), typeof(Animator))]
	public class TopDownController : MonoBehaviour {

        /// <summary>   Gets the camera pivot. </summary>
        ///
        /// <value> The camera pivot. </value>

		[SerializeField] Transform cameraPivot;
        /// <summary>   The jog speed. </summary>
		[SerializeField] float jogSpeed = 5f;
        /// <summary>   The rotation speed. </summary>
		[SerializeField] float rotationSpeed = 270f;
        /// <summary>   The turning on spot rotation speed. </summary>
		[SerializeField] float turningOnSpotRotationSpeed = 360f;

        /// <summary>   this transform. </summary>
		Transform thisTransform;
        /// <summary>   this animator. </summary>
		Animator thisAnimator;
        /// <summary>   this rigidbody. </summary>
		Rigidbody thisRigidbody;

        /// <summary>   Information describing the current locomotion. </summary>
		AnimatorStateInfo currentLocomotionInfo;
        /// <summary>   Target rotation. </summary>
		Quaternion targetRotation;
        /// <summary>   The movement direction. </summary>
		Vector3 movementDirection;
        /// <summary>   The directional input. </summary>
		Vector2 directionalInput;
        /// <summary>   The move speed. </summary>
		float moveSpeed;
        /// <summary>   True to turning on spot. </summary>
		bool turningOnSpot;
        /// <summary>   True to move. </summary>
		bool move;

        /// <summary>   Starts this object. </summary>
        ///
     

		void Start() {
			thisTransform = transform;
			thisAnimator = GetComponent<Animator>();
			thisRigidbody = GetComponent<Rigidbody>();

			if(!thisAnimator || !thisRigidbody) {
				Debug.LogError("Please assign both a rigidbody and an animator to this gameobject, top down controller will not function.");
				enabled = false;
			}
		}

        /// <summary>   Fixed update. </summary>
        ///
     

		void FixedUpdate() {
			UpdateAnimator();
			RotateCharacter();
			MoveCharacter();
			print(directionalInput);
		}

        /// <summary>   Updates the animator. </summary>
        ///
     

		void UpdateAnimator() {
			currentLocomotionInfo = thisAnimator.GetCurrentAnimatorStateInfo(0);

			// Get player input
			directionalInput.x = Input.GetAxisRaw("Horizontal");
			directionalInput.y = Input.GetAxisRaw("Vertical");
			moveSpeed = Mathf.Clamp01(directionalInput.magnitude);
			moveSpeed += (moveSpeed > 0f ? (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f) : 0f);
			move = Input.GetButton("Horizontal") || Input.GetButton("Vertical");

			// Handle the locomotion animations
			thisAnimator.SetFloat("move_speed", moveSpeed, 0.3f, Time.fixedDeltaTime);
			thisAnimator.SetBool("move", move);
		}

        /// <summary>   Move character. </summary>
        ///
     

		void MoveCharacter() {
			Vector3 velocity = thisTransform.forward * moveSpeed * jogSpeed;
			velocity.y = thisRigidbody.velocity.y;
			thisRigidbody.velocity = velocity;
		}

        /// <summary>   Rotate character. </summary>
        ///
     

		void RotateCharacter() {
			movementDirection = cameraPivot.right * directionalInput.x + cameraPivot.forward * directionalInput.y;
			bool inIdle = currentLocomotionInfo.IsName("idle");
			float deltaAngle = 0f;
			float targetRotationSpeed = rotationSpeed;

			if(turningOnSpot) targetRotationSpeed = turningOnSpotRotationSpeed;

			if(inIdle) {
				Vector3 targetDirection = new Vector3(movementDirection.x, 0f, movementDirection.z);
				deltaAngle = Vector3.Angle(targetDirection, transform.forward);
				float angleSign = Mathf.Sign(Vector3.Cross(transform.forward, targetDirection).y);
				deltaAngle *= angleSign;
			}

			turningOnSpot = Mathf.Abs(deltaAngle) > 30f && inIdle;

			if(movementDirection != Vector3.zero) {
				targetRotation = Quaternion.LookRotation(movementDirection);
				thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, targetRotation, Time.deltaTime * targetRotationSpeed);
			}
		}
	}
}
