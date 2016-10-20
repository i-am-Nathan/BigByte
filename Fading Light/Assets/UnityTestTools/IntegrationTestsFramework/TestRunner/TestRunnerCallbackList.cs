// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\TestRunnerCallbackList.cs
//
// summary:	Implements the test runner callback list class

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest.IntegrationTestRunner
{
    /// <summary>   List of test runner callbacks. </summary>
    ///
 

    public class TestRunnerCallbackList : ITestRunnerCallback
    {
        /// <summary>   List of callbacks. </summary>
        private readonly List<ITestRunnerCallback> m_CallbackList = new List<ITestRunnerCallback>();

        /// <summary>   Adds callback. </summary>
        ///
     
        ///
        /// <param name="callback"> The callback to remove. </param>

        public void Add(ITestRunnerCallback callback)
        {
            m_CallbackList.Add(callback);
        }

        /// <summary>   Removes the given callback. </summary>
        ///
     
        ///
        /// <param name="callback"> The callback to remove. </param>

        public void Remove(ITestRunnerCallback callback)
        {
            m_CallbackList.Remove(callback);
        }

        /// <summary>   Executes the started operation. </summary>
        ///
     
        ///
        /// <param name="platform">     The platform. </param>
        /// <param name="testsToRun">   The tests to run. </param>

        public void RunStarted(string platform, List<TestComponent> testsToRun)
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.RunStarted(platform, testsToRun);
            }
        }

        /// <summary>   Executes the finished operation. </summary>
        ///
     
        ///
        /// <param name="testResults">  The test results. </param>

        public void RunFinished(List<TestResult> testResults)
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.RunFinished(testResults);
            }
        }

        /// <summary>   All scenes finished. </summary>
        ///
     

        public void AllScenesFinished()
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.AllScenesFinished();
            }
        }

        /// <summary>   Tests started. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        public void TestStarted(TestResult test)
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.TestStarted(test);
            }
        }

        /// <summary>   Tests finished. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        public void TestFinished(TestResult test)
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.TestFinished(test);
            }
        }

        /// <summary>   Tests run interrupted. </summary>
        ///
     
        ///
        /// <param name="testsNotRun">  The tests not run. </param>

        public void TestRunInterrupted(List<ITestComponent> testsNotRun)
        {
            foreach (var unitTestRunnerCallback in m_CallbackList)
            {
                unitTestRunnerCallback.TestRunInterrupted(testsNotRun);
            }
        }
    }
}
