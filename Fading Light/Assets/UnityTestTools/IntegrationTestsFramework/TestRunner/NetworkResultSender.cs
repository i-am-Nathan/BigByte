// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\NetworkResultSender.cs
//
// summary:	Implements the network result sender class

#if !UNITY_METRO && (UNITY_PRO_LICENSE || !(UNITY_ANDROID || UNITY_IPHONE))
#define UTT_SOCKETS_SUPPORTED
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTest.IntegrationTestRunner;

#if UTT_SOCKETS_SUPPORTED
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace UnityTest
{
    /// <summary>   A network result sender. </summary>
    ///
 

    public class NetworkResultSender : ITestRunnerCallback
    {
#if UTT_SOCKETS_SUPPORTED
        /// <summary>   The connection timeout. </summary>
        private readonly TimeSpan m_ConnectionTimeout = TimeSpan.FromSeconds(5);

        /// <summary>   The IP. </summary>
        private readonly string m_Ip;
        /// <summary>   The port. </summary>
        private readonly int m_Port;
#endif
        /// <summary>   True to lost connection. </summary>
        private bool m_LostConnection;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="ip">   The IP. </param>
        /// <param name="port"> The port. </param>

        public NetworkResultSender(string ip, int port)
        {
#if UTT_SOCKETS_SUPPORTED
            m_Ip = ip;
            m_Port = port;
#endif
        }

        /// <summary>   Sends a dto. </summary>
        ///
     
        ///
        /// <param name="dto">  The dto. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        private bool SendDTO(ResultDTO dto)
        {
            if (m_LostConnection) return false;
#if UTT_SOCKETS_SUPPORTED 
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var result = tcpClient.BeginConnect(m_Ip, m_Port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(m_ConnectionTimeout);
                    if (!success)
                    {
                        return false;
                    }
                    try
                    {
                        tcpClient.EndConnect(result);
                    }
                    catch (SocketException)
                    {
                        m_LostConnection = true;
                        return false;
                    }

                    var bf = new DTOFormatter();
                    bf.Serialize(tcpClient.GetStream(), dto);
                    tcpClient.GetStream().Close();
                    tcpClient.Close();
                    Debug.Log("Sent " + dto.messageType);
                }
            }
            catch (SocketException e)
            {
                Debug.LogException(e);
                m_LostConnection = true;
                return false;
            }
#endif  // if UTT_SOCKETS_SUPPORTED
            return true;
        }

        /// <summary>   Pings this object. </summary>
        ///
     
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool Ping()
        {
            var result = SendDTO(ResultDTO.CreatePing());
            m_LostConnection = false;
            return result;
        }

        /// <summary>   Executes the started operation. </summary>
        ///
     
        ///
        /// <param name="platform">     The platform. </param>
        /// <param name="testsToRun">   The tests to run. </param>

        public void RunStarted(string platform, List<TestComponent> testsToRun)
        {
            SendDTO(ResultDTO.CreateRunStarted());
        }

        /// <summary>   Executes the finished operation. </summary>
        ///
     
        ///
        /// <param name="testResults">  The test results. </param>

        public void RunFinished(List<TestResult> testResults)
        {
            SendDTO(ResultDTO.CreateRunFinished(testResults));
        }

        /// <summary>   Tests started. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        public void TestStarted(TestResult test)
        {
            SendDTO(ResultDTO.CreateTestStarted(test));
        }

        /// <summary>   Tests finished. </summary>
        ///
     
        ///
        /// <param name="test"> The test. </param>

        public void TestFinished(TestResult test)
        {
            SendDTO(ResultDTO.CreateTestFinished(test));
        }

        /// <summary>   All scenes finished. </summary>
        ///
     

        public void AllScenesFinished()
        {
            SendDTO (ResultDTO.CreateAllScenesFinished ());
        }

        /// <summary>   Tests run interrupted. </summary>
        ///
     
        ///
        /// <param name="testsNotRun">  The tests not run. </param>

        public void TestRunInterrupted(List<ITestComponent> testsNotRun)
        {
            RunFinished(new List<TestResult>());
        }
    }
}
