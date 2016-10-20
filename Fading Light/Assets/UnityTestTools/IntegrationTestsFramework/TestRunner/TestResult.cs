// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\TestResult.cs
//
// summary:	Implements the test result class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   (Serializable) encapsulates the result of a test. </summary>
    ///
 

    [Serializable]
    public class TestResult : ITestResult, IComparable<TestResult>
    {
        /// <summary>   The go. </summary>
        private readonly GameObject m_Go;
        /// <summary>   The name. </summary>
        private string m_Name;
        /// <summary>   Type of the result. </summary>
        public ResultType resultType = ResultType.NotRun;
        /// <summary>   The duration. </summary>
        public double duration;
        /// <summary>   The messages. </summary>
        public string messages;
        /// <summary>   The stacktrace. </summary>
        public string stacktrace;
        /// <summary>   The identifier. </summary>
        public string id;
        /// <summary>   True to dynamic test. </summary>
        public bool dynamicTest;

        /// <summary>   The test component. </summary>
        public TestComponent TestComponent;

        /// <summary>   Gets the game object. </summary>
        ///
        /// <value> The game object. </value>

        public GameObject GameObject
        {
            get { return m_Go; }
        }

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="testComponent">    The test component. </param>

        public TestResult(TestComponent testComponent)
        {
            TestComponent = testComponent;
            m_Go = testComponent.gameObject;
            id = testComponent.gameObject.GetInstanceID().ToString();
            dynamicTest = testComponent.dynamic;

            if (m_Go != null) m_Name = m_Go.name;

            if (dynamicTest)
                id = testComponent.dynamicTypeName;
        }

        /// <summary>   Updates the given oldResult. </summary>
        ///
     
        ///
        /// <param name="oldResult">    The old result. </param>

        public void Update(TestResult oldResult)
        {
            resultType = oldResult.resultType;
            duration = oldResult.duration;
            messages = oldResult.messages;
            stacktrace = oldResult.stacktrace;
        }

        /// <summary>   Values that represent result types. </summary>
        ///
     

        public enum ResultType
        {
            /// <summary>   An enum constant representing the success option. </summary>
            Success,
            /// <summary>   An enum constant representing the failed option. </summary>
            Failed,
            /// <summary>   An enum constant representing the timeout option. </summary>
            Timeout,
            /// <summary>   An enum constant representing the not run option. </summary>
            NotRun,
            /// <summary>   An enum constant representing the failed exception option. </summary>
            FailedException,
            /// <summary>   An enum constant representing the ignored option. </summary>
            Ignored
        }

        /// <summary>   Resets this object. </summary>
        ///
     

        public void Reset()
        {
            resultType = ResultType.NotRun;
            duration = 0f;
            messages = "";
            stacktrace = "";
        }

        #region ITestResult implementation

        /// <summary>   Gets the state of the result. </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <value> The result state. </value>

        public TestResultState ResultState {
            get
            {
                switch (resultType)
                {
                    case ResultType.Success: return TestResultState.Success;
                    case ResultType.Failed: return TestResultState.Failure;
                    case ResultType.FailedException: return TestResultState.Error;
                    case ResultType.Ignored: return TestResultState.Ignored;
                    case ResultType.NotRun: return TestResultState.Skipped;
                    case ResultType.Timeout: return TestResultState.Cancelled;
                    default: throw new Exception();
                }
            }
        }

        /// <summary>   Gets the message. </summary>
        ///
        /// <value> The message. </value>

        public string Message { get { return messages; } }

        /// <summary>   Gets the logs. </summary>
        ///
        /// <value> The logs. </value>

        public string Logs { get { return null; } }

        /// <summary>   Gets a value indicating whether the executed. </summary>
        ///
        /// <value> True if executed, false if not. </value>

        public bool Executed { get { return resultType != ResultType.NotRun; } }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>

        public string Name { get { if (m_Go != null) m_Name = m_Go.name; return m_Name; } }

        /// <summary>   Gets the identifier. </summary>
        ///
        /// <value> The identifier. </value>

        public string Id { get { return id; } }

        /// <summary>   Gets a value indicating whether this object is success. </summary>
        ///
        /// <value> True if this object is success, false if not. </value>

        public bool IsSuccess { get { return resultType == ResultType.Success; } }

        /// <summary>   Gets a value indicating whether this object is timeout. </summary>
        ///
        /// <value> True if this object is timeout, false if not. </value>

        public bool IsTimeout { get { return resultType == ResultType.Timeout; } }

        /// <summary>   Gets the duration. </summary>
        ///
        /// <value> The duration. </value>

        public double Duration { get { return duration; } }

        /// <summary>   Gets the stack trace. </summary>
        ///
        /// <value> The stack trace. </value>

        public string StackTrace { get { return stacktrace; } }

        /// <summary>   Gets the name of the full. </summary>
        ///
        /// <value> The name of the full. </value>

        public string FullName {
            get
            {
                var fullName = Name;
                if (m_Go != null)
                {
                    var tempGo = m_Go.transform.parent;
                    while (tempGo != null)
                    {
                        fullName = tempGo.name + "." + fullName;
                        tempGo = tempGo.transform.parent;
                    }
                }
                return fullName;
            }
        }

        /// <summary>   Gets a value indicating whether this object is ignored. </summary>
        ///
        /// <value> True if this object is ignored, false if not. </value>

        public bool IsIgnored { get { return resultType == ResultType.Ignored; } }

        /// <summary>   Gets a value indicating whether this object is failure. </summary>
        ///
        /// <value> True if this object is failure, false if not. </value>

        public bool IsFailure
        {
            get
            {
                return resultType == ResultType.Failed
                       || resultType == ResultType.FailedException
                       || resultType == ResultType.Timeout;
            }
        }
        #endregion

        #region IComparable, GetHashCode and Equals implementation

        /// <summary>   Calculates a hash code for this object. </summary>
        ///
     
        ///
        /// <returns>   A hash code for this object. </returns>

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        /// <summary>
        /// Compares this TestResult object to another to determine their relative ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="other">    Another instance to compare. </param>
        ///
        /// <returns>
        /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
        /// greater.
        /// </returns>

        public int CompareTo(TestResult other)
        {
            var result = Name.CompareTo(other.Name);
            if (result == 0)
                result = m_Go.GetInstanceID().CompareTo(other.m_Go.GetInstanceID());
            return result;
        }

        /// <summary>   Tests if this object is considered equal to another. </summary>
        ///
     
        ///
        /// <param name="obj">  The object to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>

        public override bool Equals(object obj)
        {
            if (obj is TestResult)
                return GetHashCode() == obj.GetHashCode();
            return base.Equals(obj);
        }
        #endregion
    }
}
