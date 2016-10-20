// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\Editor\Renderer\IntegrationTestLine.cs
//
// summary:	Implements the integration test line class

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   An integration test line. </summary>
    ///
 

    class IntegrationTestLine : IntegrationTestRendererBase
    {
        /// <summary>   The results. </summary>
        public static List<TestResult> Results;
        /// <summary>   The result. </summary>
        protected TestResult m_Result;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="gameObject">   The game object. </param>
        /// <param name="testResult">   The test result. </param>

        public IntegrationTestLine(GameObject gameObject, TestResult testResult) : base(gameObject)
        {
            m_Result = testResult;
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
            if(Event.current.type != EventType.repaint)
				return;

			Styles.testName.Draw (rect, label, false, false, false, isSelected);

            if (m_Result.IsTimeout)
            {
				float min, max;
				Styles.testName.CalcMinMaxWidth(label, out min, out max);
				var timeoutRect = new Rect(rect);
				timeoutRect.x += min - 12;
				Styles.testName.Draw(timeoutRect, s_GUITimeoutIcon, false, false, false, isSelected);
            }
        }

        /// <summary>   Gets the result. </summary>
        ///
     
        ///
        /// <returns>   The result. </returns>

        protected internal override TestResult.ResultType GetResult()
        {
            if (!m_Result.Executed && test.ignored) return TestResult.ResultType.Ignored;
            return m_Result.resultType;
        }

        /// <summary>   Query if 'options' is visible. </summary>
        ///
     
        ///
        /// <param name="options">  Options for controlling the operation. </param>
        ///
        /// <returns>   True if visible, false if not. </returns>

        protected internal override bool IsVisible(RenderingOptions options)
        {
            if (!string.IsNullOrEmpty(options.nameFilter) && !m_GameObject.name.ToLower().Contains(options.nameFilter.ToLower())) return false;
            if (!options.showSucceeded && m_Result.IsSuccess) return false;
            if (!options.showFailed && m_Result.IsFailure) return false;
            if (!options.showNotRunned && !m_Result.Executed) return false;
            if (!options.showIgnored && test.ignored) return false;
            return true;
        }

        /// <summary>   Tests set current. </summary>
        ///
     
        ///
        /// <param name="tc">   The tc. </param>
        ///
        /// <returns>   True if the test passes, false if the test fails. </returns>

        public override bool SetCurrentTest(TestComponent tc)
        {
            m_IsRunning = test == tc;
            return m_IsRunning;
        }
    }
}
