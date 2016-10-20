// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\Batch.cs
//
// summary:	Implements the batch class

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityTest.IntegrationTests;
using UnityEditor.SceneManagement;

namespace UnityTest
{
    /// <summary>   A batch. </summary>
    ///
 

    public static partial class Batch
	{
        /// <summary>   . </summary>
		const string k_ResultFilePathParam = "-resultFilePath=";
        /// <summary>   . </summary>
        private const string k_TestScenesParam = "-testscenes=";
        /// <summary>   . </summary>
        private const string k_OtherBuildScenesParam = "-includeBuildScenes=";
        /// <summary>   . </summary>
        const string k_TargetPlatformParam = "-targetPlatform=";
        /// <summary>   . </summary>
        const string k_ResultFileDirParam = "-resultsFileDirectory=";

        /// <summary>   The return code tests ok. </summary>
        public static int returnCodeTestsOk = 0;
        /// <summary>   The return code tests failed. </summary>
        public static int returnCodeTestsFailed = 2;
        /// <summary>   The return code run error. </summary>
        public static int returnCodeRunError = 3;

        /// <summary>   Executes the integration tests. </summary>
        ///
     

        public static void RunIntegrationTests()
        {
            var targetPlatform = GetTargetPlatform();
            var otherBuildScenes = GetSceneListFromParam (k_OtherBuildScenesParam);

            var testScenes = GetSceneListFromParam(k_TestScenesParam);
            if (testScenes.Count == 0)
                testScenes = FindTestScenesInProject();

            RunIntegrationTests(targetPlatform, testScenes, otherBuildScenes);
        }

        /// <summary>   Executes the integration tests. </summary>
        ///
     
        ///
        /// <param name="targetPlatform">   Target platform. </param>

        public static void RunIntegrationTests(BuildTarget ? targetPlatform)
        {
            var sceneList = FindTestScenesInProject();
            RunIntegrationTests(targetPlatform, sceneList, new List<string>());
        }

        /// <summary>   Executes the integration tests. </summary>
        ///
     
        ///
        /// <param name="targetPlatform">   Target platform. </param>
        /// <param name="testScenes">       The test scenes. </param>
        /// <param name="otherBuildScenes"> The other build scenes. </param>

        public static void RunIntegrationTests(BuildTarget? targetPlatform, List<string> testScenes, List<string> otherBuildScenes)
        {
            if (targetPlatform.HasValue)
                BuildAndRun(targetPlatform.Value, testScenes, otherBuildScenes);
            else
                RunInEditor(testScenes,  otherBuildScenes);
        }

        /// <summary>   Builds and run. </summary>
        ///
     
        ///
        /// <param name="target">           Target for the. </param>
        /// <param name="testScenes">       The test scenes. </param>
        /// <param name="otherBuildScenes"> The other build scenes. </param>

        private static void BuildAndRun(BuildTarget target, List<string> testScenes, List<string> otherBuildScenes)
        {
            var resultFilePath = GetParameterArgument(k_ResultFileDirParam);

            const int port = 0;
            var ipList = TestRunnerConfigurator.GetAvailableNetworkIPs();

            var config = new PlatformRunnerConfiguration
            {
                buildTarget = target,
                buildScenes = otherBuildScenes,
                testScenes = testScenes,
                projectName = "IntegrationTests",
                resultsDir = resultFilePath,
                sendResultsOverNetwork = InternalEditorUtility.inBatchMode,
                ipList = ipList,
                port = port
            };

            if (Application.isWebPlayer)
            {
                config.sendResultsOverNetwork = false;
                Debug.Log("You can't use WebPlayer as active platform for running integration tests. Switching to Standalone");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
            }

            PlatformRunner.BuildAndRunInPlayer(config);
        }

        /// <summary>   Executes the in editor operation. </summary>
        ///
     
        ///
        /// <param name="testScenes">       The test scenes. </param>
        /// <param name="otherBuildScenes"> The other build scenes. </param>

        private static void RunInEditor(List<string> testScenes, List<string> otherBuildScenes)
        {
            CheckActiveBuildTarget();

            NetworkResultsReceiver.StopReceiver();
            if (testScenes == null || testScenes.Count == 0)
            {
                Debug.Log("No test scenes on the list");
                EditorApplication.Exit(returnCodeRunError);
                return;
            }
             
            string previousScenesXml = "";
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(EditorBuildSettingsScene[]));
            using(StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, EditorBuildSettings.scenes);
                previousScenesXml = textWriter.ToString();
            }
                
            EditorBuildSettings.scenes = (testScenes.Concat(otherBuildScenes).ToList()).Select(s => new EditorBuildSettingsScene(s, true)).ToArray();
            EditorSceneManager.OpenScene(testScenes.First());
            GuiHelper.SetConsoleErrorPause(false);

            var config = new PlatformRunnerConfiguration
            {
                resultsDir = GetParameterArgument(k_ResultFileDirParam),
                ipList = TestRunnerConfigurator.GetAvailableNetworkIPs(),
                port = PlatformRunnerConfiguration.TryToGetFreePort(),
                runInEditor = true
            };
                    
            var settings = new PlayerSettingConfigurator(true);
            settings.AddConfigurationFile(TestRunnerConfigurator.integrationTestsNetwork, string.Join("\n", config.GetConnectionIPs()));
            settings.AddConfigurationFile(TestRunnerConfigurator.testScenesToRun, string.Join ("\n", testScenes.ToArray()));
            settings.AddConfigurationFile(TestRunnerConfigurator.previousScenes, previousScenesXml);
         
            NetworkResultsReceiver.StartReceiver(config);

            EditorApplication.isPlaying = true;
        }

        /// <summary>   Gets parameter argument. </summary>
        ///
     
        ///
        /// <param name="parameterName">    Name of the parameter. </param>
        ///
        /// <returns>   The parameter argument. </returns>

        private static string GetParameterArgument(string parameterName)
        {
            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.ToLower().StartsWith(parameterName.ToLower()))
                {
                    return arg.Substring(parameterName.Length);
                }
            }
            return null;
        }

        /// <summary>   Check active build target. </summary>
        ///
     

        static void CheckActiveBuildTarget()
        {
            var notSupportedPlatforms = new[] { "MetroPlayer", "WebPlayer", "WebPlayerStreamed" };
            if (notSupportedPlatforms.Contains(EditorUserBuildSettings.activeBuildTarget.ToString()))
            {
                Debug.Log("activeBuildTarget can not be  "
                    + EditorUserBuildSettings.activeBuildTarget + 
                    " use buildTarget parameter to open Unity.");
            }
        }

        /// <summary>   Gets target platform. </summary>
        ///
     
        ///
        /// <returns>   The target platform. </returns>

        private static BuildTarget ? GetTargetPlatform()
        {
            string platformString = null;
            BuildTarget buildTarget;
            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.ToLower().StartsWith(k_TargetPlatformParam.ToLower()))
                {
                    platformString = arg.Substring(k_ResultFilePathParam.Length);
                    break;
                }
            }
            try
            {
                if (platformString == null) return null;
                buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), platformString);
            }
            catch
            {
                return null;
            }
            return buildTarget;
        }

        /// <summary>   Searches for the first test scenes in project. </summary>
        ///
     
        ///
        /// <returns>   The found test scenes in project. </returns>

        private static List<string> FindTestScenesInProject()
        {
            var integrationTestScenePattern = "*Test?.unity";
            return Directory.GetFiles("Assets", integrationTestScenePattern, SearchOption.AllDirectories).ToList();
        }

        /// <summary>   Gets scene list from parameter. </summary>
        ///
     
        ///
        /// <param name="param">    The parameter. </param>
        ///
        /// <returns>   The scene list from parameter. </returns>

        private static List<string> GetSceneListFromParam(string param)
        {
            var sceneList = new List<string>();
            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.ToLower().StartsWith(param.ToLower()))
                {
                    var scenesFromParam = arg.Substring(param.Length).Split(',');
                    foreach (var scene in scenesFromParam)
                    {
                        var sceneName = scene;
                        if (!sceneName.EndsWith(".unity"))
                            sceneName += ".unity";
                        var foundScenes = Directory.GetFiles(Directory.GetCurrentDirectory(), sceneName, SearchOption.AllDirectories);
                        if (foundScenes.Length == 1)
                            sceneList.Add(foundScenes[0].Substring(Directory.GetCurrentDirectory().Length + 1));
                        else
                            Debug.Log(sceneName + " not found or multiple entries found");
                    }
                }
            }
            return sceneList.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        }
    }
}
