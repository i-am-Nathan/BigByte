// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\ITestRunnerCallback.cs
//
// summary:	Declares the ITestRunnerCallback interface

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest.IntegrationTestRunner
{
    /// <summary>   Interface for test runner callback. </summary>
    ///
 

    public interface ITestRunnerCallback
    {
        /// <summary>   Executes the started operation. </summary>
        ///
        /// <param name="platform">     The platform. </param>
        /// <param name="testsToRun">   The tests to run. </param>

        void RunStarted(string platform, List<TestComponent> testsToRun);

        /// <summary>   Executes the finished operation. </summary>
        ///
        /// <param name="testResults">  The test results. </param>

        void RunFinished(List<TestResult> testResults);
        /// <summary>   All scenes finished. </summary>
        void AllScenesFinished();

        /// <summary>   Tests started. </summary>
        ///
        /// <param name="test"> The test. </param>

        void TestStarted(TestResult test);

        /// <summary>   Tests finished. </summary>
        ///
        /// <param name="test"> The test. </param>

        void TestFinished(TestResult test);

        /// <summary>   Tests run interrupted. </summary>
        ///
        /// <param name="testsNotRun">  The tests not run. </param>

        void TestRunInterrupted(List<ITestComponent> testsNotRun);
    }
}
