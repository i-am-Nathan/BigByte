// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\TestRunnerConfigurator.cs
//
// summary:	Implements the test runner configurator class

#if !UNITY_METRO && !UNITY_WEBPLAYER && (UNITY_PRO_LICENSE || !(UNITY_ANDROID || UNITY_IPHONE))
#define UTT_SOCKETS_SUPPORTED
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityTest.IntegrationTestRunner;
#if UTT_SOCKETS_SUPPORTED
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
#endif

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace UnityTest
{
    /// <summary>   A test runner configurator. </summary>
    ///
 

    public class TestRunnerConfigurator
    {
        /// <summary>   The integration tests network. </summary>
        public static string integrationTestsNetwork = "networkconfig.txt";
        /// <summary>   The batch run file marker. </summary>
        public static string batchRunFileMarker = "batchrun.txt";
        /// <summary>   The test scenes to run. </summary>
        public static string testScenesToRun = "testscenes.txt";
        /// <summary>   The previous scenes. </summary>
        public static string previousScenes = "previousScenes.txt";

        /// <summary>   Gets or sets a value indicating whether this object is batch run. </summary>
        ///
        /// <value> True if this object is batch run, false if not. </value>

        public bool isBatchRun { get; private set; }

        /// <summary>   Gets or sets a value indicating whether the send results over network. </summary>
        ///
        /// <value> True if send results over network, false if not. </value>

        public bool sendResultsOverNetwork { get; private set; }

#if UTT_SOCKETS_SUPPORTED
        private readonly List<IPEndPoint> m_IPEndPointList = new List<IPEndPoint>();
#endif

        /// <summary>   Default constructor. </summary>
        ///
     

        public TestRunnerConfigurator()
        {
            CheckForBatchMode();
            CheckForSendingResultsOverNetwork();
        }

#if UNITY_EDITOR

        /// <summary>   Gets the previous scenes to restore. </summary>
        ///
     
        ///
        /// <returns>   An array of editor build settings scene. </returns>

        public UnityEditor.EditorBuildSettingsScene[] GetPreviousScenesToRestore()
        {
            string text = null;
            if (Application.isEditor)
            {
                text = GetTextFromTempFile(previousScenes);
            }
                
            if(text != null)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UnityEditor.EditorBuildSettingsScene[]));
                using(var textReader = new StringReader(text))
                {
                    try 
                    {
                        return (UnityEditor.EditorBuildSettingsScene[] )serializer.Deserialize(textReader);
                    }
                    catch (System.Xml.XmlException e)
                    {
                        return null;
                    }
                }
            }
                
            return null;
        }
#endif

        /// <summary>   Gets integration test scenes. </summary>
        ///
     
        ///
        /// <param name="testSceneNum"> The test scene number. </param>
        ///
        /// <returns>   The integration test scenes. </returns>

        public string GetIntegrationTestScenes(int testSceneNum)
        {
            string text;
            if (Application.isEditor)
                text = GetTextFromTempFile(testScenesToRun);
            else
                text = GetTextFromTextAsset(testScenesToRun);

            List<string> sceneList = new List<string>();
            foreach (var line in text.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                sceneList.Add(line.ToString());
            }

            if (testSceneNum < sceneList.Count)
                return sceneList.ElementAt(testSceneNum);
            else
                return null;
        }

        /// <summary>   Check for sending results over network. </summary>
        ///
     
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>

        private void CheckForSendingResultsOverNetwork()
        {
#if UTT_SOCKETS_SUPPORTED
            string text;
            if (Application.isEditor)
                text = GetTextFromTempFile(integrationTestsNetwork);
            else
                text = GetTextFromTextAsset(integrationTestsNetwork);

            if (text == null) return;

            sendResultsOverNetwork = true;

            m_IPEndPointList.Clear();

            foreach (var line in text.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                var idx = line.IndexOf(':');
                if (idx == -1) throw new Exception(line);
                var ip = line.Substring(0, idx);
                var port = line.Substring(idx + 1);
                m_IPEndPointList.Add(new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port)));
            }
#endif  // if UTT_SOCKETS_SUPPORTED
        }

        /// <summary>   Gets text from text asset. </summary>
        ///
     
        ///
        /// <param name="fileName"> Filename of the file. </param>
        ///
        /// <returns>   The text from text asset. </returns>

        private static string GetTextFromTextAsset(string fileName)
        {
            var nameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf('.'));
            var resultpathFile = Resources.Load(nameWithoutExtension) as TextAsset;
            return resultpathFile != null ? resultpathFile.text : null;
        }

        /// <summary>   Gets text from temporary file. </summary>
        ///
     
        ///
        /// <param name="fileName"> Filename of the file. </param>
        ///
        /// <returns>   The text from temporary file. </returns>

        private static string GetTextFromTempFile(string fileName)
        {
            string text = null;
            try
            {
#if UNITY_EDITOR && !UNITY_WEBPLAYER
                text = File.ReadAllText(Path.Combine("Temp", fileName));
#endif
            }
            catch
            {
                return null;
            }
            return text;
        }

        /// <summary>   Check for batch mode. </summary>
        ///
     

        private void CheckForBatchMode()
        {
#if IMITATE_BATCH_MODE
            isBatchRun = true;
#elif UNITY_EDITOR
            if (Application.isEditor && InternalEditorUtility.inBatchMode)
                isBatchRun = true;
#else
            if (GetTextFromTextAsset(batchRunFileMarker) != null) isBatchRun = true;
#endif
        }

        /// <summary>   Gets available network i ps. </summary>
        ///
     
        ///
        /// <returns>   The available network i ps. </returns>

        public static List<string> GetAvailableNetworkIPs()
        {
#if UTT_SOCKETS_SUPPORTED
            if (!NetworkInterface.GetIsNetworkAvailable()) 
                return new List<String>{IPAddress.Loopback.ToString()};

            var ipList = new List<UnicastIPAddressInformation>();
            var allIpsList = new List<UnicastIPAddressInformation>();

            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 &&
                    netInterface.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    continue;

                var ipAdresses = netInterface.GetIPProperties().UnicastAddresses
                    .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork);
                allIpsList.AddRange(ipAdresses);

                if (netInterface.OperationalStatus != OperationalStatus.Up) continue;

                ipList.AddRange(ipAdresses);
            }

            //On Mac 10.10 all interfaces return OperationalStatus.Unknown, thus this workaround
            if(!ipList.Any()) return allIpsList.Select(i => i.Address.ToString()).ToList();

            // sort ip list by their masks to predict which ip belongs to lan network
            ipList.Sort((ip1, ip2) =>
                        {
                            var mask1 = BitConverter.ToInt32(ip1.IPv4Mask.GetAddressBytes().Reverse().ToArray(), 0);
                            var mask2 = BitConverter.ToInt32(ip2.IPv4Mask.GetAddressBytes().Reverse().ToArray(), 0);
                            return mask2.CompareTo(mask1);
                        });
            if (ipList.Count == 0)
                return new List<String> { IPAddress.Loopback.ToString() };
            return ipList.Select(i => i.Address.ToString()).ToList();
#else
            return new List<string>();
#endif  // if UTT_SOCKETS_SUPPORTED
        }

        /// <summary>   Resolve network connection. </summary>
        ///
     
        ///
        /// <returns>   An ITestRunnerCallback. </returns>

        public ITestRunnerCallback ResolveNetworkConnection()
        {
#if UTT_SOCKETS_SUPPORTED
            var nrsList = m_IPEndPointList.Select(ipEndPoint => new NetworkResultSender(ipEndPoint.Address.ToString(), ipEndPoint.Port)).ToList();

            var timeout = TimeSpan.FromSeconds(30);
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime) < timeout)
            {
                foreach (var networkResultSender in nrsList)
                {
                    try
                    {
                        if (!networkResultSender.Ping()) continue;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        sendResultsOverNetwork = false;
                        return null;
                    }
                    return networkResultSender;
                }
                Thread.Sleep(500);
            }
            Debug.LogError("Couldn't connect to the server: " + string.Join(", ", m_IPEndPointList.Select(ipep => ipep.Address + ":" + ipep.Port).ToArray()));
            sendResultsOverNetwork = false;
#endif  // if UTT_SOCKETS_SUPPORTED
            return null;
        }
    }
}
