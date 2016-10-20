// ****************************************************************
// Based on nUnit 2.6.2 (http://www.nunit.org/)
// ****************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   Summary description for ResultSummarizer. </summary>
    ///
 

    public class ResultSummarizer
    {
        /// <summary>   Number of errors. </summary>
        private int m_ErrorCount;
        /// <summary>   Number of failures. </summary>
        private int m_FailureCount;
        /// <summary>   Number of ignores. </summary>
        private int m_IgnoreCount;
        /// <summary>   Number of inconclusives. </summary>
        private int m_InconclusiveCount;
        /// <summary>   The not runnable. </summary>
        private int m_NotRunnable;
        /// <summary>   Number of results. </summary>
        private int m_ResultCount;
        /// <summary>   Number of skips. </summary>
        private int m_SkipCount;
        /// <summary>   Number of success. </summary>
        private int m_SuccessCount;
        /// <summary>   The tests run. </summary>
        private int m_TestsRun;

        /// <summary>   The duration. </summary>
        private TimeSpan m_Duration;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="results">  The results. </param>

        public ResultSummarizer(IEnumerable<ITestResult> results)
        {
            foreach (var result in results)
                Summarize(result);
        }

        /// <summary>   Gets a value indicating whether the success. </summary>
        ///
        /// <value> True if success, false if not. </value>

        public bool Success
        {
            get { return m_FailureCount == 0; }
        }

        /// <summary>
        /// Returns the number of test cases for which results have been summarized. Any tests excluded
        /// by use of Category or Explicit attributes are not counted.
        /// </summary>
        ///
        /// <value> The number of results. </value>

        public int ResultCount
        {
            get { return m_ResultCount; }
        }

        /// <summary>
        /// Returns the number of test cases actually run, which is the same as ResultCount, less any
        /// Skipped, Ignored or NonRunnable tests.
        /// </summary>
        ///
        /// <value> The tests run. </value>

        public int TestsRun
        {
            get { return m_TestsRun; }
        }

        /// <summary>   Returns the number of tests that passed. </summary>
        ///
        /// <value> The passed. </value>

        public int Passed
        {
            get { return m_SuccessCount; }
        }

        /// <summary>   Returns the number of test cases that had an error. </summary>
        ///
        /// <value> The errors. </value>

        public int Errors
        {
            get { return m_ErrorCount; }
        }

        /// <summary>   Returns the number of test cases that failed. </summary>
        ///
        /// <value> The failures. </value>

        public int Failures
        {
            get { return m_FailureCount; }
        }

        /// <summary>   Returns the number of test cases that failed. </summary>
        ///
        /// <value> The inconclusive. </value>

        public int Inconclusive
        {
            get { return m_InconclusiveCount; }
        }

        /// <summary>
        /// Returns the number of test cases that were not runnable due to errors in the signature of the
        /// class or method. Such tests are also counted as Errors.
        /// </summary>
        ///
        /// <value> The not runnable. </value>

        public int NotRunnable
        {
            get { return m_NotRunnable; }
        }

        /// <summary>   Returns the number of test cases that were skipped. </summary>
        ///
        /// <value> The skipped. </value>

        public int Skipped
        {
            get { return m_SkipCount; }
        }

        /// <summary>   Gets the ignored. </summary>
        ///
        /// <value> The ignored. </value>

        public int Ignored
        {
            get { return m_IgnoreCount; }
        }

        /// <summary>   Gets the duration. </summary>
        ///
        /// <value> The duration. </value>

        public double Duration
        {
            get { return m_Duration.TotalSeconds; }
        }

        /// <summary>   Gets the tests not run. </summary>
        ///
        /// <value> The tests not run. </value>

        public int TestsNotRun
        {
            get { return m_SkipCount + m_IgnoreCount + m_NotRunnable; }
        }

        /// <summary>   Summarizes the given result. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        public void Summarize(ITestResult result)
        {
            m_Duration += TimeSpan.FromSeconds(result.Duration);
            m_ResultCount++;
            
            if(!result.Executed)
            {
                if(result.IsIgnored)
                {
                    m_IgnoreCount++;
                    return;
                }
                
                m_SkipCount++;
                return;
            }
            
            switch (result.ResultState)
            {
                case TestResultState.Success:
                    m_SuccessCount++;
                    m_TestsRun++;
                    break;
                case TestResultState.Failure:
                    m_FailureCount++;
                    m_TestsRun++;
                    break;
                case TestResultState.Error:
                case TestResultState.Cancelled:
                    m_ErrorCount++;
                    m_TestsRun++;
                    break;
                case TestResultState.Inconclusive:
                    m_InconclusiveCount++;
                    m_TestsRun++;
                    break;
                case TestResultState.NotRunnable:
                    m_NotRunnable++;
                    // errorCount++;
                    break;
                case TestResultState.Ignored:
                    m_IgnoreCount++;
                    break;
                default:
                    m_SkipCount++;
                    break;
            }
        }
    }
}
