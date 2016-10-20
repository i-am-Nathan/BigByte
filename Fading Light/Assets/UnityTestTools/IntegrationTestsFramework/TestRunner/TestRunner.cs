// #define IMITATE_BATCH_MODE //uncomment if you want to imitate batch mode behaviour in non-batch mode mode run

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityTest.IntegrationTestRunner;
using System.IO;
using UnityEngine.SceneManagement;

namespace UnityTest
{
    /// <summary>   (Serializable) a test runner. </summary>
    ///
 

    [Serializable]
    public class TestRunner : MonoBehaviour
    {
        /// <summary>   The test scene number. </summary>
        static private int TestSceneNumber = 0;
        /// <summary>   The result renderer. </summary>
        static private readonly TestResultRenderer k_ResultRenderer = new TestResultRenderer();

        /// <summary>   The current test. </summary>
        public TestComponent currentTest;
        /// <summary>   List of results. </summary>
        private List<TestResult> m_ResultList = new List<TestResult>();
        /// <summary>   The test components. </summary>
        private List<TestComponent> m_TestComponents;

        /// <summary>   Gets a value indicating whether this object is initialized by runner. </summary>
        ///
        /// <value> True if this object is initialized by runner, false if not. </value>

        public bool isInitializedByRunner
        {
            get
            {
#if !IMITATE_BATCH_MODE
                if (Application.isEditor && !IsBatchMode())
                    return true;
#endif
                return false;
            }
        }

        /// <summary>   The start time. </summary>
        private double m_StartTime;
        /// <summary>   True to ready to run. </summary>
        private bool m_ReadyToRun;

        /// <summary>   The test messages. </summary>
        private string m_TestMessages;
        /// <summary>   The stacktrace. </summary>
        private string m_Stacktrace;
        /// <summary>   State of the test. </summary>
        private TestState m_TestState = TestState.Running;

        /// <summary>   The configurator. </summary>
        private TestRunnerConfigurator m_Configurator;

        /// <summary>   The test runner callback. </summary>
        public TestRunnerCallbackList TestRunnerCallback = new TestRunnerCallbackList();
        /// <summary>   The tests provider. </summary>
        private IntegrationTestsProvider m_TestsProvider;

        /// <summary>   The prefix. </summary>
        private const string k_Prefix = "IntegrationTest";
        /// <summary>   Message describing the started. </summary>
        private const string k_StartedMessage = k_Prefix + " Started";
        /// <summary>   Message describing the finished. </summary>
        private const string k_FinishedMessage = k_Prefix + " Finished";
        /// <summary>   Message describing the timeout. </summary>
        private const string k_TimeoutMessage = k_Prefix + " Timeout";
        /// <summary>   Message describing the failed. </summary>
        private const string k_FailedMessage = k_Prefix + " Failed";
        /// <summary>   Message describing the failed exception. </summary>
        private const string k_FailedExceptionMessage = k_Prefix + " Failed with exception";
        /// <summary>   Message describing the ignored. </summary>
        private const string k_IgnoredMessage = k_Prefix + " Ignored";
        /// <summary>   Message describing the interrupted. </summary>
        private const string k_InterruptedMessage = k_Prefix + " Run interrupted";

        /// <summary>   Awakes this object. </summary>
        ///
     

        public void Awake()
        {
            m_Configurator = new TestRunnerConfigurator();
            if (isInitializedByRunner) return;
            TestComponent.DisableAllTests();
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        public void Start()
        {
            if (isInitializedByRunner) return;

            if (m_Configurator.sendResultsOverNetwork)
            {
                var nrs = m_Configurator.ResolveNetworkConnection();
                if (nrs != null)
                    TestRunnerCallback.Add(nrs);
            }

            TestComponent.DestroyAllDynamicTests();
            var dynamicTestTypes = TestComponent.GetTypesWithHelpAttribute(SceneManager.GetActiveScene().name);
            foreach (var dynamicTestType in dynamicTestTypes)
                TestComponent.CreateDynamicTest(dynamicTestType);

            var tests = TestComponent.FindAllTestsOnScene();

            InitRunner(tests, dynamicTestTypes.Select(type => type.AssemblyQualifiedName).ToList());
        }

        /// <summary>   Init runner. </summary>
        ///
     
        ///
        /// <param name="tests">                The tests. </param>
        /// <param name="dynamicTestsToRun">    The dynamic tests to run. </param>

        public void InitRunner(List<TestComponent> tests, List<string> dynamicTestsToRun)
        {
            Application.logMessageReceived += LogHandler;

            // Init dynamic tests
            foreach (var typeName in dynamicTestsToRun)
            {
                var t = Type.GetType(typeName);
                if (t == null) continue;
                var scriptComponents = Resources.FindObjectsOfTypeAll(t) as MonoBehaviour[];
                if (scriptComponents.Length == 0)
                {
                    Debug.LogWarning(t + " not found. Skipping.");
                    continue;
                }
                if (scriptComponents.Length > 1) Debug.LogWarning("Multiple GameObjects refer to " + typeName);
                tests.Add(scriptComponents.First().GetComponent<TestComponent>());
            }
            // create test structure
            m_TestComponents = ParseListForGroups(tests).ToList();
            // create results for tests
            m_ResultList = m_TestComponents.Select(component => new TestResult(component)).ToList();
            // init test provider
            m_TestsProvider = new IntegrationTestsProvider(m_ResultList.Select(result => result.TestComponent as ITestComponent));
            m_ReadyToRun = true;
        }

        /// <summary>   Enumerates parse list for groups in this collection. </summary>
        ///
     
        ///
        /// <param name="tests">    The tests. </param>
        ///
        /// <returns>
        /// An enumerator that allows foreach to be used to process parse list for groups in this
        /// collection.
        /// </returns>

        private static IEnumerable<TestComponent> ParseListForGroups(IEnumerable<TestComponent> tests)
        {
            var results = new HashSet<TestComponent>();
            foreach (var testResult in tests)
            {
                if (testResult.IsTestGroup())
                {
                    var childrenTestResult = testResult.gameObject.GetComponentsInChildren(typeof(TestComponent), true)
                                             .Where(t => t != testResult)
                                             .Cast<TestComponent>()
                                             .ToArray();
                    foreach (var result in childrenTestResult)
                    {
                        if (!result.IsTestGroup())
                            results.Add(result);
                    }
                    continue;
                }
                results.Add(testResult);
            }
            return results;
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        public void Update()
        {
            if (m_ReadyToRun  && Time.frameCount > 1)
            {
                m_ReadyToRun = false;
                StartCoroutine("StateMachine");
            }
        }

        /// <summary>   Executes the destroy action. </summary>
        ///
     

        public void OnDestroy()
        {
            if (currentTest != null)
            {
                var testResult = m_ResultList.Single(result => result.TestComponent == currentTest);
                testResult.messages += "Test run interrupted (crash?)";
                LogMessage(k_InterruptedMessage);
                FinishTest(TestResult.ResultType.Failed);
            }
            if (currentTest != null || (m_TestsProvider != null && m_TestsProvider.AnyTestsLeft()))
            {
                var remainingTests = m_TestsProvider.GetRemainingTests();
                TestRunnerCallback.TestRunInterrupted(remainingTests.ToList());
            }
            Application.logMessageReceived -= LogHandler;
        }

        /// <summary>   Handler, called when the log. </summary>
        ///
     
        ///
        /// <param name="condition">    The condition. </param>
        /// <param name="stacktrace">   The stacktrace. </param>
        /// <param name="type">         The type. </param>

        private void LogHandler(string condition, string stacktrace, LogType type)
        {
            if (!condition.StartsWith(k_StartedMessage) && !condition.StartsWith(k_FinishedMessage))
            {
                var msg = condition;
                if (msg.StartsWith(k_Prefix)) msg = msg.Substring(k_Prefix.Length + 1);
                if (currentTest != null && msg.EndsWith("(" + currentTest.name + ')')) msg = msg.Substring(0, msg.LastIndexOf('('));
                m_TestMessages += msg + "\n";
            }
            switch (type)
            {
                case LogType.Exception:
                {
                    var exceptionType = condition.Substring(0, condition.IndexOf(':'));
                    if (currentTest != null && currentTest.IsExceptionExpected(exceptionType))
                    {
                        m_TestMessages += exceptionType + " was expected\n";
                        if (currentTest.ShouldSucceedOnException())
                        {
                            m_TestState = TestState.Success;
                        }
                    }
                    else
                    {
                        m_TestState = TestState.Exception;
                        m_Stacktrace = stacktrace;
                    }
                }
                    break;
                case LogType.Assert:
                case LogType.Error:
                    m_TestState = TestState.Failure;
                    m_Stacktrace = stacktrace;
                    break;
                case LogType.Log:
                    if (m_TestState ==  TestState.Running && condition.StartsWith(IntegrationTest.passMessage))
                    {
                        m_TestState = TestState.Success;
                    }
                    if (condition.StartsWith(IntegrationTest.failMessage))
                    {
                        m_TestState = TestState.Failure;
                    }
                    break;
            }
        }

        /// <summary>   State machine. </summary>
        ///
     
        ///
        /// <returns>   An IEnumerator. </returns>

        public IEnumerator StateMachine()
        {
            TestRunnerCallback.RunStarted(Application.platform.ToString(), m_TestComponents);
            while (true)
            {
                if (!m_TestsProvider.AnyTestsLeft() && currentTest == null)
                {
                    FinishTestRun();
                    yield break;
                }
                if (currentTest == null)
                {
                    StartNewTest();
                }
                if (currentTest != null)
                {
                    if (m_TestState == TestState.Running)
                    {
                        if(currentTest.ShouldSucceedOnAssertions())
                        {
                            var assertionsToCheck = currentTest.gameObject.GetComponentsInChildren<AssertionComponent>().Where(a => a.enabled).ToArray();
                            if (assertionsToCheck.Any () && assertionsToCheck.All(a => a.checksPerformed > 0))
                            {
                                IntegrationTest.Pass(currentTest.gameObject);
                                m_TestState = TestState.Success;
                            }
                        }
                        if (currentTest != null && Time.time > m_StartTime + currentTest.GetTimeout())
                        {
                            m_TestState = TestState.Timeout;
                        }
                    }

                    switch (m_TestState)
                    {
                        case TestState.Success:
                            LogMessage(k_FinishedMessage);
                            FinishTest(TestResult.ResultType.Success);
                            break;
                        case TestState.Failure:
                            LogMessage(k_FailedMessage);
                            FinishTest(TestResult.ResultType.Failed);
                            break;
                        case TestState.Exception:
                            LogMessage(k_FailedExceptionMessage);
                            FinishTest(TestResult.ResultType.FailedException);
                            break;
                        case TestState.Timeout:
                            LogMessage(k_TimeoutMessage);
                            FinishTest(TestResult.ResultType.Timeout);
                            break;
                        case TestState.Ignored:
                            LogMessage(k_IgnoredMessage);
                            FinishTest(TestResult.ResultType.Ignored);
                            break;
                    }
                }
                yield return null;
            }
        }

        /// <summary>   Logs a message. </summary>
        ///
     
        ///
        /// <param name="message">  The message. </param>

        private void LogMessage(string message)
        {
            if (currentTest != null)
                Debug.Log(message + " (" + currentTest.Name + ")", currentTest.gameObject);
            else
                Debug.Log(message);
        }

        /// <summary>   Finishes test run. </summary>
        ///
     

        private void FinishTestRun()
        {
            PrintResultToLog();
            TestRunnerCallback.RunFinished(m_ResultList);
            LoadNextLevelOrQuit();
        }

        /// <summary>   Print result to log. </summary>
        ///
     

        private void PrintResultToLog()
        {
            var resultString = "";
            resultString += "Passed: " + m_ResultList.Count(t => t.IsSuccess);
            if (m_ResultList.Any(result => result.IsFailure))
            {
                resultString += " Failed: " + m_ResultList.Count(t => t.IsFailure);
                Debug.Log("Failed tests: " + string.Join(", ", m_ResultList.Where(t => t.IsFailure).Select(result => result.Name).ToArray()));
            }
            if (m_ResultList.Any(result => result.IsIgnored))
            {
                resultString += " Ignored: " + m_ResultList.Count(t => t.IsIgnored);
                Debug.Log("Ignored tests: " + string.Join(", ",
                                                          m_ResultList.Where(t => t.IsIgnored).Select(result => result.Name).ToArray()));
            }
            Debug.Log(resultString);
        }

        /// <summary>   Loads next level or quit. </summary>
        ///
     

        private void LoadNextLevelOrQuit()
        {
            if (isInitializedByRunner) return;


            TestSceneNumber += 1;
            string testScene = m_Configurator.GetIntegrationTestScenes(TestSceneNumber);

            if (testScene != null)
                SceneManager.LoadScene(Path.GetFileNameWithoutExtension(testScene));
            else
            {
                TestRunnerCallback.AllScenesFinished();
                k_ResultRenderer.ShowResults();

#if UNITY_EDITOR
                var prevScenes = m_Configurator.GetPreviousScenesToRestore();
                if(prevScenes!=null)
                {
                    UnityEditor.EditorBuildSettings.scenes = prevScenes;
                }
#endif

                if (m_Configurator.isBatchRun && m_Configurator.sendResultsOverNetwork)
                    Application.Quit();
            }
        }

        /// <summary>   Executes the graphical user interface action. </summary>
        ///
     

        public void OnGUI()
        {
            k_ResultRenderer.Draw();
        }

        /// <summary>   Tests start new. </summary>
        ///
     

        private void StartNewTest()
        {
            m_TestMessages = "";
            m_Stacktrace = "";
            m_TestState = TestState.Running;

            m_StartTime = Time.time;
            currentTest = m_TestsProvider.GetNextTest() as TestComponent;

            var testResult = m_ResultList.Single(result => result.TestComponent == currentTest);

            if (currentTest != null && currentTest.IsExludedOnThisPlatform())
            {
                m_TestState = TestState.Ignored;
                Debug.Log(currentTest.gameObject.name + " is excluded on this platform");
            }

            // don't ignore test if user initiated it from the runner and it's the only test that is being run
            if (currentTest != null
                && (currentTest.IsIgnored()
                    && !(isInitializedByRunner && m_ResultList.Count == 1)))
                m_TestState = TestState.Ignored;

            LogMessage(k_StartedMessage);
            TestRunnerCallback.TestStarted(testResult);
        }

        /// <summary>   Tests finish. </summary>
        ///
     
        ///
        /// <param name="result">   The result. </param>

        private void FinishTest(TestResult.ResultType result)
        {
            m_TestsProvider.FinishTest(currentTest);
            var testResult = m_ResultList.Single(t => t.GameObject == currentTest.gameObject);
            testResult.resultType = result;
            testResult.duration = Time.time - m_StartTime;
            testResult.messages = m_TestMessages;
            testResult.stacktrace = m_Stacktrace;
            TestRunnerCallback.TestFinished(testResult);
            currentTest = null;
            if (!testResult.IsSuccess
                && testResult.Executed
                && !testResult.IsIgnored) k_ResultRenderer.AddResults(SceneManager.GetActiveScene().name, testResult);
        }

        #region Test Runner Helpers

        /// <summary>   Gets test runner. </summary>
        ///
     
        ///
        /// <returns>   The test runner. </returns>

        public static TestRunner GetTestRunner()
        {
            TestRunner testRunnerComponent = null;
            var testRunnerComponents = Resources.FindObjectsOfTypeAll(typeof(TestRunner));

            if (testRunnerComponents.Count() > 1)
                foreach (var t in testRunnerComponents) DestroyImmediate(((TestRunner)t).gameObject);
            else if (!testRunnerComponents.Any())
                testRunnerComponent = Create().GetComponent<TestRunner>();
            else
                testRunnerComponent = testRunnerComponents.Single() as TestRunner;

            return testRunnerComponent;
        }

        /// <summary>   Creates a new GameObject. </summary>
        ///
     
        ///
        /// <returns>   A GameObject. </returns>

        private static GameObject Create()
        {
            var runner = new GameObject("TestRunner");
            runner.AddComponent<TestRunner>();
            Debug.Log("Created Test Runner");
            return runner;
        }

        /// <summary>   Query if this object is batch mode. </summary>
        ///
     
        ///
        /// <returns>   True if batch mode, false if not. </returns>

        private static bool IsBatchMode()
        {
#if !UNITY_METRO
            const string internalEditorUtilityClassName = "UnityEditorInternal.InternalEditorUtility, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

            var t = Type.GetType(internalEditorUtilityClassName, false);
            if (t == null) return false;

            const string inBatchModeProperty = "inBatchMode";
            var prop = t.GetProperty(inBatchModeProperty);
            return (bool)prop.GetValue(null, null);
#else   // if !UNITY_METRO
            return false;
#endif  // if !UNITY_METRO
        }

        #endregion

        /// <summary>   Values that represent test states. </summary>
        ///
     

        enum TestState
        {
            /// <summary>   An enum constant representing the running option. </summary>
            Running,
            /// <summary>   An enum constant representing the success option. </summary>
            Success,
            /// <summary>   An enum constant representing the failure option. </summary>
            Failure,
            /// <summary>   An enum constant representing the exception option. </summary>
            Exception,
            /// <summary>   An enum constant representing the timeout option. </summary>
            Timeout,
            /// <summary>   An enum constant representing the ignored option. </summary>
            Ignored
        }
    }
}
