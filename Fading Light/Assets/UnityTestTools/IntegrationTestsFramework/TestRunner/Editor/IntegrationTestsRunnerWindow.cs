// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\IntegrationTestsRunnerWindow.cs
//
// summary:	Implements the integration tests runner Windows Form

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityTest.IntegrationTestRunner;
using UnityEngine.SceneManagement;

namespace UnityTest
{
    /// <summary>   (Serializable) form for viewing the integration tests runner. </summary>
    ///
 

    [Serializable]
    public class IntegrationTestsRunnerWindow : EditorWindow, IHasCustomMenu
    {
        #region GUI Contents
        /// <summary>   The graphical user interface create new test. </summary>
        private readonly GUIContent m_GUICreateNewTest = new GUIContent("Create", "Create new test");
        /// <summary>   The graphical user interface run selected tests. </summary>
        private readonly GUIContent m_GUIRunSelectedTests = new GUIContent("Run Selected", "Run selected test(s)");
        /// <summary>   The graphical user interface run all tests. </summary>
        private readonly GUIContent m_GUIRunAllTests = new GUIContent("Run All", "Run all tests");
        /// <summary>   The graphical user interface block user interface. </summary>
        private readonly GUIContent m_GUIBlockUI = new GUIContent("Block UI when running", "Block UI when running tests");
        /// <summary>   The graphical user interface pause on failure. </summary>
        private readonly GUIContent m_GUIPauseOnFailure = new GUIContent("Pause on test failure");
        #endregion

        #region runner steerign vars
        /// <summary>   The instance. </summary>
        private static IntegrationTestsRunnerWindow s_Instance;

        /// <summary>   Gets the tests to run. </summary>
        ///
        /// <value> The m tests to run. </value>

        [SerializeField] private List<GameObject> m_TestsToRun;

        /// <summary>   Gets the dynamic tests to run. </summary>
        ///
        /// <value> The m dynamic tests to run. </value>

        [SerializeField] private List<string> m_DynamicTestsToRun;

        /// <summary>   Gets a value indicating whether the ready to run. </summary>
        ///
        /// <value> True if ready to run, false if not. </value>

        [SerializeField] private bool m_ReadyToRun;
        /// <summary>   True if this object is building. </summary>
        private bool m_IsBuilding;
        /// <summary>   True to selected in hierarchy. </summary>
        public static bool selectedInHierarchy;
        /// <summary>   The horizontal split bar position. </summary>
        private float m_HorizontalSplitBarPosition = 200;

        /// <summary>   Gets the test list scroll. </summary>
        ///
        /// <value> The m test list scroll. </value>

        private Vector2 m_TestInfoScroll, m_TestListScroll;
        /// <summary>   The test lines. </summary>
        private IntegrationTestRendererBase[] m_TestLines;
        /// <summary>   Name of the currect scene. </summary>
        private string m_CurrectSceneName;
        /// <summary>   The filter settings. </summary>
        private TestFilterSettings m_FilterSettings;
        
        /// <summary>   Size of the result text. </summary>
        Vector2 m_resultTextSize;
        /// <summary>   The result text. </summary>
        string m_resultText;
        /// <summary>   The last selected go. </summary>
        GameObject m_lastSelectedGO;
        /// <summary>   Length of the result test maximum. </summary>
        int m_resultTestMaxLength = 15000;

        /// <summary>   Gets the selected line. </summary>
        ///
        /// <value> The m selected line. </value>

        [SerializeField] private GameObject m_SelectedLine;
        /// <summary>   List of results. </summary>
        [SerializeField] private List<TestResult> m_ResultList = new List<TestResult>();
        /// <summary>   The fold markers. </summary>
        [SerializeField] private List<GameObject> m_FoldMarkers = new List<GameObject>();

        /// <summary>   Options for controlling the operation. </summary>
        private IntegrationTestsRunnerSettings m_Settings;

        #endregion

        /// <summary>   Static constructor. </summary>
        ///
     

        static IntegrationTestsRunnerWindow()
        {
            InitBackgroundRunners();
        }

        /// <summary>   Init background runners. </summary>
        ///
     

        private static void InitBackgroundRunners()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemDraw;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemDraw;
            EditorApplication.hierarchyWindowChanged -= OnHierarchyChangeUpdate;
            EditorApplication.hierarchyWindowChanged += OnHierarchyChangeUpdate;
            EditorApplication.update -= BackgroundSceneChangeWatch;
            EditorApplication.update += BackgroundSceneChangeWatch;
            EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
            EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
        }

        /// <summary>   Executes the playmode state changed action. </summary>
        ///
     

        private static void OnPlaymodeStateChanged()
        {
            if (s_Instance && EditorApplication.isPlaying  == EditorApplication.isPlayingOrWillChangePlaymode)
                s_Instance.RebuildTestList();
        }

        /// <summary>   Executes the destroy action. </summary>
        ///
     

        public void OnDestroy()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemDraw;
            EditorApplication.update -= BackgroundSceneChangeWatch;
            EditorApplication.hierarchyWindowChanged -= OnHierarchyChangeUpdate;
            EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;

            TestComponent.DestroyAllDynamicTests();
        }

        /// <summary>   Background scene change watch. </summary>
        ///
     

        private static void BackgroundSceneChangeWatch()
        {
            if (!s_Instance) return;
            var currentScene = SceneManager.GetActiveScene().path;
            if (s_Instance.m_CurrectSceneName != null && s_Instance.m_CurrectSceneName == currentScene) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            TestComponent.DestroyAllDynamicTests();
            s_Instance.m_CurrectSceneName = currentScene;
            s_Instance.m_ResultList.Clear();
            s_Instance.RebuildTestList();
        }

        /// <summary>   Executes the enable action. </summary>
        ///
     

        public void OnEnable()
        {
            titleContent = new GUIContent("Integration Tests");
            s_Instance = this;

            m_Settings = ProjectSettingsBase.Load<IntegrationTestsRunnerSettings>();
            m_FilterSettings = new TestFilterSettings("UnityTest.IntegrationTestsRunnerWindow");

            InitBackgroundRunners();
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !m_ReadyToRun) RebuildTestList();
        }

        /// <summary>   Executes the selection change action. </summary>
        ///
     

        public void OnSelectionChange()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode
                || Selection.objects == null
                || Selection.objects.Length == 0) return;

            if (Selection.gameObjects.Length == 1)
            {
                var go = Selection.gameObjects.Single();
                var temp = go.transform;
                while (temp != null)
                {
                    var tc = temp.GetComponent<TestComponent>();
                    if (tc != null) break;
                    temp = temp.parent;
                }

                if (temp != null)
                {
                    SelectInHierarchy(temp.gameObject);
                    Selection.activeGameObject = temp.gameObject;
                    m_SelectedLine = temp.gameObject;
                }
            }
        }

        /// <summary>   Executes the hierarchy change update action. </summary>
        ///
     

        public static void OnHierarchyChangeUpdate()
        {
            if (!s_Instance || s_Instance.m_TestLines == null || EditorApplication.isPlayingOrWillChangePlaymode) return;

            // create a test runner if it doesn't exist
            TestRunner.GetTestRunner();

            // make tests are not places under a go that is not a test itself
            foreach (var test in TestComponent.FindAllTestsOnScene())
            {
                if (test.gameObject.transform.parent != null && test.gameObject.transform.parent.gameObject.GetComponent<TestComponent>() == null)
                {
                    test.gameObject.transform.parent = null;
                    Debug.LogWarning("Tests need to be on top of the hierarchy or directly under another test.");
                }
            }
            if (selectedInHierarchy) selectedInHierarchy = false;
            else s_Instance.RebuildTestList();
        }

        /// <summary>   Tests get result for. </summary>
        ///
     
        ///
        /// <param name="tc">   The tc. </param>
        ///
        /// <returns>   The result for test. </returns>

        public static TestResult GetResultForTest(TestComponent tc)
        {
            if(!s_Instance) return new TestResult(tc);
            return s_Instance.m_ResultList.FirstOrDefault(r => r.GameObject == tc.gameObject);
        }

        /// <summary>   Executes the hierarchy window item draw action. </summary>
        ///
     
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="rect"> The rectangle. </param>

        public static void OnHierarchyWindowItemDraw(int id, Rect rect)
        {
            var o = EditorUtility.InstanceIDToObject(id);
            if (o is GameObject)
            {
                var go = o as GameObject;

                if (Event.current.type == EventType.MouseDown
                    && Event.current.button == 0
                    && rect.Contains(Event.current.mousePosition))
                {
                    var temp = go.transform;
                    while (temp != null)
                    {
                        var c = temp.GetComponent<TestComponent>();
                        if (c != null) break;
                        temp = temp.parent;
                    }
                    if (temp != null) SelectInHierarchy(temp.gameObject);
                }
            }
        }

        /// <summary>   Select in hierarchy. </summary>
        ///
     
        ///
        /// <param name="gameObject">   The game object. </param>

        private static void SelectInHierarchy(GameObject gameObject)
        {
            if (!s_Instance) return;
            if (gameObject == s_Instance.m_SelectedLine && gameObject.activeInHierarchy) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!gameObject.activeSelf)
            {
                selectedInHierarchy = true;
                gameObject.SetActive(true);
            }

            var tests = TestComponent.FindAllTestsOnScene();
            var skipList = gameObject.GetComponentsInChildren(typeof(TestComponent), true).ToList();
            tests.RemoveAll(skipList.Contains);
            foreach (var test in tests)
            {
                var enable = test.GetComponentsInChildren(typeof(TestComponent), true).Any(c => c.gameObject == gameObject);
                if (test.gameObject.activeSelf != enable) test.gameObject.SetActive(enable);
            }
        }

        /// <summary>   Executes the tests. </summary>
        ///
     
        ///
        /// <param name="tests">    The tests. </param>

        private void RunTests(IList<ITestComponent> tests)
        {
            if (!tests.Any() || EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            FocusWindowIfItsOpen(GetType());

            var testComponents = tests.Where(t => t is TestComponent).Cast<TestComponent>().ToList();
            var dynaminTests = testComponents.Where(t => t.dynamic).ToList();
            m_DynamicTestsToRun = dynaminTests.Select(c => c.dynamicTypeName).ToList();
            testComponents.RemoveAll(dynaminTests.Contains);

            m_TestsToRun = testComponents.Select( tc => tc.gameObject ).ToList();

            m_ReadyToRun = true;
            TestComponent.DisableAllTests();

            EditorApplication.isPlaying = true;
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        public void Update()
        {
            if (m_ReadyToRun && EditorApplication.isPlaying)
            {
                m_ReadyToRun = false;
                var testRunner = TestRunner.GetTestRunner();
                testRunner.TestRunnerCallback.Add(new RunnerCallback(this));
                var testComponents = m_TestsToRun.Select(go => go.GetComponent<TestComponent>()).ToList();
                testRunner.InitRunner(testComponents, m_DynamicTestsToRun);
            }
        }

        /// <summary>   Rebuild test list. </summary>
        ///
     

        private void RebuildTestList()
        {
            m_TestLines = null;
            if (!TestComponent.AnyTestsOnScene() 
                && !TestComponent.AnyDynamicTestForCurrentScene()) return;

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                var dynamicTestsOnScene = TestComponent.FindAllDynamicTestsOnScene();
                var dynamicTestTypes = TestComponent.GetTypesWithHelpAttribute(SceneManager.GetActiveScene().path);

                foreach (var dynamicTestType in dynamicTestTypes)
                {
                    var existingTests = dynamicTestsOnScene.Where(component => component.dynamicTypeName == dynamicTestType.AssemblyQualifiedName);
                    if (existingTests.Any())
                    {
                        var testComponent = existingTests.Single();
                        foreach (var c in testComponent.gameObject.GetComponents<Component>())
                        {
                            var type = Type.GetType(testComponent.dynamicTypeName);
                            if (c is TestComponent || c is Transform || type.IsInstanceOfType(c)) continue;
                            DestroyImmediate(c);
                        }
                        dynamicTestsOnScene.Remove(existingTests.Single());
                        continue;
                    }
                    TestComponent.CreateDynamicTest(dynamicTestType);
                }

                foreach (var testComponent in dynamicTestsOnScene)
                    DestroyImmediate(testComponent.gameObject);
            }

            var topTestList = TestComponent.FindAllTopTestsOnScene();

            var newResultList = new List<TestResult>();
            m_TestLines = ParseTestList(topTestList, newResultList);

            var oldDynamicResults = m_ResultList.Where(result => result.dynamicTest);
            foreach (var oldResult in m_ResultList)
            {
                var result = newResultList.Find(r => r.Id == oldResult.Id);
                if (result == null) continue;
                result.Update(oldResult);
            }
            newResultList.AddRange(oldDynamicResults.Where(r => !newResultList.Contains(r)));
            m_ResultList = newResultList;

            IntegrationTestRendererBase.RunTest = RunTests;
            IntegrationTestGroupLine.FoldMarkers = m_FoldMarkers;
            IntegrationTestLine.Results = m_ResultList;
            
            m_FilterSettings.UpdateCounters(m_ResultList.Cast<ITestResult>());

            m_FoldMarkers.RemoveAll(o => o == null);

            selectedInHierarchy = true;
            Repaint();
        }

        /// <summary>   Parse test list. </summary>
        ///
     
        ///
        /// <param name="testList"> List of tests. </param>
        /// <param name="results">  The results. </param>
        ///
        /// <returns>   An IntegrationTestRendererBase[]. </returns>

        private IntegrationTestRendererBase[] ParseTestList(List<TestComponent> testList, List<TestResult> results)
        {
            var tempList = new List<IntegrationTestRendererBase>();
            foreach (var testObject in testList)
            {
                if (!testObject.IsTestGroup())
                {
                    var result = new TestResult(testObject);
                    if (results != null)
                        results.Add(result);
                    tempList.Add(new IntegrationTestLine(testObject.gameObject, result));
                    continue;
                }
                var group = new IntegrationTestGroupLine(testObject.gameObject);
                var children = testObject.gameObject.GetComponentsInChildren(typeof(TestComponent), true).Cast<TestComponent>().ToList();
                children = children.Where(c => c.gameObject.transform.parent == testObject.gameObject.transform).ToList();
                group.AddChildren(ParseTestList(children, results));
                tempList.Add(group);
            }
            tempList.Sort();
            return tempList.ToArray();
        }

        /// <summary>   Executes the graphical user interface action. </summary>
        ///
     

        public void OnGUI()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                m_IsBuilding = true;
            }
            else if (m_IsBuilding)
            {
                m_IsBuilding = false;
                Repaint();
            }

            PrintHeadPanel();

            EditorGUILayout.BeginVertical(Styles.testList);
            m_TestListScroll = EditorGUILayout.BeginScrollView(m_TestListScroll);
            bool repaint = PrintTestList(m_TestLines);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            RenderDetails();

            if (repaint) Repaint();
        }

        /// <summary>   Print head panel. </summary>
        ///
     

        public void PrintHeadPanel()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            if (GUILayout.Button(m_GUIRunAllTests, EditorStyles.toolbarButton))
            {
                RunTests(TestComponent.FindAllTestsOnScene().Cast<ITestComponent>().ToList());
            }
            EditorGUI.BeginDisabledGroup(!Selection.gameObjects.Any (t => t.GetComponent(typeof(ITestComponent))));
            if (GUILayout.Button(m_GUIRunSelectedTests, EditorStyles.toolbarButton))
            {
                RunTests(Selection.gameObjects.Select(t => t.GetComponent(typeof(TestComponent))).Cast<ITestComponent>().ToList());
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button(m_GUICreateNewTest, EditorStyles.toolbarButton))
            {
                var test = TestComponent.CreateTest();
                if (Selection.gameObjects.Length == 1
                    && Selection.activeGameObject != null
                    && Selection.activeGameObject.GetComponent<TestComponent>())
                {
                    test.transform.parent = Selection.activeGameObject.transform.parent;
                }
                Selection.activeGameObject = test;
                RebuildTestList();
            }
            EditorGUI.EndDisabledGroup();
            
            GUILayout.FlexibleSpace ();
            
            m_FilterSettings.OnGUI ();
            
            EditorGUILayout.EndHorizontal ();
        }

        /// <summary>   Adds the items to menu. </summary>
        ///
     
        ///
        /// <param name="menu"> The menu. </param>

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(m_GUIBlockUI, m_Settings.blockUIWhenRunning, m_Settings.ToggleBlockUIWhenRunning);
            menu.AddItem(m_GUIPauseOnFailure, m_Settings.pauseOnTestFailure, m_Settings.TogglePauseOnTestFailure);
        }

        /// <summary>   Print test list. </summary>
        ///
     
        ///
        /// <param name="renderedLines">    The rendered lines. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        private bool PrintTestList(IntegrationTestRendererBase[] renderedLines)
        {
            if (renderedLines == null) return false;

            var filter = m_FilterSettings.BuildRenderingOptions();

            bool repaint = false;
            foreach (var renderedLine in renderedLines)
            {
                repaint |= renderedLine.Render(filter);
            }
            return repaint;
        }

        /// <summary>   Renders the details. </summary>
        ///
     

        private void RenderDetails()
        {
            var ctrlId = GUIUtility.GetControlID(FocusType.Passive);

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y = rect.height + rect.y - 1;
            rect.height = 3;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);
            var e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (GUIUtility.hotControl == 0 && rect.Contains(e.mousePosition))
                        GUIUtility.hotControl = ctrlId;
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == ctrlId)
                    {
                        m_HorizontalSplitBarPosition -= e.delta.y;
                        if (m_HorizontalSplitBarPosition < 20) m_HorizontalSplitBarPosition = 20;
                        Repaint();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == ctrlId)
                        GUIUtility.hotControl = 0;
                    break;
            }

            m_TestInfoScroll = EditorGUILayout.BeginScrollView(m_TestInfoScroll, GUILayout.MinHeight(m_HorizontalSplitBarPosition));

            if (m_SelectedLine != null)
                UpdateResultText(m_SelectedLine);

            EditorGUILayout.SelectableLabel(m_resultText, Styles.info, 
                                            GUILayout.ExpandHeight(true), 
                                            GUILayout.ExpandWidth(true), 
                                            GUILayout.MinWidth(m_resultTextSize.x), 
                                            GUILayout.MinHeight(m_resultTextSize.y));
            EditorGUILayout.EndScrollView();
        }

        /// <summary>   Updates the result text described by go. </summary>
        ///
     
        ///
        /// <param name="go">   The go. </param>

        private void UpdateResultText(GameObject go)
        {
            if(go == m_lastSelectedGO) return;
            m_lastSelectedGO = go;
            var result = m_ResultList.Find(r => r.GameObject == go);
            if (result == null)
            {
                m_resultText = string.Empty;
                m_resultTextSize = Styles.info.CalcSize(new GUIContent(string.Empty));
                return;
            }
            var sb = new StringBuilder(result.Name.Trim());
            if (!string.IsNullOrEmpty(result.messages))
            {
                sb.Append("\n---\n");
                sb.Append(result.messages.Trim());
            }
            if (!string.IsNullOrEmpty(result.stacktrace))
            {
                sb.Append("\n---\n");
                sb.Append(result.stacktrace.Trim());
            }
            if(sb.Length>m_resultTestMaxLength)
            {
                sb.Length = m_resultTestMaxLength;
                sb.AppendFormat("...\n\n---MESSAGE TRUNCATED AT {0} CHARACTERS---", m_resultTestMaxLength);
            }
            m_resultText = sb.ToString().Trim();
            m_resultTextSize = Styles.info.CalcSize(new GUIContent(m_resultText));
        }

        /// <summary>   Executes the inspector update action. </summary>
        ///
     

        public void OnInspectorUpdate()
        {
            if (focusedWindow != this) Repaint();
        }

        /// <summary>   Tests set current. </summary>
        ///
     
        ///
        /// <param name="tc">   The tc. </param>

        private void SetCurrentTest(TestComponent tc)
        {
            foreach (var line in m_TestLines)
                line.SetCurrentTest(tc);
        }

        /// <summary>   (Serializable) a runner callback. </summary>
        ///
     

        class RunnerCallback : ITestRunnerCallback
        {
            /// <summary>   The window. </summary>
            private readonly IntegrationTestsRunnerWindow m_Window;
            /// <summary>   The test number. </summary>
            private int m_TestNumber;
            /// <summary>   The current test number. </summary>
            private int m_CurrentTestNumber;

            /// <summary>   True to console error on pause value. </summary>
            private readonly bool m_ConsoleErrorOnPauseValue;
            /// <summary>   True to run in background. </summary>
            private readonly bool m_RunInBackground;
            /// <summary>   The current test. </summary>
            private TestComponent m_CurrentTest;

            /// <summary>   Constructor. </summary>
            ///
         
            ///
            /// <param name="window">   The window. </param>

            public RunnerCallback(IntegrationTestsRunnerWindow window)
            {
                m_Window = window;

                m_ConsoleErrorOnPauseValue = GuiHelper.GetConsoleErrorPause();
                GuiHelper.SetConsoleErrorPause(false);
                m_RunInBackground = PlayerSettings.runInBackground;
                PlayerSettings.runInBackground = true;
            }

            /// <summary>   Executes the started operation. </summary>
            ///
         
            ///
            /// <param name="platform">     The platform. </param>
            /// <param name="testsToRun">   The tests to run. </param>

            public void RunStarted(string platform, List<TestComponent> testsToRun)
            {
                EditorApplication.update += OnEditorUpdate;
                m_TestNumber = testsToRun.Count;
                foreach (var test in testsToRun)
                {
                    var result = m_Window.m_ResultList.Find(r => r.TestComponent == test);
                    if (result != null) result.Reset();
                }
            }

            /// <summary>   Executes the finished operation. </summary>
            ///
         
            ///
            /// <param name="testResults">  The test results. </param>

            public void RunFinished(List<TestResult> testResults)
            {
                m_Window.SetCurrentTest(null);
                m_CurrentTest = null;
                EditorApplication.update -= OnEditorUpdate;
                EditorApplication.isPlaying = false;
                EditorUtility.ClearProgressBar();
                GuiHelper.SetConsoleErrorPause(m_ConsoleErrorOnPauseValue);
                PlayerSettings.runInBackground = m_RunInBackground;
            }

            /// <summary>   All scenes finished. </summary>
            ///
         

            public void AllScenesFinished()
            {

            }

            /// <summary>   Tests started. </summary>
            ///
         
            ///
            /// <param name="test"> The test. </param>

            public void TestStarted(TestResult test)
            {
                m_Window.SetCurrentTest(test.TestComponent);
                m_CurrentTest = test.TestComponent;
            }

            /// <summary>   Tests finished. </summary>
            ///
         
            ///
            /// <param name="test"> The test. </param>

            public void TestFinished(TestResult test)
            {
                m_CurrentTestNumber++;

                var result = m_Window.m_ResultList.Find(r => r.Id == test.Id);
                if (result != null)
                    result.Update(test);
                else
                    m_Window.m_ResultList.Add(test);
                    
                if(test.IsFailure && m_Window.m_Settings.pauseOnTestFailure)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.isPaused = true;
                }
            }

            /// <summary>   Tests run interrupted. </summary>
            ///
         
            ///
            /// <param name="testsNotRun">  The tests not run. </param>

            public void TestRunInterrupted(List<ITestComponent> testsNotRun)
            {
                Debug.Log("Test run interrupted");
                RunFinished(new List<TestResult>());
            }

            /// <summary>   Executes the editor update action. </summary>
            ///
         

            private void OnEditorUpdate()
            {
                if(!EditorApplication.isPlaying) 
                {
                    TestRunInterrupted(null);
                    return;
                }

                if (m_Window.m_Settings.blockUIWhenRunning 
                    && m_CurrentTest != null 
                    && !EditorApplication.isPaused 
                    && EditorUtility.DisplayCancelableProgressBar("Integration Test Runner",
                                                                  "Running " + m_CurrentTest.Name,
                                                                  (float)m_CurrentTestNumber / m_TestNumber))
                {
                    TestRunInterrupted(null);
                }
            }
        }

        /// <summary>   Shows the window. </summary>
        ///
     
        ///
        /// <returns>   An IntegrationTestsRunnerWindow. </returns>

        [MenuItem("Unity Test Tools/Integration Test Runner %#&t")]
        public static IntegrationTestsRunnerWindow ShowWindow()
        {
            var w = GetWindow(typeof(IntegrationTestsRunnerWindow));
            w.Show();
            return w as IntegrationTestsRunnerWindow;
        }
    }
}
