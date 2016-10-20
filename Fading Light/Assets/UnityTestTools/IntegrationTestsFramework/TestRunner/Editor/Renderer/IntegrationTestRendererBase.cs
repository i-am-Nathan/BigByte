// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\Renderer\IntegrationTestRendererBase.cs
//
// summary:	Implements the integration test renderer base class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityTest
{
    /// <summary>   An integration test renderer base. </summary>
    ///
 

    public abstract class IntegrationTestRendererBase : IComparable<IntegrationTestRendererBase>
    {
        /// <summary>   The run test. </summary>
        public static Action<IList<ITestComponent>> RunTest;

        /// <summary>   True to refresh. </summary>
        protected static bool s_Refresh;

        /// <summary>   The graphical user interface run selected. </summary>
        private static readonly GUIContent k_GUIRunSelected = new GUIContent("Run Selected");
        /// <summary>   The graphical user interface run. </summary>
        private static readonly GUIContent k_GUIRun = new GUIContent("Run");
        /// <summary>   The graphical user interface delete. </summary>
        private static readonly GUIContent k_GUIDelete = new GUIContent("Delete");
        /// <summary>   The graphical user interface delete selected. </summary>
        private static readonly GUIContent k_GUIDeleteSelected = new GUIContent("Delete selected");

        /// <summary>   The graphical user interface timeout icon. </summary>
        protected static GUIContent s_GUITimeoutIcon = new GUIContent(Icons.StopwatchImg, "Timeout");

        /// <summary>   The game object. </summary>
        protected GameObject m_GameObject;
        /// <summary>   The test. </summary>
        public TestComponent test;
        /// <summary>   The name. </summary>
        private readonly string m_Name;

        /// <summary>   Specialised constructor for use only by derived class. </summary>
        ///
     
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="gameObject">   The game object. </param>

        protected IntegrationTestRendererBase(GameObject gameObject)
        {
            test = gameObject.GetComponent(typeof(TestComponent)) as TestComponent;
            if (test == null) throw new ArgumentException("Provided GameObject is not a test object");
            m_GameObject = gameObject;
            m_Name = test.Name;
        }

        /// <summary>
        /// Compares this IntegrationTestRendererBase object to another to determine their relative
        /// ordering.
        /// </summary>
        ///
     
        ///
        /// <param name="other">    Another instance to compare. </param>
        ///
        /// <returns>
        /// Negative if this object is less than the other, 0 if they are equal, or positive if this is
        /// greater.
        /// </returns>

        public int CompareTo(IntegrationTestRendererBase other)
        {
            return test.CompareTo(other.test);
        }

        /// <summary>   Renders this object. </summary>
        ///
     
        ///
        /// <param name="options">  Options for controlling the operation. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>

        public bool Render(RenderingOptions options)
        {
            s_Refresh = false;
            EditorGUIUtility.SetIconSize(new Vector2(15, 15));
            Render(0, options);
            EditorGUIUtility.SetIconSize(Vector2.zero);
            return s_Refresh;
        }

        /// <summary>   Renders this object. </summary>
        ///
     
        ///
        /// <param name="indend">   The indend. </param>
        /// <param name="options">  Options for controlling the operation. </param>

        protected internal virtual void Render(int indend, RenderingOptions options)
        {
            if (!IsVisible(options)) return;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indend * 10);

            var tempColor = GUI.color;
            if (m_IsRunning)
            {
                var frame = Mathf.Abs(Mathf.Cos(Time.realtimeSinceStartup * 4)) * 0.6f + 0.4f;
                GUI.color = new Color(1, 1, 1, frame);
            }

            var isSelected = Selection.gameObjects.Contains(m_GameObject);

            var value = GetResult();
            var icon = GetIconForResult(value);

            var label = new GUIContent(m_Name, icon);
            var labelRect = GUILayoutUtility.GetRect(label, EditorStyles.label, GUILayout.ExpandWidth(true), GUILayout.Height(18));

            OnLeftMouseButtonClick(labelRect);
            OnContextClick(labelRect);
            DrawLine(labelRect, label, isSelected, options);

            if (m_IsRunning) GUI.color = tempColor;
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>   Executes the select action. </summary>
        ///
     

        protected void OnSelect()
        {
			if (!Event.current.control && !Event.current.command) 
			{
				Selection.objects = new Object[0];
				GUIUtility.keyboardControl = 0;
			}

			if ((Event.current.control || Event.current.command) && Selection.gameObjects.Contains(test.gameObject))
                Selection.objects = Selection.gameObjects.Where(o => o != test.gameObject).ToArray();
            else
                Selection.objects = Selection.gameObjects.Concat(new[] { test.gameObject }).ToArray();
        }

        /// <summary>   Executes the left mouse button click action. </summary>
        ///
     
        ///
        /// <param name="rect"> The rectangle. </param>

        protected void OnLeftMouseButtonClick(Rect rect)
        {
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.mouseDown && Event.current.button == 0)
            {
                rect.width = 20;
                if (rect.Contains(Event.current.mousePosition)) return;
                Event.current.Use();
                OnSelect();
            }
        }

        /// <summary>   Executes the context click action. </summary>
        ///
     
        ///
        /// <param name="rect"> The rectangle. </param>

        protected void OnContextClick(Rect rect)
        {
            if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.ContextClick)
            {
                DrawContextMenu(test);
            }
        }

        /// <summary>   Draw context menu. </summary>
        ///
     
        ///
        /// <param name="testComponent">    The test component. </param>

        public static void DrawContextMenu(TestComponent testComponent)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            var selectedTests = Selection.gameObjects.Where(go => go.GetComponent(typeof(TestComponent)));
            var manySelected = selectedTests.Count() > 1;

            var m = new GenericMenu();
            if (manySelected)
            {
                // var testsToRun
                m.AddItem(k_GUIRunSelected, false, data => RunTest(selectedTests.Select(o => o.GetComponent(typeof(TestComponent))).Cast<ITestComponent>().ToList()), null);
            }
            m.AddItem(k_GUIRun, false, data => RunTest(new[] { testComponent }), null);
            m.AddSeparator("");
            m.AddItem(manySelected ? k_GUIDeleteSelected : k_GUIDelete, false, data => RemoveTests(selectedTests.ToArray()), null);
            m.ShowAsContext();
        }

        /// <summary>   Removes the tests described by testsToDelete. </summary>
        ///
     
        ///
        /// <param name="testsToDelete">    The tests to delete. </param>

        private static void RemoveTests(GameObject[] testsToDelete)
        {
            foreach (var t in testsToDelete)
            {
                Undo.DestroyObjectImmediate(t);
            }
        }

        /// <summary>   Gets icon for result. </summary>
        ///
     
        ///
        /// <param name="resultState">  State of the result. </param>
        ///
        /// <returns>   The icon for result. </returns>

        public static Texture GetIconForResult(TestResult.ResultType resultState)
        {
            switch (resultState)
            {
                case TestResult.ResultType.Success:
                    return Icons.SuccessImg;
                case TestResult.ResultType.Timeout:
                case TestResult.ResultType.Failed:
                case TestResult.ResultType.FailedException:
                    return Icons.FailImg;
                case TestResult.ResultType.Ignored:
                    return Icons.IgnoreImg;
                default:
                    return Icons.UnknownImg;
            }
        }

        /// <summary>   True if this object is running. </summary>
        protected internal bool m_IsRunning;

        /// <summary>   Draw line. </summary>
        ///
     
        ///
        /// <param name="rect">         The rectangle. </param>
        /// <param name="label">        The label. </param>
        /// <param name="isSelected">   True if this object is selected. </param>
        /// <param name="options">      Options for controlling the operation. </param>

        protected internal abstract void DrawLine(Rect rect, GUIContent label, bool isSelected, RenderingOptions options);

        /// <summary>   Gets the result. </summary>
        ///
     
        ///
        /// <returns>   The result. </returns>

        protected internal abstract TestResult.ResultType GetResult();

        /// <summary>   Query if 'options' is visible. </summary>
        ///
     
        ///
        /// <param name="options">  Options for controlling the operation. </param>
        ///
        /// <returns>   True if visible, false if not. </returns>

        protected internal abstract bool IsVisible(RenderingOptions options);

        /// <summary>   Tests set current. </summary>
        ///
     
        ///
        /// <param name="tc">   The tc. </param>
        ///
        /// <returns>   True if the test passes, false if the test fails. </returns>

        public abstract bool SetCurrentTest(TestComponent tc);
    }
}
