// file:	Assets\UnityTestTools\Common\Editor\ResultWriter\XmlResultWriter.cs
//
// summary:	Implements the XML result writer class

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An XML result writer. </summary>
    ///
 

    public class XmlResultWriter
    {
        /// <summary>   The result writer. </summary>
        private readonly StringBuilder m_ResultWriter = new StringBuilder();
        /// <summary>   The indend. </summary>
        private int m_Indend;
        /// <summary>   Name of the suite. </summary>
        private readonly string m_SuiteName;
        /// <summary>   The results. </summary>
        private readonly ITestResult[] m_Results;
        /// <summary>   The platform. </summary>
        string m_Platform;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="suiteName">    Name of the suite. </param>
        /// <param name="platform">     The platform. </param>
        /// <param name="results">      The results. </param>

        public XmlResultWriter(string suiteName, string platform, ITestResult[] results)
        {
            m_SuiteName = suiteName;
            m_Results = results;
            m_Platform = platform;
        }

        /// <summary>   The unit version. </summary>
        private const string k_NUnitVersion = "2.6.2-Unity";

        /// <summary>   Gets test result. </summary>
        ///
     
        ///
        /// <returns>   The test result. </returns>

        public string GetTestResult()
        {
            InitializeXmlFile(m_SuiteName, new ResultSummarizer(m_Results));
            foreach (var result in m_Results)
            {
                WriteResultElement(result);
            }
            TerminateXmlFile();
            return m_ResultWriter.ToString();
        }

        /// <summary>   Initializes the XML file. </summary>
        ///
     
        ///
        /// <param name="resultsName">      Name of the results. </param>
        /// <param name="summaryResults">   The summary results. </param>

        private void InitializeXmlFile(string resultsName, ResultSummarizer summaryResults)
        {
            WriteHeader();

            DateTime now = DateTime.Now;
            var attributes = new Dictionary<string, string>
            {
                {"name", "Unity Tests"},
                {"total", summaryResults.TestsRun.ToString()},
                {"errors", summaryResults.Errors.ToString()},
                {"failures", summaryResults.Failures.ToString()},
                {"not-run", summaryResults.TestsNotRun.ToString()},
                {"inconclusive", summaryResults.Inconclusive.ToString()},
                {"ignored", summaryResults.Ignored.ToString()},
                {"skipped", summaryResults.Skipped.ToString()},
                {"invalid", summaryResults.NotRunnable.ToString()},
                {"date", now.ToString("yyyy-MM-dd")},
                {"time", now.ToString("HH:mm:ss")}
            };

            WriteOpeningElement("test-results", attributes);

            WriteEnvironment(m_Platform);
            WriteCultureInfo();
            WriteTestSuite(resultsName, summaryResults);
            WriteOpeningElement("results");
        }

        /// <summary>   Writes an opening element. </summary>
        ///
     
        ///
        /// <param name="elementName">  Name of the element. </param>

        private void WriteOpeningElement(string elementName)
        {
            WriteOpeningElement(elementName, new Dictionary<string, string>());
        }

        /// <summary>   Writes an opening element. </summary>
        ///
     
        ///
        /// <param name="elementName">  Name of the element. </param>
        /// <param name="attributes">   The attributes. </param>

        private void WriteOpeningElement(string elementName, Dictionary<string, string> attributes)
        {
            WriteOpeningElement(elementName, attributes, false);
        }

        /// <summary>   Writes an opening element. </summary>
        ///
     
        ///
        /// <param name="elementName">          Name of the element. </param>
        /// <param name="attributes">           The attributes. </param>
        /// <param name="closeImmediatelly">    True to close immediatelly. </param>

        private void WriteOpeningElement(string elementName, Dictionary<string, string> attributes, bool closeImmediatelly)
        {
            WriteIndend();
            m_Indend++;
            m_ResultWriter.Append("<");
            m_ResultWriter.Append(elementName);
            foreach (var attribute in attributes)
            {
                m_ResultWriter.AppendFormat(" {0}=\"{1}\"", attribute.Key, SecurityElement.Escape(attribute.Value));
            }
            if (closeImmediatelly)
            {
                m_ResultWriter.Append(" /");
                m_Indend--;
            }
            m_ResultWriter.AppendLine(">");
        }

        /// <summary>   Writes the indend. </summary>
        ///
     

        private void WriteIndend()
        {
            for (int i = 0; i < m_Indend; i++)
            {
                m_ResultWriter.Append("  ");
            }
        }

        /// <summary>   Writes a closing element. </summary>
        ///
     
        ///
        /// <param name="elementName">  Name of the element. </param>

        private void WriteClosingElement(string elementName)
        {
            m_Indend--;
            WriteIndend();
            m_ResultWriter.AppendLine("</" + elementName + ">");
        }

        /// <summary>   Writes the header. </summary>
        ///
     

        private void WriteHeader()
        {
            m_ResultWriter.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            m_ResultWriter.AppendLine("<!--This file represents the results of running a test suite-->");
        }

        /// <summary>   Gets environment user name. </summary>
        ///
     
        ///
        /// <returns>   The environment user name. </returns>

        static string GetEnvironmentUserName()
        {
            return Environment.UserName;
        }

        /// <summary>   Gets environment machine name. </summary>
        ///
     
        ///
        /// <returns>   The environment machine name. </returns>

        static string GetEnvironmentMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>   Gets environment user domain name. </summary>
        ///
     
        ///
        /// <returns>   The environment user domain name. </returns>

        static string GetEnvironmentUserDomainName()
        {
            return Environment.UserDomainName;
        }

        /// <summary>   Gets environment version. </summary>
        ///
     
        ///
        /// <returns>   The environment version. </returns>

        static string GetEnvironmentVersion()
        {
            return Environment.Version.ToString();
        }

        /// <summary>   Gets environment operating system version. </summary>
        ///
     
        ///
        /// <returns>   The environment operating system version. </returns>

        static string GetEnvironmentOSVersion()
        {
            return Environment.OSVersion.ToString();
        }

        /// <summary>   Gets environment operating system version platform. </summary>
        ///
     
        ///
        /// <returns>   The environment operating system version platform. </returns>

        static string GetEnvironmentOSVersionPlatform()
        {
            return Environment.OSVersion.Platform.ToString();
        }

        /// <summary>   Environment get current directory. </summary>
        ///
     
        ///
        /// <returns>   A string. </returns>

        static string EnvironmentGetCurrentDirectory()
        {
            return Environment.CurrentDirectory;
        }

        /// <summary>   Writes an environment. </summary>
        ///
     
        ///
        /// <param name="targetPlatform">   Target platform. </param>

        private void WriteEnvironment( string targetPlatform )
        {
            var attributes = new Dictionary<string, string>
            {
                {"nunit-version", k_NUnitVersion},
                {"clr-version", GetEnvironmentVersion()},
                {"os-version", GetEnvironmentOSVersion()},
                {"platform", GetEnvironmentOSVersionPlatform()},
                {"cwd", EnvironmentGetCurrentDirectory()},
                {"machine-name", GetEnvironmentMachineName()},
                {"user", GetEnvironmentUserName()},
                {"user-domain", GetEnvironmentUserDomainName()},
                {"unity-version", Application.unityVersion},
                {"unity-platform", targetPlatform}
            };
            WriteOpeningElement("environment", attributes, true);
        }

        /// <summary>   Writes the culture information. </summary>
        ///
     

        private void WriteCultureInfo()
        {
            var attributes = new Dictionary<string, string>
            {
                {"current-culture", CultureInfo.CurrentCulture.ToString()},
                {"current-uiculture", CultureInfo.CurrentUICulture.ToString()}
            };
            WriteOpeningElement("culture-info", attributes, true);
        }

        /// <summary>   Writes a test suite. </summary>
        ///
     
        ///
        /// <param name="resultsName">      Name of the results. </param>
        /// <param name="summaryResults">   The summary results. </param>

        private void WriteTestSuite(string resultsName, ResultSummarizer summaryResults)
        {
            var attributes = new Dictionary<string, string>
            {
                {"name", resultsName},
                {"type", "Assembly"},
                {"executed", "True"},
                {"result", summaryResults.Success ? "Success" : "Failure"},
                {"success", summaryResults.Success ? "True" : "False"},
                {"time", summaryResults.Duration.ToString("#####0.000", NumberFormatInfo.InvariantInfo)}
            };
            WriteOpeningElement("test-suite", attributes);
        }

        /// <summary>   Writes a result element. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        private void WriteResultElement(ITestResult result)
        {
            StartTestElement(result);

            switch (result.ResultState)
            {
                case TestResultState.Ignored:
                case TestResultState.NotRunnable:
                case TestResultState.Skipped:
                    WriteReasonElement(result);
                    break;

                case TestResultState.Failure:
                case TestResultState.Error:
                case TestResultState.Cancelled:
                    WriteFailureElement(result);
                    break;
                case TestResultState.Success:
                case TestResultState.Inconclusive:
                    if (result.Message != null)
                        WriteReasonElement(result);
                    break;
            };

            WriteClosingElement("test-case");
        }

        /// <summary>   Terminate XML file. </summary>
        ///
     

        private void TerminateXmlFile()
        {
            WriteClosingElement("results");
            WriteClosingElement("test-suite");
            WriteClosingElement("test-results");
        }

        #region Element Creation Helpers

        /// <summary>   Starts test element. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        private void StartTestElement(ITestResult result)
        {
            var attributes = new Dictionary<string, string>
            {
                {"name", result.FullName},
                {"executed", result.Executed.ToString()}
            };
            string resultString;
            switch (result.ResultState)
            {
                case TestResultState.Cancelled:
                    resultString = TestResultState.Failure.ToString();
                    break;
                default:
                    resultString = result.ResultState.ToString();
                    break;
            }
            attributes.Add("result", resultString);
            if (result.Executed)
            {
                attributes.Add("success", result.IsSuccess.ToString());
                attributes.Add("time", result.Duration.ToString("#####0.000", NumberFormatInfo.InvariantInfo));
            }
            WriteOpeningElement("test-case", attributes);
        }

        /// <summary>   Writes a reason element. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        private void WriteReasonElement(ITestResult result)
        {
            WriteOpeningElement("reason");
            WriteOpeningElement("message");
            WriteCData(result.Message);
            WriteClosingElement("message");
            WriteClosingElement("reason");
        }

        /// <summary>   Writes a failure element. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        private void WriteFailureElement(ITestResult result)
        {
            WriteOpeningElement("failure");
            WriteOpeningElement("message");
            WriteCData(result.Message);
            WriteClosingElement("message");
            WriteOpeningElement("stack-trace");
            if (result.StackTrace != null)
                WriteCData(StackTraceFilter.Filter(result.StackTrace));
            WriteClosingElement("stack-trace");
            WriteClosingElement("failure");
        }

        #endregion

        /// <summary>   Writes a c data. </summary>
        ///
     
        ///
        /// <param name="text"> The text. </param>

        private void WriteCData(string text)
        {
            if (string.IsNullOrEmpty(text)) 
				return;
            m_ResultWriter.AppendFormat("<![CDATA[{0}]]>", text);
            m_ResultWriter.AppendLine();
        }

        /// <summary>   Writes to file. </summary>
        ///
     
        ///
        /// <param name="resultDestiantion">    The result destiantion. </param>
        /// <param name="resultFileName">       Filename of the result file. </param>

        public void WriteToFile(string resultDestiantion, string resultFileName)
        {
            try
            {
                var path = Path.Combine(resultDestiantion, resultFileName);
                Debug.Log("Saving results in " + path);
                File.WriteAllText(path, GetTestResult(), Encoding.UTF8);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while opening file");
                Debug.LogException(e);
            }
        }
    }
}
