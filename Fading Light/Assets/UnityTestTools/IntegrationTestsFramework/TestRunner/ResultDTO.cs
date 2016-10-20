// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\ResultDTO.cs
//
// summary:	Implements the result dto class

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTest
{
    /// <summary>   (Serializable) a result dto. </summary>
    ///
 

    [Serializable]
    public class ResultDTO
    {
        /// <summary>   Type of the message. </summary>
        public MessageType messageType;
        /// <summary>   Number of levels. </summary>
        public int levelCount;
        /// <summary>   The loaded level. </summary>
        public int loadedLevel;
        /// <summary>   Name of the loaded level. </summary>
        public string loadedLevelName;
        /// <summary>   Name of the test. </summary>
        public string testName;
        /// <summary>   The test timeout. </summary>
        public float testTimeout;
        /// <summary>   The test result. </summary>
        public ITestResult testResult;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="messageType">  Type of the message. </param>

        private ResultDTO(MessageType messageType)
        {
            this.messageType = messageType;
            levelCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            loadedLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            loadedLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        /// <summary>   Values that represent message types. </summary>
        ///
     

        public enum MessageType : byte
        {
            /// <summary>   An enum constant representing the ping option. </summary>
            Ping,
            /// <summary>   An enum constant representing the run started option. </summary>
            RunStarted,
            /// <summary>   An enum constant representing the run finished option. </summary>
            RunFinished,
            /// <summary>   An enum constant representing the test started option. </summary>
            TestStarted,
            /// <summary>   An enum constant representing the test finished option. </summary>
            TestFinished,
            /// <summary>   An enum constant representing the run interrupted option. </summary>
            RunInterrupted,
            /// <summary>   An enum constant representing all scenes finished option. </summary>
            AllScenesFinished
        }

        /// <summary>   Creates the ping. </summary>
        ///
     
        ///
        /// <returns>   The new ping. </returns>

        public static ResultDTO CreatePing()
        {
            var dto = new ResultDTO(MessageType.Ping);
            return dto;
        }

        /// <summary>   Creates run started. </summary>
        ///
     
        ///
        /// <returns>   The new run started. </returns>

        public static ResultDTO CreateRunStarted()
        {
            var dto = new ResultDTO(MessageType.RunStarted);
            return dto;
        }

        /// <summary>   Creates run finished. </summary>
        ///
     
        ///
        /// <param name="testResults">  The test results. </param>
        ///
        /// <returns>   The new run finished. </returns>

        public static ResultDTO CreateRunFinished(List<TestResult> testResults)
        {
            var dto = new ResultDTO(MessageType.RunFinished);
            return dto;
        }

        /// <summary>   Creates test started. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>
        ///
        /// <returns>   The new test started. </returns>

        public static ResultDTO CreateTestStarted(TestResult test)
        {
            var dto = new ResultDTO(MessageType.TestStarted);
            dto.testName = test.FullName;
            dto.testTimeout = test.TestComponent.timeout;
            return dto;
        }

        /// <summary>   Creates test finished. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>
        ///
        /// <returns>   The new test finished. </returns>

        public static ResultDTO CreateTestFinished(TestResult test)
        {
            var dto = new ResultDTO(MessageType.TestFinished);
            dto.testName = test.FullName;
            dto.testResult = GetSerializableTestResult(test);
            return dto;
        }

        /// <summary>   Creates all scenes finished. </summary>
        ///
     
        ///
        /// <returns>   The new all scenes finished. </returns>

        public static ResultDTO CreateAllScenesFinished()
        {
            var dto = new ResultDTO(MessageType.AllScenesFinished);
            return dto;
        }

        /// <summary>   Gets serializable test result. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>
        ///
        /// <returns>   The serializable test result. </returns>

        private static ITestResult GetSerializableTestResult(TestResult test)
        {
            var str = new SerializableTestResult();

            str.resultState = test.ResultState;
            str.message = test.messages;
            str.executed = test.Executed;
            str.name = test.Name;
            str.fullName = test.FullName;
            str.id = test.id;
            str.isSuccess = test.IsSuccess;
            str.duration = test.duration;
            str.stackTrace = test.stacktrace;
            str.isIgnored = test.IsIgnored;

            return str;
        }
    }

    #region SerializableTestResult

    /// <summary>   (Serializable) encapsulates the result of a serializable test. </summary>
    ///
 

    [Serializable]
    internal class SerializableTestResult : ITestResult
    {
        /// <summary>   State of the result. </summary>
        public TestResultState resultState;
        /// <summary>   The message. </summary>
        public string message;
        /// <summary>   True if executed. </summary>
        public bool executed;
        /// <summary>   The name. </summary>
        public string name;
        /// <summary>   Name of the full. </summary>
        public string fullName;
        /// <summary>   The identifier. </summary>
        public string id;
        /// <summary>   True if this object is success. </summary>
        public bool isSuccess;
        /// <summary>   The duration. </summary>
        public double duration;
        /// <summary>   The stack trace. </summary>
        public string stackTrace;
        /// <summary>   True if this object is ignored. </summary>
        public bool isIgnored;

        /// <summary>   Gets the state of the result. </summary>
        ///
        /// <value> The result state. </value>

        public TestResultState ResultState
        {
            get { return resultState; }
        }

        /// <summary>   Gets the message. </summary>
        ///
        /// <value> The message. </value>

        public string Message
        {
            get { return message; }
        }

        /// <summary>   Gets the logs. </summary>
        ///
        /// <value> The logs. </value>

        public string Logs
        {
            get { return null; }
        }

        /// <summary>   Gets a value indicating whether the executed. </summary>
        ///
        /// <value> True if executed, false if not. </value>

        public bool Executed
        {
            get { return executed; }
        }

        /// <summary>   Gets the name. </summary>
        ///
        /// <value> The name. </value>

        public string Name
        {
            get { return name; }
        }

        /// <summary>   Gets the name of the full. </summary>
        ///
        /// <value> The name of the full. </value>

        public string FullName
        {
            get { return fullName; }
        }

        /// <summary>   Gets the identifier. </summary>
        ///
        /// <value> The identifier. </value>

        public string Id
        {
            get { return id; }
        }

        /// <summary>   Gets a value indicating whether this object is success. </summary>
        ///
        /// <value> True if this object is success, false if not. </value>

        public bool IsSuccess
        {
            get { return isSuccess; }
        }

        /// <summary>   Gets the duration. </summary>
        ///
        /// <value> The duration. </value>

        public double Duration
        {
            get { return duration; }
        }

        /// <summary>   Gets the stack trace. </summary>
        ///
        /// <value> The stack trace. </value>

        public string StackTrace
        {
            get { return stackTrace; }
        }

        /// <summary>   Gets a value indicating whether this object is ignored. </summary>
        ///
        /// <value> True if this object is ignored, false if not. </value>

        public bool IsIgnored 
        {
            get { return isIgnored; }
        }
    }
    #endregion
}
