// file:	Assets\UnityTestTools\Assertions\Assertions.cs
//
// summary:	Implements the assertions class

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityTest
{
    /// <summary>   An assertions. </summary>
    ///
 

    public static class Assertions
    {
        /// <summary>   Check assertions. </summary>
        ///
     

        public static void CheckAssertions()
        {
            var assertions = Object.FindObjectsOfType(typeof(AssertionComponent)) as AssertionComponent[];
            CheckAssertions(assertions);
        }

        /// <summary>   Check assertions. </summary>
        ///
     
        ///
        /// <param name="assertion">    The assertion. </param>

        public static void CheckAssertions(AssertionComponent assertion)
        {
            CheckAssertions(new[] {assertion});
        }

        /// <summary>   Check assertions. </summary>
        ///
     
        ///
        /// <param name="gameObject">   The game object. </param>

        public static void CheckAssertions(GameObject gameObject)
        {
            CheckAssertions(gameObject.GetComponents<AssertionComponent>());
        }

        /// <summary>   Check assertions. </summary>
        ///
     
        ///
        /// <param name="assertions">   The assertions. </param>

        public static void CheckAssertions(AssertionComponent[] assertions)
        {
            if (!Debug.isDebugBuild)
                return;
            foreach (var assertion in assertions)
            {
                assertion.checksPerformed++;
                var result = assertion.Action.Compare();
                if (!result)
                {
                    assertion.hasFailed = true;
                    assertion.Action.Fail(assertion);
                }
            }
        }
    }
}
