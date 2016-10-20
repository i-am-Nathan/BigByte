// file:	Assets\UnityTestTools\Assertions\CheckMethod.cs
//
// summary:	Implements the check method class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A bit-field of flags for specifying check methods. </summary>
    ///
 

    [Flags]
    public enum CheckMethod
    {
        /// <summary>   A binary constant representing the after period of time flag. </summary>
        AfterPeriodOfTime       = 1 << 0,
            /// <summary>   A binary constant representing the start flag. </summary>
            Start                   = 1 << 1,
            /// <summary>   A binary constant representing the update flag. </summary>
            Update                  = 1 << 2,
            /// <summary>   A binary constant representing the fixed update flag. </summary>
            FixedUpdate             = 1 << 3,
            /// <summary>   A binary constant representing the late update flag. </summary>
            LateUpdate              = 1 << 4,
            /// <summary>   A binary constant representing the on destroy flag. </summary>
            OnDestroy               = 1 << 5,
            /// <summary>   A binary constant representing the on enable flag. </summary>
            OnEnable                = 1 << 6,
            /// <summary>   A binary constant representing the on disable flag. </summary>
            OnDisable               = 1 << 7,
            /// <summary>   A binary constant representing the on controller collider hit flag. </summary>
            OnControllerColliderHit = 1 << 8,
            /// <summary>   A binary constant representing the on particle collision flag. </summary>
            OnParticleCollision     = 1 << 9,
            /// <summary>   A binary constant representing the on joint break flag. </summary>
            OnJointBreak            = 1 << 10,
            /// <summary>   A binary constant representing the on became invisible flag. </summary>
            OnBecameInvisible       = 1 << 11,
            /// <summary>   A binary constant representing the on became visible flag. </summary>
            OnBecameVisible         = 1 << 12,
            /// <summary>   A binary constant representing the on trigger enter flag. </summary>
            OnTriggerEnter          = 1 << 13,
            /// <summary>   A binary constant representing the on trigger exit flag. </summary>
            OnTriggerExit           = 1 << 14,
            /// <summary>   A binary constant representing the on trigger stay flag. </summary>
            OnTriggerStay           = 1 << 15,
            /// <summary>   A binary constant representing the on collision enter flag. </summary>
            OnCollisionEnter        = 1 << 16,
            /// <summary>   A binary constant representing the on collision exit flag. </summary>
            OnCollisionExit         = 1 << 17,
            /// <summary>   A binary constant representing the on collision stay flag. </summary>
            OnCollisionStay         = 1 << 18,
            /// <summary>   A binary constant representing the on trigger enter 2D flag. </summary>
            OnTriggerEnter2D        = 1 << 19,
            /// <summary>   A binary constant representing the on trigger exit 2D flag. </summary>
            OnTriggerExit2D         = 1 << 20,
            /// <summary>   A binary constant representing the on trigger stay 2D flag. </summary>
            OnTriggerStay2D         = 1 << 21,
            /// <summary>   A binary constant representing the on collision enter 2D flag. </summary>
            OnCollisionEnter2D      = 1 << 22,
            /// <summary>   A binary constant representing the on collision exit 2D flag. </summary>
            OnCollisionExit2D       = 1 << 23,
            /// <summary>   A binary constant representing the on collision stay 2D flag. </summary>
            OnCollisionStay2D       = 1 << 24,
    }
}
