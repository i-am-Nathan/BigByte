// file:	Assets\UnityTestTools\Assertions\AssertionException.cs
//
// summary:	Implements the assertion exception class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   Exception for signalling assertion errors. </summary>
    ///
 

    public class AssertionException : Exception
    {
        /// <summary>   The assertion. </summary>
        private readonly AssertionComponent m_Assertion;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="assertion">    The assertion. </param>

        public AssertionException(AssertionComponent assertion) : base(assertion.Action.GetFailureMessage())
        {
            m_Assertion = assertion;
        }

        /// <summary>   Gets the stack trace. </summary>
        ///
        /// <value> The stack trace. </value>

        public override string StackTrace
        {
            get
            {
                return "Created in " + m_Assertion.GetCreationLocation();
            }
        }
    }
}
