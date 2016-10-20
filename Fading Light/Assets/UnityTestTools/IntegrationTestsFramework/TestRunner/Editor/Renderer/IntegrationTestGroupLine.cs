// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\Renderer\IntegrationTestGroupLine.cs
//
// summary:	Implements the integration test group line class

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An integration test group line. </summary>
    ///
 

    class IntegrationTestGroupLine : IntegrationTestRendererBase
    {
        /// <summary>   The fold markers. </summary>
        public static List<GameObject> FoldMarkers;
        /// <summary>   The children. </summary>
        private IntegrationTestRendererBase[] m_Children;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="gameObject">   The game object. </param>

        public IntegrationTestGroupLine(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>   Draw line. </summary>
        ///
     
        ///
        /// <param name="rect">         The rectangle. </param>
        /// <param name="label">        The label. </param>
        /// <param name="isSelected">   True if this object is selected. </param>
        /// <param name="options">      Options for controlling the operation. </param>

        protected internal override void DrawLine(Rect rect, GUIContent label, bool isSelected, RenderingOptions options)
        {
            EditorGUI.BeginChangeCheck();
            var isClassFolded = !EditorGUI.Foldout(rect, !Folded, label, isSelected ? Styles.selectedFoldout : Styles.foldout);
            if (EditorGUI.EndChangeCheck()) Folded = isClassFolded;
        }

        /// <summary>   Gets or sets a value indicating whether the folded. </summary>
        ///
        /// <value> True if folded, false if not. </value>

        private bool Folded
        {
            get { return FoldMarkers.Contains(m_GameObject); }

            set
            {
                if (value) FoldMarkers.Add(m_GameObject);
                else FoldMarkers.RemoveAll(s => s == m_GameObject);
            }
        }

        /// <summary>   Renders this object. </summary>
        ///
     
        ///
        /// <param name="indend">   The indend. </param>
        /// <param name="options">  Options for controlling the operation. </param>

        protected internal override void Render(int indend, RenderingOptions options)
        {
            base.Render(indend, options);
            if (!Folded)
                foreach (var child in m_Children)
                    child.Render(indend + 1, options);
        }

        /// <summary>   Gets the result. </summary>
        ///
     
        ///
        /// <returns>   The result. </returns>

        protected internal override TestResult.ResultType GetResult()
        {
            bool ignored = false;
            bool success = false;
            foreach (var child in m_Children)
            {
                var result = child.GetResult();

                if (result == TestResult.ResultType.Failed || result == TestResult.ResultType.FailedException || result == TestResult.ResultType.Timeout)
                    return TestResult.ResultType.Failed;
                if (result == TestResult.ResultType.Success)
                    success = true;
                else if (result == TestResult.ResultType.Ignored)
                    ignored = true;
                else
                    ignored = false;
            }
            if (success) return TestResult.ResultType.Success;
            if (ignored) return TestResult.ResultType.Ignored;
            return TestResult.ResultType.NotRun;
        }

        /// <summary>   Query if 'options' is visible. </summary>
        ///
     
        ///
        /// <param name="options">  Options for controlling the operation. </param>
        ///
        /// <returns>   True if visible, false if not. </returns>

        protected internal override bool IsVisible(RenderingOptions options)
        {
            return m_Children.Any(c => c.IsVisible(options));
        }

        /// <summary>   Tests set current. </summary>
        ///
     
        ///
        /// <param name="tc">   The tc. </param>
        ///
        /// <returns>   True if the test passes, false if the test fails. </returns>

        public override bool SetCurrentTest(TestComponent tc)
        {
            m_IsRunning = false;
            foreach (var child in m_Children)
                m_IsRunning |= child.SetCurrentTest(tc);
            return m_IsRunning;
        }

        /// <summary>   Adds a children. </summary>
        ///
     
        ///
        /// <param name="parseTestList">    List of parse tests. </param>

        public void AddChildren(IntegrationTestRendererBase[] parseTestList)
        {
            m_Children = parseTestList;
        }
    }
}
