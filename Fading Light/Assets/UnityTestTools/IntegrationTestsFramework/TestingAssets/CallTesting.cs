// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestingAssets\CallTesting.cs
//
// summary:	Implements the call testing class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A call testing. </summary>
    ///
 

    public class CallTesting : MonoBehaviour
    {
        /// <summary>   Values that represent functions. </summary>
        ///
     

        public enum Functions
        {
            /// <summary>   An enum constant representing the call after seconds option. </summary>
            CallAfterSeconds,
            /// <summary>   An enum constant representing the call after frames option. </summary>
            CallAfterFrames,
            /// <summary>   An enum constant representing the start option. </summary>
            Start,
            /// <summary>   An enum constant representing the update option. </summary>
            Update,
            /// <summary>   An enum constant representing the fixed update option. </summary>
            FixedUpdate,
            /// <summary>   An enum constant representing the late update option. </summary>
            LateUpdate,
            /// <summary>   An enum constant representing the on destroy option. </summary>
            OnDestroy,
            /// <summary>   An enum constant representing the on enable option. </summary>
            OnEnable,
            /// <summary>   An enum constant representing the on disable option. </summary>
            OnDisable,
            /// <summary>   An enum constant representing the on controller collider hit option. </summary>
            OnControllerColliderHit,
            /// <summary>   An enum constant representing the on particle collision option. </summary>
            OnParticleCollision,
            /// <summary>   An enum constant representing the on joint break option. </summary>
            OnJointBreak,
            /// <summary>   An enum constant representing the on became invisible option. </summary>
            OnBecameInvisible,
            /// <summary>   An enum constant representing the on became visible option. </summary>
            OnBecameVisible,
            /// <summary>   An enum constant representing the on trigger enter option. </summary>
            OnTriggerEnter,
            /// <summary>   An enum constant representing the on trigger exit option. </summary>
            OnTriggerExit,
            /// <summary>   An enum constant representing the on trigger stay option. </summary>
            OnTriggerStay,
            /// <summary>   An enum constant representing the on collision enter option. </summary>
            OnCollisionEnter,
            /// <summary>   An enum constant representing the on collision exit option. </summary>
            OnCollisionExit,
            /// <summary>   An enum constant representing the on collision stay option. </summary>
            OnCollisionStay,
            /// <summary>   An enum constant representing the on trigger enter 2D option. </summary>
            OnTriggerEnter2D,
            /// <summary>   An enum constant representing the on trigger exit 2D option. </summary>
            OnTriggerExit2D,
            /// <summary>   An enum constant representing the on trigger stay 2D option. </summary>
            OnTriggerStay2D,
            /// <summary>   An enum constant representing the on collision enter 2D option. </summary>
            OnCollisionEnter2D,
            /// <summary>   An enum constant representing the on collision exit 2D option. </summary>
            OnCollisionExit2D,
            /// <summary>   An enum constant representing the on collision stay 2D option. </summary>
            OnCollisionStay2D,
        }

        /// <summary>   Values that represent methods. </summary>
        ///
     

        public enum Method
        {
            /// <summary>   An enum constant representing the pass option. </summary>
            Pass,
            /// <summary>   An enum constant representing the fail option. </summary>
            Fail
        }

        /// <summary>   The after frames. </summary>
        public int afterFrames = 0;
        /// <summary>   The after in seconds. </summary>
        public float afterSeconds = 0.0f;
        /// <summary>   The call on method. </summary>
        public Functions callOnMethod = Functions.Start;

        /// <summary>   The method to call. </summary>
        public Method methodToCall;
        /// <summary>   The start frame. </summary>
        private int m_StartFrame;
        /// <summary>   The start time. </summary>
        private float m_StartTime;

        /// <summary>   Try to call testing. </summary>
        ///
     
        ///
        /// <param name="invokingMethod">   The invoking method. </param>

        private void TryToCallTesting(Functions invokingMethod)
        {
            if (invokingMethod == callOnMethod)
            {
                if (methodToCall == Method.Pass)
                    IntegrationTest.Pass(gameObject);
                else
                    IntegrationTest.Fail(gameObject);

                afterFrames = 0;
                afterSeconds = 0.0f;
                m_StartTime = float.PositiveInfinity;
                m_StartFrame = int.MinValue;
            }
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        public void Start()
        {
            m_StartTime = Time.time;
            m_StartFrame = afterFrames;
            TryToCallTesting(Functions.Start);
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        public void Update()
        {
            TryToCallTesting(Functions.Update);
            CallAfterSeconds();
            CallAfterFrames();
        }

        /// <summary>   Call after frames. </summary>
        ///
     

        private void CallAfterFrames()
        {
            if (afterFrames > 0 && (m_StartFrame + afterFrames) <= Time.frameCount)
                TryToCallTesting(Functions.CallAfterFrames);
        }

        /// <summary>   Call after seconds. </summary>
        ///
     

        private void CallAfterSeconds()
        {
            if ((m_StartTime + afterSeconds) <= Time.time)
                TryToCallTesting(Functions.CallAfterSeconds);
        }

        /// <summary>   Executes the disable action. </summary>
        ///
     

        public void OnDisable()
        {
            TryToCallTesting(Functions.OnDisable);
        }

        /// <summary>   Executes the enable action. </summary>
        ///
     

        public void OnEnable()
        {
            TryToCallTesting(Functions.OnEnable);
        }

        /// <summary>   Executes the destroy action. </summary>
        ///
     

        public void OnDestroy()
        {
            TryToCallTesting(Functions.OnDestroy);
        }

        /// <summary>   Fixed update. </summary>
        ///
     

        public void FixedUpdate()
        {
            TryToCallTesting(Functions.FixedUpdate);
        }

        /// <summary>   Late update. </summary>
        ///
     

        public void LateUpdate()
        {
            TryToCallTesting(Functions.LateUpdate);
        }

        /// <summary>   Executes the controller collider hit action. </summary>
        ///
     

        public void OnControllerColliderHit()
        {
            TryToCallTesting(Functions.OnControllerColliderHit);
        }

        /// <summary>   Executes the particle collision action. </summary>
        ///
     

        public void OnParticleCollision()
        {
            TryToCallTesting(Functions.OnParticleCollision);
        }

        /// <summary>   Executes the joint break action. </summary>
        ///
     

        public void OnJointBreak()
        {
            TryToCallTesting(Functions.OnJointBreak);
        }

        /// <summary>   Executes the became invisible action. </summary>
        ///
     

        public void OnBecameInvisible()
        {
            TryToCallTesting(Functions.OnBecameInvisible);
        }

        /// <summary>   Executes the became visible action. </summary>
        ///
     

        public void OnBecameVisible()
        {
            TryToCallTesting(Functions.OnBecameVisible);
        }

        /// <summary>   Executes the trigger enter action. </summary>
        ///
     

        public void OnTriggerEnter()
        {
            TryToCallTesting(Functions.OnTriggerEnter);
        }

        /// <summary>   Executes the trigger exit action. </summary>
        ///
     

        public void OnTriggerExit()
        {
            TryToCallTesting(Functions.OnTriggerExit);
        }

        /// <summary>   Executes the trigger stay action. </summary>
        ///
     

        public void OnTriggerStay()
        {
            TryToCallTesting(Functions.OnTriggerStay);
        }

        /// <summary>   Executes the collision enter action. </summary>
        ///
     

        public void OnCollisionEnter()
        {
            TryToCallTesting(Functions.OnCollisionEnter);
        }

        /// <summary>   Executes the collision exit action. </summary>
        ///
     

        public void OnCollisionExit()
        {
            TryToCallTesting(Functions.OnCollisionExit);
        }

        /// <summary>   Executes the collision stay action. </summary>
        ///
     

        public void OnCollisionStay()
        {
            TryToCallTesting(Functions.OnCollisionStay);
        }

        /// <summary>   Executes the trigger enter 2D action. </summary>
        ///
     

        public void OnTriggerEnter2D()
        {
            TryToCallTesting(Functions.OnTriggerEnter2D);
        }

        /// <summary>   Executes the trigger exit 2D action. </summary>
        ///
     

        public void OnTriggerExit2D()
        {
            TryToCallTesting(Functions.OnTriggerExit2D);
        }

        /// <summary>   Executes the trigger stay 2D action. </summary>
        ///
     

        public void OnTriggerStay2D()
        {
            TryToCallTesting(Functions.OnTriggerStay2D);
        }

        /// <summary>   Executes the collision enter 2D action. </summary>
        ///
     

        public void OnCollisionEnter2D()
        {
            TryToCallTesting(Functions.OnCollisionEnter2D);
        }

        /// <summary>   Executes the collision exit 2D action. </summary>
        ///
     

        public void OnCollisionExit2D()
        {
            TryToCallTesting(Functions.OnCollisionExit2D);
        }

        /// <summary>   Executes the collision stay 2D action. </summary>
        ///
     

        public void OnCollisionStay2D()
        {
            TryToCallTesting(Functions.OnCollisionStay2D);
        }
    }
}
